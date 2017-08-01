using System;
using SFJson.Exceptions;

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
            try
            {
                var obj = LastTokenization.GetValue<T>();
                return obj;
            }
            catch (DeserializationException de)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new DeserializationException("An error occured during deserialization.", LastTokenization, e);
            }
        }

        public object Deserialize(Type type, string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            _stringToDeserialize = stringToSerialize;
            LastTokenization = new Tokenizer().Tokenize(_stringToDeserialize, deserializerSettings ?? new DeserializerSettings());
            try
            {
                var obj = LastTokenization.GetValue(type);
                return obj;
            }
            catch (DeserializationException de)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new DeserializationException("An error occured during deserialization.", LastTokenization, e);
            }
        }
    }
}
