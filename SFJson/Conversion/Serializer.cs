using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SFJson.Attributes;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Utils;
using SFJson.WrapperObjects;

namespace SFJson.Conversion
{
    /// <summary>
    /// Handles converting an <c>object</c> to a JSON string.
    /// </summary>
    public class Serializer
    {
        private SettingsManager _settingsManager;
        private StringBuilder _serialized;

        internal static IDictionaryWrapper CreateDictionaryWrapper(object dictionary)
        {
            IDictionaryWrapper wrapper = new DictionaryWrapper<object,object>((IDictionary)dictionary, true);
            return wrapper;
        }
        internal static IDictionaryWrapper CreateDictionaryWrapper(object dictionary, Type dictType, Type keyType, Type valueType)
        {
            var wrapperType = typeof(DictionaryWrapper<,>).MakeGenericType(keyType, valueType);
            ConstructorInfo genericWrapperConstructor = wrapperType.GetConstructor(new[] { dictType });
            IDictionaryWrapper wrapper = (IDictionaryWrapper)genericWrapperConstructor?.Invoke(new []{dictionary});
            return wrapper;
        }

        internal static IListWrapper CreateListWrapper(object list)
        {
            IListWrapper wrapper = new ListWrapper<object>((IList)list, true);
            return wrapper;
        }

        internal static IListWrapper CreateListWrapper(object list, Type listType, Type elementType)
        {
            var wrapperType = typeof(ListWrapper<>).MakeGenericType(elementType);
            ConstructorInfo genericWrapperConstructor = wrapperType.GetConstructor(new[] { listType });
            IListWrapper wrapper = (IListWrapper)genericWrapperConstructor?.Invoke(new []{list});
            return wrapper;
        }

        /// <summary>
        /// Converts an <c>object</c> to a JSON string.
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
                if(objectToSerialize != null)
                {
                    SerializeObject(objectToSerialize.GetType(), objectToSerialize);
                }
                else
                {
                    return null;
                }
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

        private void SerializeDictionary(IDictionaryWrapper dictionary, int indentLevel)
        {
            var appendSeparator = false;
            if(dictionary != null)
            {
                _serialized.Append(Constants.OPEN_CURLY);
                AppendType(dictionary.Dictionary, SerializationTypeHandle.Collections, ++indentLevel, Constants.COMMA.ToString());
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

        private void SerializeList(IListWrapper list, int indentLevel)
        {
            var appendSeparator = false;
            if(list != null)
            {
                if(_settingsManager.SerializationTypeHandle == SerializationTypeHandle.All || _settingsManager.SerializationTypeHandle == SerializationTypeHandle.Collections)
                {
                    _serialized.Append(Constants.OPEN_CURLY);
                    AppendType(list.List, SerializationTypeHandle.Collections, ++indentLevel, ",\"$values\":[");
                    var e = list.GetEnumerator();
                    while(e.MoveNext())
                    {
                        AppendSeparator(appendSeparator, indentLevel);
                        SerializeObject(e.Current?.GetType(), e.Current, indentLevel);
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
                        SerializeObject(e.Current?.GetType(), e.Current, indentLevel);
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
                        SerializeObject(e.Current?.GetType(), e.Current, indentLevel);
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
                        SerializeObject(e.Current?.GetType(), e.Current, indentLevel);
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
                if(!(propertyInfo.CanWrite && propertyInfo.CanRead) || propertyInfo.GetIndexParameters().Any())
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
            var attributes = memberInfo.GetCustomAttributes(true);
            var ignoreAttribute = (JsonIgnore) attributes.FirstOrDefault(a => a.GetType() == typeof(JsonIgnore));
            if(ignoreAttribute != null)
            {
                return false;
            }

            var attribute = (JsonNamedValue) attributes.FirstOrDefault(a => a.GetType() == typeof(JsonNamedValue));
            var customAttribute = (CustomConverter) attributes.FirstOrDefault(a => a.GetType() == typeof(CustomConverter));
            var memberName = attribute != null ? attribute.Name : memberInfo.Name;
            AppendSeparator(appendSeparator, indentLevel);
            AppendAsString(memberName);
            _serialized.Append(Constants.COLON);
            PrettyPrintSpace();
            if(customAttribute != null)
            {
                _serialized.Append(customAttribute.Serialize(value));
            }
            else
            {
                SerializeObject(type, value, indentLevel);
            }

            return true;
        }

        protected void PrettyPrintIndent(int indentLevel)
        {
            if(!_settingsManager.FormattedString)
            {
                return;
            }

            for(var i = 0; i < indentLevel; i++)
            {
                _serialized.Append('\t');
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
            if(value == null)
            {
                _serialized.AppendFormat(Constants.NULL);
                return;
            }

            type = type.IsInterface ? value.GetType() : type;

            if(type.IsEnum)
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
                var indexAndFormat = $"{{0:{_settingsManager.DateTimeOffsetFormat}}}";
                AppendAsString(string.Format(indexAndFormat, value));
            }
            else if(value is DateTime)
            {
                var indexAndFormat = $"{{0:{_settingsManager.DateTimeFormat}}}";
                AppendAsString(string.Format(indexAndFormat, value));
            }
            else if(value is TimeSpan)
            {
                AppendAsString(value.ToString());
            }
            else if(value is Type vType)
            {
                AppendAsString(vType.GetTypeAsString());
            }
            else if(type == typeof(string))
            {
                AppendAsString(((string) value).EscapeQuotes());
            }
            else if(type == typeof(Guid))
            {
                AppendAsString(value.ToString());
            }
            else if(IsGenericDictionary(value, type, out IDictionaryWrapper dictionaryWrapper))
            {
                SerializeDictionary(dictionaryWrapper, indentLevel);
            }
            else if(type.IsArray || type.IsStack() || type.IsQueue())
            {
                SerializeList((IEnumerable)value, indentLevel);
            }
            else if(IsGenericList(value, type, out IListWrapper listWrapper))
            {
                SerializeList(listWrapper, indentLevel);
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

        private bool IsGenericList(object obj, Type type, out IListWrapper listWrapper)
        {
            listWrapper = null;
            foreach(var interfaceType in type.GetInterfaces())
            {
                if(interfaceType.IsGenericType && typeof(IList<>).IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                {
                    var genArgs = interfaceType.GenericTypeArguments;
                    listWrapper = CreateListWrapper(obj, interfaceType, genArgs[0]);
                    return true;
                }
                else if (typeof(IList).IsAssignableFrom(interfaceType))
                {
                    listWrapper = CreateListWrapper(obj);
                    return true;
                }
            }

            return false;
        }

        private bool IsGenericDictionary(object obj, Type type, out IDictionaryWrapper dictionaryWrapper)
        {
            dictionaryWrapper = null;
            foreach(var interfaceType in obj.GetType().GetInterfaces())
            {
                if(interfaceType.IsGenericType && (interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                {
                    var genArgs = interfaceType.GenericTypeArguments;
                    dictionaryWrapper = CreateDictionaryWrapper(obj, type, genArgs[0], genArgs[1]);
                    return true;
                }
            }
            if(typeof(IDictionary).IsAssignableFrom(type))
            {
                dictionaryWrapper = CreateDictionaryWrapper(obj);
                return true;
            }

            return false;
        }
    }
}
