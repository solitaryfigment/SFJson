using System;
using System.Collections;
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
    public class JsonCollection : JsonToken
    {
        /// <summary>
        /// Returns the token type, a collection will always be <c>JsonTokenType.Collection</c>
        /// </summary>
        public override JsonTokenType JsonTokenType
        {
            get { return JsonTokenType.Collection; }
        }

        public override object GetValue(Type type)
        {
            type = DetermineType(type);
            var obj = CreateInstance(type);
            if(type.Implements(typeof(IDictionary)))
            {
                obj = GetDictionaryValues(type, obj as IDictionary);
            }
            else if(type.Implements(typeof(IEnumerable)))
            {
                obj = GetListValues(type, obj as IList);
            }
            else
            {
                // throw error
            }
            return obj;
        }
    }
}
