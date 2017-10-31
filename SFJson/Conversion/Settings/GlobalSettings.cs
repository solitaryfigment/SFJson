using System;
using SFJson.Utils;

namespace SFJson.Conversion.Settings
{
    /// <summary>
    /// Static wrapper and defaults for <see cref="DeserializerSettings"/> and <see cref="SerializerSettings"/>
    /// If the passed in settings are null the <see cref="GlobalSettings"/> will be used.
    /// </summary>
    public static class GlobalSettings
    {
        private static readonly DeserializerSettings _deserializationSettings;
        private static readonly SerializerSettings _serializationSettings;

        static GlobalSettings()
        {
            _deserializationSettings = new DeserializerSettings();
            _serializationSettings = new SerializerSettings();
        }

        #region Serializer Settings

        /// <summary>
        /// <see cref="SerializerSettings.SerializationTypeHandle"/>
        /// </summary>
        public static SerializationTypeHandle SerializationTypeHandle
        {
            get { return _serializationSettings.SerializationTypeHandle; }
            set { _serializationSettings.SerializationTypeHandle = value; }
        }

        /// <summary>
        /// <see cref="SerializerSettings.DateTimeFormat"/>
        /// </summary>
        public static string DateTimeFormat
        {
            get { return _serializationSettings.DateTimeFormat; }
            set { _serializationSettings.DateTimeFormat = value; }
        }

        /// <summary>
        /// <see cref="SerializerSettings.DateTimeOffsetFormat"/>
        /// </summary>
        public static string DateTimeOffsetFormat
        {
            get { return _serializationSettings.DateTimeOffsetFormat; }
            set { _serializationSettings.DateTimeOffsetFormat = value; }
        }

        #endregion
        
        #region Deserialization Settings

        /// <summary>
        /// <see cref="DeserializerSettings.SkipNullKeysInKeyValuedCollections"/>
        /// </summary>
        public static bool SkipNullKeysInKeyValuedCollections
        {
            get { return _deserializationSettings.SkipNullKeysInKeyValuedCollections; }
            set { _deserializationSettings.SkipNullKeysInKeyValuedCollections = value; }
        }

        #region TypeBindings

        /// <summary>
        /// <see cref="TypeBindings.Add{TFrom, TTo}"/>
        /// </summary>
        public static void AddTypeBinding<TFrom, TTo>()
        {
            _deserializationSettings.TypeBindings.Add<TFrom, TTo>();
        }

        /// <summary>
        /// <see cref="TypeBindings.TryGetValue{T}"/>
        /// </summary>
        public static Type TryGetTypeBinding<T>()
        {
            return _deserializationSettings.TypeBindings.TryGetValue<T>();
        }

        /// <summary> 
        /// <see cref="TypeBindings.TryGetValue"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type TryGetTypeBinding(Type type)
        {
            return _deserializationSettings.TypeBindings.TryGetValue(type);
        }
        
        /// <summary>
        /// <see cref="TypeBindings.Remove{T}"/>
        /// </summary>
        public static bool RemoveTypeBinding<T>()
        {
            return _deserializationSettings.TypeBindings.Remove<T>();
        }        

        #endregion
        
        #endregion
    }
}
