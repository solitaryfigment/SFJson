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
            object value;
            if(!TryParseValue(type, out value))
            {
                value = Convert.ChangeType(_value, type);
            }
            return value;
        }

        private bool TryParseValue(Type type, out object value)
        {
            bool didParse = true;
            value = null;
            if(type.IsEnum)
            {
                Console.WriteLine("Enum");
                var v = Enum.Parse(type, _value.ToString());
                Console.WriteLine("Done");
                value = v;
            }
            else if(type == typeof(DateTimeOffset))
            {
                Console.WriteLine("DateTimeOffset");
                var v = DateTimeOffset.Parse((string)_value);
                Console.WriteLine("Done");
                value = v;
            }
            else if(type == typeof(TimeSpan))
            {
                Console.WriteLine("TimeSpan");
                var v = TimeSpan.Parse((string)_value);
                Console.WriteLine("Done");
                value = v;
            }
            else
            {
                didParse = false;
            }
            return didParse;
        }
    }
}
