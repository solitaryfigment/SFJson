using System;
using SFJson.Tokenization.Tokens;

namespace SFJson.Exceptions
{
    /// <summary>
    /// Exception thrown during deserialization process. This is a fatal
    /// error and will not continue the deserialization process. However,
    /// the property <c>Token</c> will contain the json object in tokenized form. 
    /// </summary>
    public class DeserializationException : Exception
    {
        public JsonToken Token { get; set; }

        public DeserializationException(string message, JsonToken token)
            : this(message, token, null)
        {
        }

        public DeserializationException(string message, JsonToken token, Exception innerException)
            : base(message, innerException)
        {
            Token = token;
        }
    }
}
