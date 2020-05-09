using System;

namespace SFJson.Attributes
{
    /// <summary>
    /// Maps the provided key name to the field or property during deserialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class JsonNamedValue : Attribute
    {
        public string Name { get; }

        public JsonNamedValue(string name)
        {
            Name = name;
        }
    }
}
