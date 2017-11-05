using System;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Tokenization;
using SFJson.Tokenization.Tokens;

namespace SFJson.Conversion
{
    /// <summary>
    /// Handles converting a JSON string to an intance of an object.
    /// </summary>
    public class Deserializer
    {
        private string _stringToDeserialize;
        private JsonToken _lastTokenization;

        /// <summary>
        /// Converts a JSON string to an instance of type <c>T</c>.
        /// </summary>
        /// <param name="stringToSerialize"></param>
        /// <param name="deserializerSettings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// A deserialized instance of type <c>T</c>
        /// </returns>
        /// <exception cref="DeserializationException">
        /// Deserializtion errors will contain the root <see cref="JsonToken"/> from
        /// the tokenization phase.
        /// </exception>
        public T Deserialize<T>(string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            _stringToDeserialize = stringToSerialize;
            _lastTokenization = new Tokenizer().Tokenize(_stringToDeserialize, new SettingsManager { DeserializationSettings = deserializerSettings });
            try
            {
                var obj = _lastTokenization.GetValue<T>();
                return obj;
            }
            catch(DeserializationException e)
            {
                throw new DeserializationException(e.Message, _lastTokenization, e.InnerException);
            }
            catch(Exception e)
            {
                throw new DeserializationException("An error occured during deserialization.", _lastTokenization, e);
            }
        }

        /// <summary>
        /// Converts a JSON string to an instance of <pararef name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stringToSerialize"></param>
        /// <param name="deserializerSettings"></param>
        /// <returns>
        /// A deserialized <c>object</c> instance and may be converted
        /// to <paramref name="type"/>.
        /// </returns>
        /// <exception cref="DeserializationException">
        /// Deserializtion errors will contain the root <see cref="JsonToken"/> from
        /// the tokenization phase.
        /// </exception>
        public object Deserialize(Type type, string stringToSerialize, DeserializerSettings deserializerSettings = null)
        {
            _stringToDeserialize = stringToSerialize;
            _lastTokenization = new Tokenizer().Tokenize(_stringToDeserialize, new SettingsManager { DeserializationSettings = deserializerSettings });
            try
            {
                var obj = _lastTokenization.GetValue(type);
                return obj;
            }
            catch(DeserializationException e)
            {
                throw new DeserializationException(e.Message, _lastTokenization, e.InnerException);
            }
            catch(Exception e)
            {
                throw new DeserializationException("An error occured during deserialization.", _lastTokenization, e);
            }
        }
    }
}
