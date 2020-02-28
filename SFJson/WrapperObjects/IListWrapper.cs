using System;
using System.Collections;

namespace SFJson.Conversion
{
    public interface IListWrapper : IList
    {
        object List { get; }
        Type ElementType { get; }
    }
}
    
