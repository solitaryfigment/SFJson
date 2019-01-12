namespace SFJson.Conversion.Settings
{
    /// <summary>
    /// Static wrapper and defaults for <see cref="DeserializerSettings"/> and <see cref="SerializerSettings"/>
    /// If the passed in settings are null the <see cref="GlobalSettings"/> will be used.
    /// </summary>
    public static class GlobalSettings
    {
        public static DeserializerSettings DeserializationSettings { get; }
        public static SerializerSettings SerializationSettings { get; }

        static GlobalSettings()
        {
            DeserializationSettings = new DeserializerSettings();
            SerializationSettings = new SerializerSettings();
        }
    }
}
