using System;

namespace SFJson.Attributes
{
    public abstract class SerializeStep : Attribute
    {
                
    }
    
    /// <summary>
    /// Marks the field or property to be ignored during serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PostDeserialization : SerializeStep
    {
    }
}
