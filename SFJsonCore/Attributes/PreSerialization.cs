using System;
using System.Runtime.CompilerServices;

namespace SFJson.Attributes
{
    /// <summary>
    /// Marks the field or property to be ignored during serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PreSerialization : SerializeStep
    {
    }
}
