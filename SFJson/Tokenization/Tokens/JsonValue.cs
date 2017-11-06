using System;
using System.Text;
using SFJson.Utils;

namespace SFJson.Tokenization.Tokens
{
    /// <summary>
    /// Represents an object in tokenized form to be deserialized.
    /// </summary>
    /// <seealso cref="JsonToken"/>
    /// <seealso cref="JsonCollection"/>
    /// <seealso cref="JsonDictionary"/>
    /// <seealso cref="JsonObject"/>
    public class JsonValue : JsonToken
    {
        private readonly object _value;
        private readonly bool _isQuoted;

        public override JsonTokenType JsonTokenType
        {
            get { return JsonTokenType.Value; }
        }

        public JsonValue(string name, object value, bool isQuoted)
        {
            Name = name;
            _value = value;
            _isQuoted = isQuoted;
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

        internal override void InternalPrettyPrint(int indentLevel, StringBuilder stringBuilder, bool f = false)
        {
            stringBuilder.Append('\n');
            PrettyPrintIndent(indentLevel, stringBuilder);
            if(!string.IsNullOrEmpty(Name))
            {
                stringBuilder.Append(Name + " : ");
            }
            if(_isQuoted)
            {
                stringBuilder.Append((_value != null) ? $"\"{_value.ToString()}\"" : "null");
            }
            else
            {
                stringBuilder.Append((_value != null) ? _value.ToString() : "null");
            }
        }
    }
}
