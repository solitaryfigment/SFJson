using System;

namespace SFJson.Exceptions
{
    public class TokenizationException : Exception
    {
        public TokenizationException(string message)
            : this(message, null)
        {
        }

        public TokenizationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}