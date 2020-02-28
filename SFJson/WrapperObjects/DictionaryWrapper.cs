using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SFJson.Conversion
{
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
            get
            {
                if(_dictionary != null)
                {
                    return _dictionary.Keys;
                }

                return _genericDictionary.Values.ToList();
            }
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
}
