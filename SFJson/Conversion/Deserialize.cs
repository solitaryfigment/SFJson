using System;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Tokenization;
using SFJson.Tokenization.Tokens;

namespace SFJson.Conversion
{
    public class Deserializer
    {
        private string _stringToDeserialize;
        private JsonToken _lastTokenization;

        public T Deserialize<T>(string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            _stringToDeserialize = stringToSerialize;
            _lastTokenization = new Tokenizer().Tokenize(_stringToDeserialize, deserializerSettings);
            try
            {
                var obj = _lastTokenization.GetValue<T>();
                return obj;
            }
            catch(DeserializationException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new DeserializationException("An error occured during deserialization.", _lastTokenization, e);
            }
        }

        public object Deserialize(Type type, string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            _stringToDeserialize = stringToSerialize;
            _lastTokenization = new Tokenizer().Tokenize(_stringToDeserialize, deserializerSettings);
            try
            {
                var obj = _lastTokenization.GetValue(type);
                return obj;
            }
            catch(DeserializationException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new DeserializationException("An error occured during deserialization.", _lastTokenization, e);
            }
        }
    }
}
