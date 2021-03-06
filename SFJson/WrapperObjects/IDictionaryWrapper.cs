using System;
using System.Collections;

namespace SFJson.WrapperObjects
{
    public interface IDictionaryWrapper : IDictionary
    {
        object Dictionary { get; }
        Type KeyType { get; }
        Type ValueType { get; }
    }
}
