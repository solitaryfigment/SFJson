using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using SFJson.Attributes;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Tokenization;
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

        private void SerializeObject(object obj, int indentLevel)
        {
            if(obj != null)
            {
                _serialized.Append(Constants.OPEN_CURLY);
                AppendType(obj, SerializationTypeHandle.Objects, ++indentLevel);
                SerializeMembers(obj, indentLevel);
                PrettyPrintNewLine();
                PrettyPrintIndent(--indentLevel);
                _serialized.Append(Constants.CLOSE_CURLY);
                return;
            }

            _serialized.Append(Constants.NULL);
        }

        private void SerializeDictionary(IDictionary dictionary, int indentLevel)
        {
            var appendSeparator = false;
            if(dictionary != null)
            {
                _serialized.Append(Constants.OPEN_CURLY);
                AppendType(dictionary, SerializationTypeHandle.Collections, ++indentLevel, Constants.COMMA.ToString());
                foreach(var key in dictionary.Keys)
                {
                    AppendSeparator(appendSeparator, indentLevel);
                    SerializeObject(key.GetType(), key, indentLevel);
                    _serialized.Append(Constants.COLON);
                    PrettyPrintSpace();
                    SerializeObject(dictionary[key].GetType(), dictionary[key], indentLevel);
                    appendSeparator = true;
                }
                PrettyPrintNewLine();
                PrettyPrintIndent(--indentLevel);
                _serialized.Append(Constants.CLOSE_CURLY);
                
                return;
            }

            _serialized.Append(Constants.NULL);
        }

        private void SerializeList(IEnumerable list, int indentLevel)
        {
            var appendSeparator = false;
            if(list != null)
            {
                if(_settingsManager.SerializationTypeHandle == SerializationTypeHandle.All || _settingsManager.SerializationTypeHandle == SerializationTypeHandle.Collections)
                {
                    _serialized.Append(Constants.OPEN_CURLY);
                    AppendType(list, SerializationTypeHandle.Collections, ++indentLevel, ",\"$values\":[");
                    var e = list.GetEnumerator();
                    while(e.MoveNext())
                    {
                        AppendSeparator(appendSeparator, indentLevel);
                        SerializeObject(e.Current.GetType(), e.Current, indentLevel);
                        appendSeparator = true;
                    }

                    PrettyPrintNewLine();
                    PrettyPrintIndent(--indentLevel);
                    _serialized.Append(Constants.CLOSE_BRACKET);
                    PrettyPrintNewLine();
                    PrettyPrintIndent(--indentLevel);
                    _serialized.Append(Constants.CLOSE_CURLY);
                }
                else
                {
                    _serialized.Append(Constants.OPEN_BRACKET);
                    indentLevel++;
                    var e = list.GetEnumerator();
                    while(e.MoveNext())
                    {
                        AppendSeparator(appendSeparator, indentLevel);
                        SerializeObject(e.Current.GetType(), e.Current, indentLevel);
                        appendSeparator = true;
                    }

                    PrettyPrintNewLine();
                    PrettyPrintIndent(--indentLevel);
                    _serialized.Append(Constants.CLOSE_BRACKET);
                }
                
                return;
            }

            _serialized.Append(Constants.NULL);
        }

        private void AppendSeparator(bool appendSeparator, int indentLevel)
        {
            if(appendSeparator)
            {
                _serialized.Append(Constants.COMMA);
            }
            PrettyPrintNewLine();
            PrettyPrintIndent(indentLevel);
        }

        private void SerializeMembers(object obj, int indentLevel)
        {
            var fieldInfos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            var propertyInfos = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var appendSeparator = _settingsManager.SerializationTypeHandle == SerializationTypeHandle.All || _settingsManager.SerializationTypeHandle == SerializationTypeHandle.Objects;
            foreach(var fieldInfo in fieldInfos)
            {
                if(SerializeMember(fieldInfo, fieldInfo.FieldType, fieldInfo.GetValue(obj), appendSeparator, indentLevel))
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
                    if(SerializeMember(propertyInfo, propertyInfo.PropertyType, propertyInfo.GetValue(obj, null), appendSeparator, indentLevel))
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

        private bool SerializeMember(MemberInfo memberInfo, Type type, object value, bool appendSeparator, int indentLevel)
        {
            var ignoreAttribute = (JsonIgnore) memberInfo.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonIgnore));
            if(ignoreAttribute == null)
            {
                var attribute = (JsonNamedValue) memberInfo.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonNamedValue));
                var memberName = (attribute != null) ? attribute.Name : memberInfo.Name;
                AppendSeparator(appendSeparator, indentLevel);
                AppendAsString(memberName);
                _serialized.Append(Constants.COLON);
                PrettyPrintSpace();
                SerializeObject(type, value, indentLevel);
                return true;
            }

            return false;
        }
        

        protected void PrettyPrintIndent(int indentLevel)
        {
            if(_settingsManager.FormattedString)
            {
                for(var i = 0; i < indentLevel; i++)
                {
                    _serialized.Append('\t');
                }
            }
        }
        
        protected void PrettyPrintNewLine()
        {
            if(_settingsManager.FormattedString)
            {
                _serialized.Append(Environment.NewLine);
            }
        }
        
        protected void PrettyPrintSpace()
        {
            if(_settingsManager.FormattedString)
            {
                _serialized.Append(Constants.SPACE);
            }
        }

        private void SerializeObject(Type type, object value, int indentLevel = 0)
        {
            type = type.IsInterface ? value.GetType() : type;
            if(value == null)
            {
                _serialized.AppendFormat(Constants.NULL);
            }
            else if(type.IsEnum)
            {
                AppendAsString(value.ToString());
            }
            else if(type.IsPrimitive || value is decimal)
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
                SerializeDictionary((IDictionary)value, indentLevel);
            }
            else if(type.IsArray || type.Implements(typeof(IEnumerable)))
            {
                SerializeList((IEnumerable)value, indentLevel);
            }
            else
            {
                SerializeObject(value, indentLevel);
            }
        }

        private void AppendAsString(string value)
        {
            _serialized.AppendFormat("\"{0}\"", value);
        }

        private void AppendType(object obj, SerializationTypeHandle serializationTypeHandle, int indentLevel, string appendString = "")
        {
            if(_settingsManager.SerializationTypeHandle == SerializationTypeHandle.All || _settingsManager.SerializationTypeHandle == serializationTypeHandle)
            {
                PrettyPrintNewLine();
                PrettyPrintIndent(indentLevel);
                AppendAsString("$type");
                _serialized.Append(Constants.COLON);
                PrettyPrintSpace();
                AppendAsString(obj.GetType().GetTypeAsString());
                _serialized.Append(appendString);
                PrettyPrintNewLine();
                PrettyPrintIndent(indentLevel);
            }
        }
    }
}
