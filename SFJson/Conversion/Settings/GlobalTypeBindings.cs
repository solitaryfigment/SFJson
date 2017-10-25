using System;

namespace SFJson.Conversion.Settings
{
    public static class GlobalTypeBindings
    {
        private static TypeBindings _bindings = new TypeBindings();
        
        public static void Add<TFrom, TTo>()
        {
            _bindings.Add<TFrom, TTo>();
        }

        public static Type TryGetValue<T>()
        {
            return _bindings.TryGetValue<T>();
        }

        public static Type TryGetValue(Type type)
        {
            return _bindings.TryGetValue(type);
        }

        public static bool Remove<T>()
        {
            return _bindings.Remove<T>();
        }
    }
}