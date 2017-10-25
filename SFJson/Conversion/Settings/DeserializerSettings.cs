namespace SFJson.Conversion.Settings
{
    public class DeserializerSettings
    {
        public bool SkipNullKeysInDictionary { get; set; }
        public TypeBindings TypeBindings { get; }

        public DeserializerSettings()
        {
            TypeBindings = new TypeBindings();
        }
    }
}