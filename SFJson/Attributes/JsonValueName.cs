using System;

namespace SFJson.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class JsonValueName : Attribute
    {
        public string Name { get; set; }
        
        public JsonValueName(string name)
        {
            Name = name;
        }
    }
}