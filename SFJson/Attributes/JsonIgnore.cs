using System;

namespace SFJson.Attributes
{
    /// <summary>
    /// Marks the field or property to be ignored during serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class JsonIgnore : Attribute
    {
    }
}
