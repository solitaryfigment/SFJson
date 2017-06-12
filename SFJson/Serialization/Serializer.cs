using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace SFJson
{
    public class Serializer
    {
        private SerializerSettings _serializerSettings;
        private StringBuilder _serialized;

        public string Serialize(object objectToSerialize)
        {
            return Serialize(objectToSerialize, new SerializerSettings { TypeHandler = TypeHandler.None });
        }

        public string Serialize(object objectToSerialize, SerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
            _serialized = new StringBuilder();
            SerializeObject(objectToSerialize);
            return _serialized.ToString();
        }

        private void SerializeObject(object obj)
        {
            if(obj == null) 
            {
                _serialized.Append("null");
            }
            else 
            {
                _serialized.Append("{");
                AppendType(obj, TypeHandler.Objects);
                SerializeMembers(obj);
                _serialized.Append("}");
            }
        }
        
        private void SerializeList(IList list)
        {
            var appendSeparator = false;

            if(list == null)
            {
                _serialized.Append("null");
            }
            else
            {
                foreach(var element in list)
                {
                    if(appendSeparator)
                    {
                        _serialized.Append(",");
                    }
                    SerializeObject(element.GetType(), element);
                    appendSeparator = true;
                }
            }
        }

        private void SerializeMembers(object obj)
        {
            var fieldInfos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            var propertyInfos = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var appendSeparator = _serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Objects;
            
            foreach(var fieldInfo in fieldInfos)
            {
                if(appendSeparator)
                {
                    _serialized.Append(",");
                }
                _serialized.AppendFormat("\"{0}\":", fieldInfo.Name);
                SerializeObject(fieldInfo.FieldType, fieldInfo.GetValue(obj));
                appendSeparator = true;
            }
            foreach(var propertyInfo in propertyInfos)
            {
                if(propertyInfo.CanWrite && propertyInfo.CanRead)
                {
                    if(appendSeparator)
                    {
                        _serialized.Append(",");
                    }
                    _serialized.AppendFormat("\"{0}\":", propertyInfo.Name);
                    SerializeObject(propertyInfo.PropertyType, propertyInfo.GetValue(obj));
                    appendSeparator = true;
                }
            }
        }

        private void SerializeObject(Type type, object value)
        {
            if(type.IsPrimitive)
            {
                _serialized.AppendFormat("{0}", value);
            }
            else if(type.IsArray || type.GetInterface("IList") != null)
            {
                _serialized.Append("[");
                AppendType(value, TypeHandler.Collections, ",\"$values\":[");
                
                SerializeList((IList)value);
                
                if(_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Collections)
                {
                    _serialized.Append("]");
                }
                _serialized.Append("]");
            }
            else if(type.IsEnum)
            {
                _serialized.AppendFormat("{0}", value);
            }
            else if(type == typeof(string))
            {
                _serialized.AppendFormat("\"{0}\"", value);
            }
            else
            {
                SerializeObject(value);
            }
        }

        private void AppendType(object obj, TypeHandler typeHandler, string appendString = "")
        {
            if(_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == typeHandler)
            {
                _serialized.AppendFormat("\"$type\":\"{0}\"", obj.GetType().GetTypeAsString());
                _serialized.Append(appendString);
            }
        }
    }
}