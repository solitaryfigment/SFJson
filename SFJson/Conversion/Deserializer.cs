using System;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Tokenization;
using SFJson.Tokenization.Tokens;

namespace SFJson.Conversion
{
    /// <summary>
    /// Handles converting a JSON string to an instance of an object.
    /// </summary>
    public class Deserializer
    {
        private string _stringToDeserialize;
        private JsonToken _lastTokenization;
        private readonly Tokenizer _tokenizer = new Tokenizer();

        public T Deserialize<T>(string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            return Deserialize<T>(null, stringToSerialize, deserializerSettings);
        }

        /// <summary>
        /// Converts a JSON string to an instance of type <c>T</c>.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="stringToSerialize"></param>
        /// <param name="deserializerSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// A deserialized instance of type <c>T</c>
        /// </returns>
        /// <exception cref="DeserializationException">
        /// Deserialization errors will contain the root <see cref="JsonToken"/> from
        /// the tokenization phase.
        /// </exception>
        public T Deserialize<T>(object instance, string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            Tokenize(stringToSerialize, deserializerSettings);
            try
            {
                var obj = _lastTokenization.GetValue<T>(instance);
                return obj;
            }
            catch(DeserializationException e)
            {
                e.Token = _lastTokenization;
                throw;
            }
            catch(Exception e)
            {
                throw new DeserializationException($"An error occured during deserialization. {_lastTokenization}.{_lastTokenization.Name}", _lastTokenization, e);
            }
        }

        public object Deserialize(Type type, string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            return this.Deserialize(type, null, stringToSerialize, deserializerSettings);
        }

        /// <summary>
        /// Converts a JSON string to an instance of <pararef name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <param name="stringToSerialize"></param>
        /// <param name="deserializerSettings"></param>
        /// <returns>
        /// A deserialized <c>object</c> instance and may be converted
        /// to <paramref name="type"/>.
        /// </returns>
        /// <exception cref="DeserializationException">
        /// Deserialization errors will contain the root <see cref="JsonToken"/> from
        /// the tokenization phase.
        /// </exception>
        public object Deserialize(Type type, object instance, string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            Tokenize(stringToSerialize, deserializerSettings);
            try
            {
                var obj = _lastTokenization.GetValue(type, instance);
                return obj;
            }
            catch(DeserializationException e)
            {
                e.Token = _lastTokenization;
                throw;
            }
            catch(Exception e)
            {
                throw new DeserializationException("An error occured during deserialization.", _lastTokenization, e);
            }
        }

        private void Tokenize(string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            _stringToDeserialize = stringToSerialize;
            _lastTokenization = _tokenizer.Tokenize(_stringToDeserialize, new SettingsManager { DeserializationSettings = deserializerSettings });
        }
    }
}
