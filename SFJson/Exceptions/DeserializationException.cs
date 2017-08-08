using System;
using SFJson.Tokenization.Tokens;

namespace SFJson.Exceptions
{
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