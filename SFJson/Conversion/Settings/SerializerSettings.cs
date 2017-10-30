using SFJson.Utils;

namespace SFJson.Conversion.Settings
{
    public class SerializerSettings
    {
        private const string DEFAULT_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss.fff";
        private const string DEFAULT_TIME_OFFSET_FORMAT = "yyyy-MM-ddTHH:mm:ss.fff zzz";
        private string _dateTimeFormat = string.Empty;
        private string _dateTimeOffsetFormat = string.Empty;

        public SerializationType SerializationType { get; set; }

        public string DateTimeFormat
        {
            get
            {
                return string.IsNullOrEmpty(_dateTimeFormat) ? DEFAULT_TIME_FORMAT : _dateTimeFormat;
            }
            set { _dateTimeFormat = value; }
        }

        public string DateTimeOffsetFormat
        {
            get
            {
                if(string.IsNullOrEmpty(_dateTimeOffsetFormat))
                {
                    return DEFAULT_TIME_OFFSET_FORMAT;
                }

                return _dateTimeOffsetFormat;
            }
            set { _dateTimeOffsetFormat = value; }
        }

        internal bool PropertyStringEscape { get; set; }
    }
}
