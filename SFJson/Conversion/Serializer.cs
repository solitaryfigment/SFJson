using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using SFJson.Attributes;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Utils;

namespace SFJson.Conversion
{
    public class Serializer
    {
        private SerializerSettings _serializerSettings;
        private StringBuilder _serialized;

        public string Serialize(object objectToSerialize)
        {
            return Serialize(objectToSerialize, new SerializerSettings {SerializationTypeHandle = SerializationTypeHandle.None});
        }

        public string Serialize(object objectToSerialize, SerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
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
                    var s = new SerializerSettings()
                    {
                        SerializationTypeHandle = _serializerSettings.SerializationTypeHandle,
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
            var appendSeparator = _serializerSettings.SerializationTypeHandle == SerializationTypeHandle.All || _serializerSettings.SerializationTypeHandle == SerializationTypeHandle.Objects;
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
                var indexAndFormat = string.Format("{0}{1}{2}", "{0:", _serializerSettings.DateTimeOffsetFormat, "}");
                AppendAsString(string.Format(indexAndFormat, value));
            }
            else if(value is DateTime)
            {
                var indexAndFormat = string.Format("{0}{1}{2}", "{0:", _serializerSettings.DateTimeFormat, "}");
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
            else if(type.Implements(typeof(IDictionary)))
            {
                _serialized.Append(Constants.OPEN_CURLY);
                AppendType(value, SerializationTypeHandle.Collections, Constants.COMMA.ToString());
                SerializeDictionary((IDictionary) value);
                _serialized.Append(Constants.CLOSE_CURLY);
            }
            else if(type.IsArray || type.Implements(typeof(IList)))
            {
                if(_serializerSettings.SerializationTypeHandle == SerializationTypeHandle.All || _serializerSettings.SerializationTypeHandle == SerializationTypeHandle.Collections)
                {
                    _serialized.Append(Constants.OPEN_CURLY);
                    AppendType(value, SerializationTypeHandle.Collections, ",\"$values\":[");
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
            if(_serializerSettings.PropertyStringEscape)
            {
                _serialized.AppendFormat("\\\"{0}\\\"", value);
            }
            else
            {
                _serialized.AppendFormat("\"{0}\"", value);
            }
        }

        private void AppendType(object obj, SerializationTypeHandle serializationTypeHandle, string appendString = "")
        {
            if(_serializerSettings.SerializationTypeHandle == SerializationTypeHandle.All || _serializerSettings.SerializationTypeHandle == serializationTypeHandle)
            {
                AppendAsString("$type");
                _serialized.Append(Constants.COLON);
                AppendAsString(obj.GetType().GetTypeAsString());
                _serialized.Append(appendString);
            }
        }
    }
}
