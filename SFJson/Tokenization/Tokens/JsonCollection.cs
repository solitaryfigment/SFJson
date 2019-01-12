using System;
using System.Collections;
using System.Linq;
using SFJson.Utils;

namespace SFJson.Tokenization.Tokens
{
    /// <summary>
    /// Represents a collection (i.e. Array or List) in tokenized form
    /// to be deserialized.
    /// </summary>
    /// <seealso cref="JsonToken"/>
    /// <seealso cref="JsonDictionary"/>
    /// <seealso cref="JsonObject"/>
    /// <seealso cref="JsonValue"/>
    public class JsonCollection : JsonObject
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
                return GetDictionaryValues(type, obj as IDictionary);
            }
            if(type.Implements(typeof(IList)))
            {
                return GetListValues(type, obj as IList);
            }
            if(type.IsStack())
            {
                return GetStackValue(type, true);
            }
            if(type.IsQueue())
            {
                return GetStackValue(type, false);
            }
            
            return obj;
        }
    }
}
