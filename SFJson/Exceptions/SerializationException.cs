using System;

namespace SFJson.Exceptions
{
    /// <summary>
    /// Exception thrown during serialization process. This is a fatal
    /// error and will not continue the serialization process. Details of
    /// the exception can be obtained from the <c>InnerException</c>.
    /// </summary>
    public class SerializationException : Exception
    {
        public SerializationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
