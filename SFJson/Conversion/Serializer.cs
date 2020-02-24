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

namespace SFJson.Conversion
{
    public interface IListWrapper : IList
    {
        object List { get; }
        Type ElementType { get; }
    }

    public class ListWrapper<T> : IListWrapper
    {
        private IList _list;
        private IList<T> _genericList;

        public object List
        {
            get
            {
                if(_list != null)
                {
                    return _list;
                }

                return _genericList;
            }
        }
        public Type ElementType
        {
            get { return typeof(T); }
        }

        public ListWrapper(IList list, bool notGeneric)
        {
            _list = list;
        }

        public ListWrapper(IList<T> list)
        {
            _genericList = list;
        }
        
        public IEnumerator GetEnumerator()
        {
            if(_list != null)
            {
                return _list.GetEnumerator();
            }
            return _genericList.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            if(_list != null)
            {
                _list.CopyTo(array, index);
            }
            _genericList.CopyTo((T[])array, index);
        }

        public int Count
        {
            get
            {
                if(_list != null)
                {
                    return _list.Count;
                }
                return _genericList.Count;
            }
        }
        
        public bool IsSynchronized
        {
            get
            {
                if(_list != null)
                {
                    return _list.IsSynchronized;
                }
                return false;
            }
        }
        
        public object SyncRoot
        {
            get
            {
                if(_list != null)
                {
                    return _list.SyncRoot;
                }
                return false;
            }
        }
        public int Add(object value)
        {
            if(_list != null)
            {
                return _list.Add(value);
            }
            _genericList.Add((T)value);
            return _genericList.Count - 1;
        }

        public void Clear()
        {
            if(_list != null)
            {
                _list.Clear();
            }
            _genericList.Clear();
        }

        public bool Contains(object value)
        {
            if(_list != null)
            {
                return _list.Contains(value);
            }
            return _genericList.Contains((T)value);
        }

        public int IndexOf(object value)
        {
            if(_list != null)
            {
                return _list.IndexOf(value);
            }
            return _genericList.IndexOf((T)value);
        }

        public void Insert(int index, object value)
        {
            if(_list != null)
            {
                _list.Insert(index, value);
            }
            _genericList.Insert(index, (T)value);
        }

        public void Remove(object value)
        {
            if(_list != null)
            {
                _list.Remove(value);
            }
            _genericList.Remove((T)value);
        }

        public void RemoveAt(int index)
        {
            if(_list != null)
            {
                _list.RemoveAt(index);
            }
            _genericList.RemoveAt(index);
        }

        public bool IsFixedSize
        {
            get
            {
                if(_list != null)
                {
                    return _list.IsFixedSize;
                }

                return false;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                if(_list != null)
                {
                    return _list.IsReadOnly;
                }

                return _genericList.IsReadOnly;
            }
        }

        public object this[int index]
        {
            get
            { 
                if(_list != null)
                {
                    return _list[index];
                }

                return _genericList[index];
            }
            set
            { 
                if(_list != null)
                {
                    _list[index] = value;
                }

                _genericList[index] = (T)value;
            }
        }
    }

    public interface IDictionaryWrapper : IDictionary
    {
        object Dictionary { get; }
        Type KeyType { get; }
        Type ValueType { get; }
    }
    
    public class DictionaryWrapper<TKey, TValue> : IDictionaryWrapper
    {
        private readonly IDictionary _dictionary;
        private readonly IDictionary<TKey, TValue> _genericDictionary;

        public Type KeyType
        {
            get { return typeof(TKey); }
        }
        public Type ValueType
        {
            get { return typeof(TValue); }
        }
        
        public object Dictionary
        {
            get
            {
                if(_dictionary != null)
                {
                    return _dictionary;
                }

                return _genericDictionary;
            }
        }

        public DictionaryWrapper(IDictionary dictionary, bool notGeneric)
        {
            _dictionary = dictionary;
        }

        public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
        {
            _genericDictionary = dictionary;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object key)
        {
            throw new NotImplementedException();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get
            {
                if(_dictionary == null)
                {
                    return _genericDictionary.IsReadOnly;
                }
                else
                {
                    return _dictionary.IsReadOnly;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                if(_dictionary == null)
                {
                    return _genericDictionary.IsReadOnly;
                }
                else
                {
                    return _dictionary.IsReadOnly;
                }
            }
        }

        public object this[object key]
        {
            get
            {
                if(_dictionary != null)
                {
                    return _dictionary[key];
                }

                return _genericDictionary[(TKey)key];
            }
            set 
            { 
                if(_dictionary != null)
                {
                    _dictionary[key] = value;
                    return;
                }

                _genericDictionary[(TKey)key] = (TValue)value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public ICollection Keys
       {
           get 
           { 
               if(_dictionary != null)
               {
                   return _dictionary.Keys;
               }

               return _genericDictionary.Keys.ToList();
           }
       }

        public ICollection Values
        {
            get { 
                if(_dictionary != null)
                {
                    return _dictionary.Keys;
                }
                return _genericDictionary.Values.ToList(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                if(_dictionary == null)
                {
                    return _genericDictionary.Count;
                }
                else
                {
                    return _dictionary.Count;
                }
            }
        }
        public bool IsSynchronized
        {
            get
            {
                if(_dictionary == null)
                {
                    return false;
                }
                else
                {
                    return _dictionary.IsSynchronized;
                }
            }
        }
        
        public object SyncRoot
        {
            get
            {
                if(_dictionary == null)
                {
                    return false;
                }
                else
                {
                    return _dictionary.SyncRoot;
                }
            }
        }

        public object obj
        {
            get
            {
                if(_dictionary == null)
                {
                    return _genericDictionary;
                }
                else
                {
                    return _dictionary;
                }
            }
        }
    }
    
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
            var wrapperType = typeof(DictionaryWrapper<,>)?.MakeGenericType(keyType, valueType);
            ConstructorInfo genericWrapperConstructor = wrapperType.GetConstructor(new[] { dictType });
            IDictionaryWrapper wrapper = (IDictionaryWrapper)genericWrapperConstructor.Invoke(new object[]{dictionary});
            return wrapper;
        }
        
        internal static IListWrapper CreateListWrapper(object list)
        {
            IListWrapper wrapper = new ListWrapper<object>((IList)list, true);
            return wrapper;
        }
        
        internal static IListWrapper CreateListWrapper(object list, Type listType, Type elementType)
        {
            var wrapperType = typeof(ListWrapper<>)?.MakeGenericType(elementType);
            ConstructorInfo genericWrapperConstructor = wrapperType.GetConstructor(new[] { listType });
            IListWrapper wrapper = (IListWrapper)genericWrapperConstructor.Invoke(new object[]{list});
            return wrapper;
        }
        
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
            foreach(var interfaceType in type.GetInterfaces())
            {
                if(interfaceType.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                {
                    var genArgs = interfaceType.GenericTypeArguments;
                    dictionaryWrapper = CreateDictionaryWrapper(obj, interfaceType, genArgs[0], genArgs[1]);
                    return true;
                }
                else if (typeof(IDictionary).IsAssignableFrom(interfaceType))
                {
                    dictionaryWrapper = CreateDictionaryWrapper(obj);
                    return true;
                }
            }

            return false;
        }
    }
}
