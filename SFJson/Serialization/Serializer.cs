﻿using System;
using System.Collections;
using System.Collections.Generic;
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
                    if(appendSeparator)
                    {
                        _serialized.Append(Constants.COMMA);
                    }
                    _serialized.Append("{");
                    _serialized.AppendFormat("\"{0}\"", new Serializer().Serialize(key, _serializerSettings));
                    _serialized.Append(":");
                    SerializeObject(dictionary[key].GetType(), dictionary[key]);
                    _serialized.Append("}");
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
                    if(appendSeparator)
                    {
                        _serialized.Append(Constants.COMMA);
                    }
                    SerializeObject(element.GetType(), element);
                    appendSeparator = true;
                }
                return;
            }
            _serialized.Append(Constants.NULL);
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
                    _serialized.Append(Constants.COMMA);
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
                        _serialized.Append(Constants.COMMA);
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
                Console.WriteLine("Primitive");
                _serialized.AppendFormat("{0}", value);
            }
            else if(type.Implements(typeof(IDictionary)))
            {
                Console.WriteLine("Dictionary");
                _serialized.Append(Constants.OPEN_BRACKET);
                AppendType(value, TypeHandler.Collections, ",\"$values\":[");
                
                SerializeDictionary((IDictionary)value);
                
                if(_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Collections)
                {
                    _serialized.Append(Constants.CLOSE_BRACKET);
                }
                _serialized.Append(Constants.CLOSE_BRACKET);
            }
            else if(type.IsArray || type.GetInterface("IList") != null)
            {
                Console.WriteLine("List");
                _serialized.Append(Constants.OPEN_BRACKET);
                AppendType(value, TypeHandler.Collections, ",\"$values\":[");
                
                SerializeList((IList)value);
                
                if(_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Collections)
                {
                    _serialized.Append(Constants.CLOSE_BRACKET);
                }
                _serialized.Append(Constants.CLOSE_BRACKET);
            }
            else if(type.IsEnum)
            {
                Console.WriteLine("Enum");
                _serialized.AppendFormat("{0}", value);
            }
            else if(type == typeof(string))
            {
                Console.WriteLine("String");
                _serialized.AppendFormat("\"{0}\"", value);
            }
            else
            {
                Console.WriteLine("ELSE");
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
