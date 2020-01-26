
using System;
using SFJson.Utils;

namespace SFJson.Conversion.Settings
{
    internal class SettingsManager
    {
        internal DeserializerSettings DeserializationSettings { get; set; }
        internal SerializerSettings SerializationSettings { get; set; }
        
        #region Serializer Settings

        internal bool FormattedString
        {
            get { return (SerializationSettings != null) ? SerializationSettings.FormattedString : false; }
        }
        
        /// <summary>
        /// <see cref="Utils.SerializationTypeHandle"/>
        /// </summary>
        internal SerializationTypeHandle SerializationTypeHandle
        {
            get
            {
                if(SerializationSettings != null)
                {
                    return SerializationSettings.SerializationTypeHandle;
                }
                
                return GlobalSettings.SerializationSettings.SerializationTypeHandle;
            }
        }

        /// <summary>
        /// <see cref="SerializerSettings.DateTimeFormat"/>
        /// </summary>
        internal string DateTimeFormat
        {
            get
            {
                if(SerializationSettings != null)
                {
                    return SerializationSettings.DateTimeFormat;
                }
                
                return GlobalSettings.SerializationSettings.DateTimeFormat;
            }
        }

        /// <summary>
        /// <see cref="SerializerSettings.DateTimeOffsetFormat"/>
        /// </summary>
        internal string DateTimeOffsetFormat
        {
            get
            {
                if(SerializationSettings != null)
                {
                    return SerializationSettings.DateTimeOffsetFormat;
                }
                
                return GlobalSettings.SerializationSettings.DateTimeOffsetFormat;
            }
        }

        #endregion
        
        #region Deserialization Settings

        /// <summary>
        /// <see cref="DeserializerSettings.SkipNullKeysInKeyValuedCollections"/>
        /// </summary>
        internal bool SkipNullKeysInKeyValuedCollections
        {
            get
            {
                if(DeserializationSettings != null)
                {
                    return DeserializationSettings.SkipNullKeysInKeyValuedCollections;
                }
                
                return GlobalSettings.DeserializationSettings.SkipNullKeysInKeyValuedCollections;
            }
        }

        #region TypeBindings

        /// <summary>
        /// <see cref="TypeBindings.TryGetValue{T}"/>
        /// </summary>
        internal Type TryGetTypeBinding<T>()
        {
            if(DeserializationSettings != null)
            {
                return DeserializationSettings.TypeBindings.TryGetValue<T>();
            }
            return GlobalSettings.DeserializationSettings.TypeBindings.TryGetValue<T>();
        }

        /// <summary> 
        /// <see cref="TypeBindings.TryGetValue"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal Type TryGetTypeBinding(Type type)
        {
            Type returnType = null;
            if(DeserializationSettings != null)
            {
                returnType = DeserializationSettings.TypeBindings.TryGetValue(type);
            }

            if(returnType == null)
            {
                returnType = GlobalSettings.DeserializationSettings.TypeBindings.TryGetValue(type);
            }

            return returnType;
        }      

        #endregion
        
        #endregion
    }
}
