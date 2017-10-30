using System;
using System.Collections.Generic;

namespace SFJson.Conversion.Settings
{
    public class TypeBindings
    {
        private static Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();

        public void Add<TFrom, TTo>()
        {
            if(!_bindings.ContainsKey(typeof(TFrom)))
            {
                _bindings.Add(typeof(TFrom), typeof(TTo));
            }
        }

        public Type TryGetValue<T>()
        {
            return TryGetValue(typeof(T));
        }

        public Type TryGetValue(Type type)
        {
            Type val;
            _bindings.TryGetValue(type, out val);
            return val;
        }

        public bool Remove<T>()
        {
            return _bindings.Remove(typeof(T));
        }
    }
}
