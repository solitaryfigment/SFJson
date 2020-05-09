using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SFJson.Attributes;
using SFJson.Conversion;
using SFJson.Utils;
using SFJson.WrapperObjects;

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
        /// <summary>
        /// Returns the token type, a collection will always be <c>JsonTokenType.Object</c>
        /// </summary>
        public override JsonTokenType JsonTokenType
        {
            get { return JsonTokenType.Object; }
        }
        
        public override void SetupChildrenForType(Type type)
        {
            var memberInfos = type?.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            foreach(var child in Children)
            {
                FindMemberInfoOfToken(child, memberInfos);
            }
        }
        
        /// <inheritdoc cref="JsonToken.GetValue"/>
        public override object GetValue(Type type, object instance = null)
        {
            type = DetermineType(type);
            SetupChildrenForType(type);
            var obj = CreateInstance(type, instance);
            var isDict = IsGenericDictionary(obj, type, out var dictionaryWrapper);
            var isList = IsGenericList(obj, type, out var listWrapper);
            if(isDict)
            {
                return GetDictionaryValues(dictionaryWrapper);
            }
            if(type.IsStack())
            {
                return GetStackValue(type, true);
            }
            if(type.IsQueue())
            {
                return GetStackValue(type, false);
            }
            if(isList)
            {
                return GetListValues(type, listWrapper);
            }

            if(type == typeof(Object))
            {
                obj = new Dictionary<string, Object>();
                return GetDictionaryValues(new DictionaryWrapper<string, Object>((Dictionary<string, Object>)obj));
            }
            
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
            return CreateInstance(type, null, list);
        }

        private void SetValue(JsonToken child, object obj)
        {
            var customConverterAttribute = (CustomConverterAttribute)child.MemberInfo?.GetCustomAttributes(typeof(CustomConverterAttribute), true).FirstOrDefault();
            var customConverter = (customConverterAttribute != null) ? (CustomConverter)Activator.CreateInstance(customConverterAttribute.ConverterType) : (CustomConverter)null;
            if(child.MemberInfo is PropertyInfo propertyInfo)
            {
                if(customConverter != null)
                {
                    propertyInfo.SetValue(obj, customConverter.Convert(child, propertyInfo.PropertyType), null);
                }
                else
                {
                    propertyInfo.SetValue(obj, child.GetValue(propertyInfo.PropertyType), null);
                }
            }
            else if(child.MemberInfo is FieldInfo fieldInfo)
            {
                if(customConverter != null)
                {
                    fieldInfo.SetValue(obj, customConverter.Convert(child, fieldInfo.FieldType));
                }
                else
                {
                    fieldInfo.SetValue(obj, child.GetValue(fieldInfo.FieldType));
                }
            }
        }

        private void FindMemberInfoOfToken(JsonToken child, MemberInfo[] memberInfos)
        {
            var memberInfo = memberInfos?.FirstOrDefault(m => m.GetCustomAttributes(true).Any(a => a is JsonNamedValue value && value.Name == child.Name) || m.Name == child.Name);
            if(memberInfo == null)
            {
                return;
            }

            var ignoreAttribute = (JsonIgnore)memberInfo.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonIgnore));
            child.MemberInfo = (ignoreAttribute == null) ? memberInfo : null;
        }
    }
}
