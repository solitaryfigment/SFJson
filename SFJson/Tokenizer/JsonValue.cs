using System;

namespace SFJson
{
    public class JsonValue : JsonToken
    {
        private object _value;
        
        public override JsonType JsonType
        {
            get { return JsonType.Value; }
        }

        public JsonValue(string name, object value)
        {
            Name = name;
            _value = value;
        }

        public override object GetValue(Type type)
        {
            if(type.IsEnum)
            {
                return Enum.Parse(type, _value.ToString());
            }
            
            if(type == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse((string)_value);
            }
            
            if(type == typeof(TimeSpan))
            {
                return TimeSpan.Parse((string)_value);
            }
            
            return Convert.ChangeType(_value, type);
        }
    }
}
