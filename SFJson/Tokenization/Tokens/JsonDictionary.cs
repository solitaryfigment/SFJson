using System;
using System.Collections;
using System.Collections.Generic;
using SFJson.Utils;

namespace SFJson.Tokenization.Tokens
{
    /// <summary>
    /// Represents a collection (i.e. Array or List) in tokenized form
    /// to be deserialized.
    /// </summary>
    /// <seealso cref="JsonToken"/>
    /// <seealso cref="JsonObject"/>
    /// <seealso cref="JsonValue"/>
    public class JsonDictionary : JsonToken
    {
        public Dictionary<JsonToken, JsonToken> Entries = new Dictionary<JsonToken, JsonToken>(); 
        /// <summary>
        /// Returns the token type, a collection will always be <c>JsonTokenType.Collection</c>
        /// </summary>
        public override JsonTokenType JsonTokenType
        {
            get { return JsonTokenType.Dicitonary; }
        }

        public override object GetValue(Type type)
        {
            Console.WriteLine("Dictionary: " + type.Name + " -- " + Children.Count);
            type = DetermineType(type);
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];
            var obj = CreateInstance(type) as IDictionary;
            Console.WriteLine("Inside the dictionary.");
            foreach(var kvp in Entries)
            {
                Console.WriteLine("Key: " + kvp.Key.JsonTokenType + " : " + kvp.Key.Name);
                Console.WriteLine("Key: " + kvp.Value.JsonTokenType + " : " + kvp.Value.Name);
                obj.Add(kvp.Key.GetValue(keyType), kvp.Value.GetValue(valueType));
            }
            return obj;
        }
    }
}
