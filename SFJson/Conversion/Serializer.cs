using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using SFJson.Attributes;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Utils;

namespace SFJson.Conversion
{
    /// <summary>
    /// Handles converting an <c>object</c> to a JSON string.
    /// </summary>
    public class Serializer
    {
        private SettingsManager _settingsManager;
        private StringBuilder _serialized;

        /// <summary>
        /// Convets an <c>object</c> to a JSON string.
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <param name="serializerSettings"></param>
        /// <returns>
        /// The converted JSON string
        /// </returns>
        /// <exception cref="SerializationException"></exception>
        public string Serialize(object objectToSerialize, SerializerSettings serializerSettings = null)
        {
            _settingsManager = new SettingsManager { SerializationSettings = serializerSettings };
            _serialized = new StringBuilder();
            try
            {
                SerializeObject(objectToSerialize.GetType(), objectToSerialize);
            }
            catch(Exception e)
            {
                throw new SerializationException("Error during serialization.", e);
            }

            return _serialized.ToString();
        }

        private void SerializeObject(object obj)
        {
            if(obj != null)
            {
                _serialized.Append(Constants.OPEN_CURLY);
                AppendType(obj, SerializationTypeHandle.Objects);
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
                    var s = new SerializerSettings
                    {
                        DateTimeFormat = _settingsManager.DateTimeFormat,
                        DateTimeOffsetFormat = _settingsManager.DateTimeOffsetFormat,
                        SerializationTypeHandle = _settingsManager.SerializationTypeHandle
                    };
                    AppendSeparator(appendSeparator);
                    SerializeObject(key.GetType(), key);
                    _serialized.Append(Constants.COLON);
                    SerializeObject(dictionary[key].GetType(), dictionary[key]);
                    appendSeparator = true;
                }

                return;
            }

            _serialized.Append(Constants.NULL);
        }

        private void SerializeList(IEnumerable list)
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
            var appendSeparator = _settingsManager.SerializationTypeHandle == SerializationTypeHandle.All || _settingsManager.SerializationTypeHandle == SerializationTypeHandle.Objects;
            foreach(var fieldInfo in fieldInfos)
            {
                if(SerializeMember(fieldInfo, fieldInfo.FieldType, fieldInfo.GetValue(obj), appendSeparator))
                {
                    appendSeparator = true;
                }
            }
            foreach(var propertyInfo in propertyInfos)
            {
                if(!(propertyInfo.CanWrite && propertyInfo.CanRead))
                {
                    continue;
                }

                try
                {
                    if(SerializeMember(propertyInfo, propertyInfo.PropertyType, propertyInfo.GetValue(obj, null), appendSeparator))
                    {
                        appendSeparator = true;
                    }
                }
                catch(TargetParameterCountException)
                {
                    // TODO: Handle better -- Ignore
                }
            }
        }

        private bool SerializeMember(MemberInfo memberInfo, Type type, object value, bool appendSeparator)
        {
            var ignoreAttribute = (JsonIgnore) memberInfo.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonIgnore));
            if(ignoreAttribute == null)
            {
                var attribute = (JsonNamedValue) memberInfo.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonNamedValue));
                var memberName = (attribute != null) ? attribute.Name : memberInfo.Name;
                AppendSeparator(appendSeparator);
                AppendAsString(memberName);
                _serialized.Append(Constants.COLON);
                SerializeObject(type, value);
                return true;
            }

            return false;
        }

        private void SerializeObject(Type type, object value)
        {
            type = type.IsInterface ? value.GetType() : type;
            if(value == null)
            {
                _serialized.AppendFormat(Constants.NULL);
            }
            else if(type.IsPrimitive || type.IsEnum || value is decimal)
            {
                var writeValue = value.ToString();
                if(value is bool)
                {
                    writeValue = writeValue.ToLower();
                }
                _serialized.AppendFormat("{0}", writeValue);
            }
            else if(value is DateTimeOffset)
            {
                var indexAndFormat = string.Format("{0}{1}{2}", "{0:", _settingsManager.DateTimeOffsetFormat, "}");
                AppendAsString(string.Format(indexAndFormat, value));
            }
            else if(value is DateTime)
            {
                var indexAndFormat = string.Format("{0}{1}{2}", "{0:", _settingsManager.DateTimeFormat, "}");
                AppendAsString(string.Format(indexAndFormat, value));
            }
            else if(value is TimeSpan)
            {
                AppendAsString(value.ToString());
            }
            else if(value is Type)
            {
                AppendAsString(((Type) value).GetTypeAsString());
            }
            else if(type == typeof(string))
            {
                AppendAsString(((string) value).EscapeQuotes());
            }
            else if(type == typeof(Guid))
            {
                AppendAsString(value.ToString());
            }
            else if(type.Implements(typeof(IDictionary)) || (type.GetGenericArguments().Length == 2 && type.Implements(typeof(IEnumerable))))
            {
                if(type.Implements(typeof(IDictionary)))
                {
                    _serialized.Append(Constants.OPEN_CURLY);
                    AppendType(value, SerializationTypeHandle.Collections, Constants.COMMA.ToString());
                    SerializeDictionary((IDictionary)value);
                    _serialized.Append(Constants.CLOSE_CURLY);
                }
                else
                {
                    Console.WriteLine("Need to do something else");
                }
            }
            else if(type.IsArray || type.Implements(typeof(IEnumerable)))
            {
                if(_settingsManager.SerializationTypeHandle == SerializationTypeHandle.All || _settingsManager.SerializationTypeHandle == SerializationTypeHandle.Collections)
                {
                    _serialized.Append(Constants.OPEN_CURLY);
                    AppendType(value, SerializationTypeHandle.Collections, ",\"$values\":[");
                    SerializeList((IEnumerable) value);
                    _serialized.Append(Constants.CLOSE_BRACKET);
                    _serialized.Append(Constants.CLOSE_CURLY);
                }
                else
                {
                    _serialized.Append(Constants.OPEN_BRACKET);
                    SerializeList((IEnumerable) value);
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
            _serialized.AppendFormat("\"{0}\"", value);
        }

        private void AppendType(object obj, SerializationTypeHandle serializationTypeHandle, string appendString = "")
        {
            if(_settingsManager.SerializationTypeHandle == SerializationTypeHandle.All || _settingsManager.SerializationTypeHandle == serializationTypeHandle)
            {
                AppendAsString("$type");
                _serialized.Append(Constants.COLON);
                AppendAsString(obj.GetType().GetTypeAsString());
                _serialized.Append(appendString);
            }
        }
    }
}
