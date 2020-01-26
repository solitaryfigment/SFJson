using System;
using SFJson.Conversion.Settings;

namespace SFJson.Conversion
{
    /// <summary>
    /// Static wrapper containing methods for serialization and deserialization.
    /// </summary>
    public static class Converter
    {
        private static readonly Serializer _serializer;
        private static readonly Deserializer _deserializer;
        
        static Converter()
        {
            _serializer = new Serializer();
            _deserializer = new Deserializer();
        }
        
        /// <see cref="Serializer.Serialize"/>
        public static string Serialize(object objectToSerialize, SerializerSettings serializerSettings = null)
        {
            return _serializer.Serialize(objectToSerialize, serializerSettings);
        }
        
        /// <see cref="Deserializer.Deserialize"/>
        public static T Deserialize<T>(string stringToDeserialize, DeserializerSettings deserializerSettings = null)
        {
            return _deserializer.Deserialize<T>(stringToDeserialize, deserializerSettings);
        }
        
        /// <see cref="Deserializer.Deserialize"/>
        public static object Deserialize(Type type, string stringToDeserialize, DeserializerSettings deserializerSettings = null)
        {
            return _deserializer.Deserialize(type, stringToDeserialize, deserializerSettings);
        }
    }
}