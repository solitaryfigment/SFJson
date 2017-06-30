using System;
using System.Collections;
using System.Reflection;
using System.Text;
using SFJson.Attributes;

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
            SerializeObject(objectToSerialize.GetType(), objectToSerialize);
            return _serialized.ToString();
        }

        private void SerializeObject(object obj)
        {
            if(obj != null) 
            {
                _serialized.Append(Constants.OPEN_CURLY);
                AppendType(obj, TypeHandler.Objects);
                SerializeMembers(obj);
                _serialized.Append(Constants.CLOSE_CURLY);
                return;
            }
            _serialized.Append(Constants.NULL);
        }

        private void SerializeDictionary(IDictionary dictionary)
        {
            var appendSeparator = false;

            if(dictionary != null)
            {
                foreach(var key in dictionary.Keys)
                {
                    var s = new SerializerSettings()
                    {
                        TypeHandler = _serializerSettings.TypeHandler,
                        PropertyStringEscape = true
                    };
                    AppendSeparator(appendSeparator);
                    _serialized.AppendFormat("\"{0}\"", new Serializer().Serialize(key, s));
                    _serialized.Append(Constants.COLON);
                    SerializeObject(dictionary[key].GetType(), dictionary[key]);
                    appendSeparator = true;
                }
                return;
            }
            _serialized.Append(Constants.NULL);
        }

        private void SerializeList(IList list)
        {
            var appendSeparator = false;

            if(list != null)
            {
                foreach(var element in list)
                {
                    AppendSeparator(appendSeparator);
                    SerializeObject(element.GetType(), element);
                    appendSeparator = true;
                }
                return;
            }
            _serialized.Append(Constants.NULL);
        }

        private void AppendSeparator(bool appendSeparator)
        {
            if(appendSeparator)
            {
                _serialized.Append(Constants.COMMA);
            }
        }

        private void SerializeMembers(object obj)
        {
            var fieldInfos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            var propertyInfos = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var appendSeparator = _serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Objects;
            
            foreach(var fieldInfo in fieldInfos)
            {
                SerializeMember(fieldInfo, fieldInfo.FieldType, fieldInfo.GetValue(obj), appendSeparator);
                appendSeparator = true;
            }
            foreach(var propertyInfo in propertyInfos)
            {
                if(propertyInfo.CanWrite && propertyInfo.CanRead)
                {
                    SerializeMember(propertyInfo, propertyInfo.PropertyType, propertyInfo.GetValue(obj), appendSeparator);
                    appendSeparator = true;
                }
            }
        }

        private void SerializeMember(MemberInfo memberInfo, Type type, object value, bool appendSeparator)
        {
            var attribute = memberInfo.GetCustomAttribute<JsonValueName>();
            var memberName = (attribute != null) ? attribute.Name : memberInfo.Name;
            AppendSeparator(appendSeparator);
            AppendAsString(memberName);
            _serialized.Append(Constants.COLON);
            SerializeObject(type, value);
        }
        
        private void SerializeObject(Type type, object value)
        {
            if(type.IsPrimitive || type.IsEnum || value is decimal)
            {
                _serialized.AppendFormat("{0}", value);
            }
            else if (value is TimeSpan || value is DateTime || value is DateTimeOffset)
            {
                AppendAsString(value.ToString());
            }
            else if (type == typeof(string))
            {
                AppendAsString(((string)value).EscapeQuotes());
            }
            else if (type.Implements(typeof(IDictionary)))
            {
                _serialized.Append(Constants.OPEN_CURLY);
                AppendType(value, TypeHandler.Collections, Constants.COMMA.ToString());
                SerializeDictionary((IDictionary) value);
                _serialized.Append(Constants.CLOSE_CURLY);
            }
            else if (type.IsArray || type.Implements(typeof(IList)))
            {
                if (_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Collections)
                {
                    _serialized.Append(Constants.OPEN_CURLY);
                    AppendType(value, TypeHandler.Collections, ",\"$values\":[");
                    SerializeList((IList) value);
                    _serialized.Append(Constants.CLOSE_BRACKET);
                    _serialized.Append(Constants.CLOSE_CURLY);
                }
                else
                {
                    _serialized.Append(Constants.OPEN_BRACKET);
                    SerializeList((IList) value);
                    _serialized.Append(Constants.CLOSE_BRACKET);
                }
            }
            else
            {
               SerializeObject(value);
            }
        }

        private void AppendAsString(string value)
        {
            if (_serializerSettings.PropertyStringEscape)
            {
                _serialized.AppendFormat("\\\"{0}\\\"", value);
            }
            else
            {
                _serialized.AppendFormat("\"{0}\"", value);
            }
        }

        private void AppendType(object obj, TypeHandler typeHandler, string appendString = "")
        {
            if(_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == typeHandler)
            {
                AppendAsString("$type");
                _serialized.Append(Constants.COLON);
                AppendAsString(obj.GetType().GetTypeAsString());
                _serialized.Append(appendString);
            }
        }
    }
}
