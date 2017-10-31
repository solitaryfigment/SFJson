namespace SFJson.Conversion.Settings
{
    /// <summary>
    /// Defines deserialization settings used during both tokenization
    /// and deserialization phases.
    /// </summary>
    public class DeserializerSettings
    {
        /// <summary>
        /// If <c>True</c> null keys will be skipped without error and/or warning.
        /// If <c>False</c> null keys will throw an exception.
        /// </summary>
        public bool SkipNullKeysInKeyValuedCollections { get; set; }
        
        /// <summary>
        /// Defines overrides for types during deserialization, a type
        /// defined in <c>TypeBindings</c> is deserialized as the <c>Value Type</c> 
        /// instead of the <c>Declared Type</c>.
        /// This allows default types for abstract and interface types, when object
        /// is not serialize with type metadata.
        /// </summary>
        public TypeBindings TypeBindings { get; }

        public DeserializerSettings()
        {
            TypeBindings = new TypeBindings();
        }
    }
}
