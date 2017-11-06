using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFJson.Utils;

namespace SFJson.Tokenization.Tokens
{
    /// <summary>
    /// Represents a Dictionary in tokenized form to be deserialized.
    /// </summary>
    /// <seealso cref="JsonToken"/>
    /// <seealso cref="JsonCollection"/>
    /// <seealso cref="JsonObject"/>
    /// <seealso cref="JsonValue"/>
    public class JsonDictionary : JsonToken
    {
        public readonly Dictionary<JsonToken, JsonToken> Entries = new Dictionary<JsonToken, JsonToken>();
        
        /// <summary>
        /// Returns the token type, dictionaries with reference type keys will always be <c>JsonTokenType.Dictionary</c>
        /// dictionaries with primitive type (i.e. int, float, string, etc) key make use of <see cref="JsonObject"/>
        /// </summary>
        public override JsonTokenType JsonTokenType
        {
            get { return JsonTokenType.Dictionary; }
        }

        public override object GetValue(Type type)
        {
            type = DetermineType(type);
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];
            var obj = CreateInstance(type) as IDictionary;

            if(obj == null)
            {
                throw new Exception("Object does not inherit from IDictionary.");
            }
            
            foreach(var kvp in Entries)
            {
                obj.Add(kvp.Key.GetValue(keyType), kvp.Value.GetValue(valueType));
            }
            
            return obj;
        }

        internal override void InternalPrettyPrint(int indentLevel, StringBuilder stringBuilder, bool f = false)
        {
            stringBuilder.Append('\n');
            PrettyPrintIndent(indentLevel, stringBuilder);
            
            PrettyPrintControl(false, stringBuilder);
            for(var index = 0; index < Entries.Count; index++)
            {
                var key = Entries.Keys.ToArray()[index];
                var value = Entries.Values.ToArray()[index];
                if(index > 0)
                {
                    stringBuilder.Append(Constants.COMMA);
                }
                key.InternalPrettyPrint(indentLevel + 1, stringBuilder);
                stringBuilder.Append(" : ");
                value.InternalPrettyPrint(indentLevel + 1, stringBuilder);
            }

            stringBuilder.Append('\n');
            PrettyPrintIndent(indentLevel, stringBuilder);
            PrettyPrintControl(true, stringBuilder);
            
        }
    }
}
