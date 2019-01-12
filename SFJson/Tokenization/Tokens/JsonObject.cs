﻿using System;
using System.Collections;
using System.Collections.Generic;
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

            if(type == typeof(System.Object))
            {
                obj = new Dictionary<string, Object>();
                return GetDictionaryValues(obj.GetType(), obj as IDictionary);
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

        protected object GetStackValue(Type type, bool reverse)
        {
            var list = CreateInstance(Type.GetType($"System.Collections.Generic.List`1[[{type.GetGenericArguments()[0].AssemblyQualifiedName}]]")) as IList;
            GetListValues(type, list);
            if(reverse)
            {
                list.Reverse();
            }
            return CreateInstance(type, list);
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
