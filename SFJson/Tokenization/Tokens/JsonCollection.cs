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
            else if(type.GetGenericArguments().Length == 1 && type.IsAssignableFrom(Type.GetType($"System.Collections.Generic.Stack`1[[{type.GetGenericArguments()[0].AssemblyQualifiedName}]], System")))
            {
                var list = CreateInstance(Type.GetType($"System.Collections.Generic.List`1[[{type.GetGenericArguments()[0].AssemblyQualifiedName}]]")) as IList;
                GetListValues(type, list);
                for(var i = 0; i < list.Count; i++)
                {
                    var element = list[i];
                    list.RemoveAt(i);
                    list.Insert(0,element);
                }
                obj = CreateInstance(type, list);
            }
            else if(type.GetGenericArguments().Length == 1 && type.IsAssignableFrom(Type.GetType($"System.Collections.Generic.Queue`1[[{type.GetGenericArguments()[0].AssemblyQualifiedName}]], System")))
            {
                var list = CreateInstance(Type.GetType($"System.Collections.Generic.List`1[[{type.GetGenericArguments()[0].AssemblyQualifiedName}]]")) as IList;
                GetListValues(type, list);
                obj = CreateInstance(type, list);
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
