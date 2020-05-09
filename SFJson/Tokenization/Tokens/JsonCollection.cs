using System;
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

        public override object GetValue(Type type, object instance = null)
        {
            type = DetermineType(type);
            var obj = CreateInstance(type, instance);
            
            if(IsGenericDictionary(obj, type, out var dictionaryWrapper))
            {
                return GetDictionaryValues(dictionaryWrapper);
            }
            if(IsGenericList(obj, type, out var listWrapper))
            {
                return GetListValues(type, listWrapper);
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
