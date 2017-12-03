using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using SFJson.Attributes;
using SFJson.Utils;

namespace SFJson.Tokenization.Tokens
{
    /// <summary>
    /// Represents an object in tokenized form to be deserialized.
    /// </summary>
    /// <seealso cref="JsonToken"/>
    /// <seealso cref="JsonDictionary"/>
    /// <seealso cref="JsonObject"/>
    /// <seealso cref="JsonValue"/>
    public class JsonObject : JsonToken
    {
        private MemberInfo[] _memberInfos;

        /// <summary>
        /// Returns the token type, a collection will always be <c>JsonTokenType.Object</c>
        /// </summary>
        public override JsonTokenType JsonTokenType
        {
            get { return JsonTokenType.Object; }
        }

        /// <inheritdoc cref="JsonToken.GetValue"/>
        public override object GetValue(Type type)
        {
            type = DetermineType(type);
            var obj = CreateInstance(type);
            if(type.Implements(typeof(IDictionary)))
            {
                return GetDictionaryValues(type, obj as IDictionary);
            }
            else if(type.IsStack())
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
            else if(type.IsQueue())
            {
                var list = CreateInstance(Type.GetType($"System.Collections.Generic.List`1[[{type.GetGenericArguments()[0].AssemblyQualifiedName}]]")) as IList;
                GetListValues(type, list);
                obj = CreateInstance(type, list);
            }
            else if(type.Implements(typeof(IList)))
            {
                return GetListValues(type, obj as IList);
            }

            _memberInfos = type.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            foreach(var child in Children)
            {
                if(child.Name != "$type")
                {
                    SetValue(child, obj);
                }
            }

            return obj;
        }

        private void SetValue(JsonToken child, object obj)
        {
            var memberInfo = FindMemberInfoOfToken(child);
            if(memberInfo is PropertyInfo)
            {
                var propertyInfo = (PropertyInfo)memberInfo;
                propertyInfo.SetValue(obj, child.GetValue(propertyInfo.PropertyType), null);
            }
            else if(memberInfo is FieldInfo)
            {
                var fieldInfo = (FieldInfo)memberInfo;
                fieldInfo.SetValue(obj, child.GetValue(fieldInfo.FieldType));
            }
        }

        private MemberInfo FindMemberInfoOfToken(JsonToken child)
        {
            var memberInfo = _memberInfos.FirstOrDefault(m => m.GetCustomAttributes(true).Any(a => a is JsonNamedValue && ((JsonNamedValue)a).Name == child.Name) || m.Name == child.Name);
            if(memberInfo == null)
            {
                return null;
            }

            var ignoreAttribute = (JsonIgnore)memberInfo.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonIgnore));
            return (ignoreAttribute == null) ? memberInfo : null;
        }
    }
}
