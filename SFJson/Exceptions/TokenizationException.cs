using System;

namespace SFJson.Exceptions
{
    /// <summary>
    /// Exception thrown during tokenization process. This is a fatal
    /// error and will not continue the deserialization process.
    /// </summary>
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
