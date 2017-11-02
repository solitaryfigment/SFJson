using System;
using System.Collections.Generic;

namespace SFJson.Conversion.Settings
{
    /// <summary>
    /// Stores a <c>KeyValuePair</c> for binding types during deserialization. 
    /// The <c>Value</c> will be used in place of <c>Key</c>. This is
    /// useful when dealing with interface and abstract types.
    /// </summary>
    public class TypeBindings
    {
        private static readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();

        /// <summary>
        /// Adds a <c>KeyValuePair</c> binding.
        /// Where <c>TFrom</c> is the key and <c>TTo</c> is the value.
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        public void Add<TFrom, TTo>()
        {
            if(!_bindings.ContainsKey(typeof(TFrom)))
            {
                _bindings.Add(typeof(TFrom), typeof(TTo));
            }
        }

        /// <summary>
        /// Retrieves the type bound for type <c>T</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// The value bound for type <c>T</c>, which may be null. 
        /// </returns>
        public Type TryGetValue<T>()
        {
            return TryGetValue(typeof(T));
        }
        
        /// <summary>
        /// Retrieves the type bound for type <c>type</c>
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// The value bound for type <c>T</c>, which may be null. 
        /// </returns>
        public Type TryGetValue(Type type)
        {
            Type val;
            _bindings.TryGetValue(type, out val);
            return val;
        }

        /// <summary>
        /// Remove the value bound for type <c>T</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// <c>true</c> if the value bound for type <c>T</c> was successfully removed.
        /// </returns>
        public bool Remove<T>()
        {
            return _bindings.Remove(typeof(T));
        }
    }
}
