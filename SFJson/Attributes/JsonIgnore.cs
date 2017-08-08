using System;

namespace SFJson.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class JsonIgnore : Attribute
    {
    }
}