﻿using System;
using SFJson.Utils;

namespace SFJson.Tokenization.Tokens
{
    public class JsonValue : JsonToken
    {
        private readonly object _value;

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
            if(_value == null && OnNullValue != null)
            {
                value = OnNullValue(type);
            }
            else if(_value == null && type.IsValueType)
            {
                value = type.GetDefault();
            }
            else if(!TryParseValue(type, out value))
            {
                value = Convert.ChangeType(_value, type);
            }
            return value;
        }

        private bool TryParseValue(Type type, out object value)
        {
            var didParse = true;
            value = null;
            if(_value == null)
            {
                didParse = false;
            }
            else if(type.IsEnum)
            {
                value = Enum.Parse(type, _value.ToString());
            }
            else if(type == typeof(DateTimeOffset))
            {
                value = DateTimeOffset.Parse((string)_value);
            }
            else if(type == typeof(TimeSpan))
            {
                value = TimeSpan.Parse((string)_value);
            }
            else if(type == typeof(Type))
            {
                value = Type.GetType(_value.ToString());
            }
            else if(type == typeof(Guid))
            {
                value = new Guid((string)_value);
            }
            else
            {
                didParse = false;
            }
            return didParse;
        }
    }
}
