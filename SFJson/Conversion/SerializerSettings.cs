namespace SFJson
{
    public class SerializerSettings
    {
        public TypeHandler TypeHandler { get; set; }
        
        internal bool PropertyStringEscape { get; set; }
    }

    public class DeserializerSettings
    {
        public bool SkipNullKeysInDictionary { get; set; }
    }
}
