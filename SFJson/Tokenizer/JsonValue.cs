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

        public override T GetValue<T>()
        {
            return (T)_value;
        }

        public override object GetValue(Type type)
        {
            if(type.IsEnum)
            {
                return Enum.Parse(type, _value.ToString());
            }
            
            return Convert.ChangeType(_value, type);
        }
    }
}