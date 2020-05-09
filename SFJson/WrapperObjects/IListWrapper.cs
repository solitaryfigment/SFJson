using System;
using System.Collections;

namespace SFJson.WrapperObjects
{
    public interface IListWrapper : IList
    {
        object List { get; }
        Type ElementType { get; }
    }
}
    
