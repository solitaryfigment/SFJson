using System;
using SFJson.Utils;

namespace SFJson.Conversion
{
    public class SerializerSettings
    {
        private const string DEFAULT_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss.fff";
        private const string DEFAULT_TIME_OFFSET_FORMAT = "yyyy-MM-ddTHH:mm:ss.fff zzz";
        private string _dateTimeFormat = String.Empty;
        private string _dateTimeOffsetFormat = String.Empty;
        
        public TypeHandler TypeHandler { get; set; }

        public string DateTimeFormat
        {
            get
            {
                if(string.IsNullOrEmpty(_dateTimeFormat))
                {
                    return DEFAULT_TIME_FORMAT;
                }
                return _dateTimeFormat;
            }
            set
            {
                _dateTimeFormat = value;
            }
            
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
            set
            {
                _dateTimeOffsetFormat = value;
            }
        }

        internal bool PropertyStringEscape { get; set; }
    }

    public class DeserializerSettings
    {
        public bool SkipNullKeysInDictionary { get; set; }
    }
}
