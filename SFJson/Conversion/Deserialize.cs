using System;

namespace SFJson
{
    public class Deserializer
    {
        private string _stringToDeserialize;

        public JsonToken LastTokenization { get; private set; }

        public T Deserialize<T>(string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            _stringToDeserialize = stringToSerialize;
            LastTokenization = new Tokenizer().Tokenize(_stringToDeserialize, deserializerSettings ?? new DeserializerSettings());
            var obj = LastTokenization.GetValue<T>();
            return obj;
        }

        public object Deserialize(Type type, string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            _stringToDeserialize = stringToSerialize;
            LastTokenization = new Tokenizer().Tokenize(_stringToDeserialize, deserializerSettings ?? new DeserializerSettings());
            var obj = LastTokenization.GetValue(type);
            return obj;
        }
    }
}
