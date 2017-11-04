using SFJson.Utils;

namespace SFJson.Conversion.Settings
{
    /// <summary>
    /// Defines serialization settings used during the serialization process.
    /// </summary>
    public class SerializerSettings
    {
        private const string DEFAULT_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss.fff";
        private const string DEFAULT_TIME_OFFSET_FORMAT = "yyyy-MM-ddTHH:mm:ss.fff zzz";
        private string _dateTimeFormat = string.Empty;
        private string _dateTimeOffsetFormat = string.Empty;
        
        internal bool PropertyStringEscape { get; set; }

        /// <summary>
        /// Defines when to inject the type handle metadata in the json output.
        /// This is represented with an element <c>"$type"</c>.
        /// </summary>
        public SerializationTypeHandle SerializationTypeHandle { get; set; }

        /// <summary>
        /// Defines the format override for <c>DateTime</c>.
        /// The default is <c>"yyyy-MM-ddTHH:mm:ss.fff"</c>.
        /// </summary>
        public string DateTimeFormat
        {
            get
            {
                return string.IsNullOrEmpty(_dateTimeFormat) ? DEFAULT_TIME_FORMAT : _dateTimeFormat;
            }
            set { _dateTimeFormat = value; }
        }
        
        /// <summary>
        /// Defines the format override for <c>DateTimeOffset</c>.
        /// The default is <c>"yyyy-MM-ddTHH:mm:ss.fff zzz"</c>.
        /// </summary>
        public string DateTimeOffsetFormat
        {
            get
            {
                return string.IsNullOrEmpty(_dateTimeOffsetFormat) ? DEFAULT_TIME_OFFSET_FORMAT : _dateTimeOffsetFormat;
            }
            set { _dateTimeOffsetFormat = value; }
        }
    }
}
