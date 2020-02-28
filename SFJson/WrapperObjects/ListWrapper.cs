using System;
using System.Collections;
using System.Collections.Generic;

namespace SFJson.Conversion
{
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
}
