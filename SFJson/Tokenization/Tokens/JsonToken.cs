using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SFJson.Conversion.Settings;

namespace SFJson.Tokenization.Tokens
{
    public abstract class JsonToken
    {
        public string Name;
        public List<JsonToken> Children = new List<JsonToken>();
        internal DeserializerSettings DeserializerSettings;
        
        protected Func<Type, object> OnNullValue;

        public abstract JsonType JsonType { get; }

        public T GetValue<T>()
        {
            return (T) GetValue(typeof(T));
        }

        public abstract object GetValue(Type type);

        protected Type DetermineType(Type type)
        {
            if(Children.Count > 0 && Children[0].Name == "$type")
            {
                var typestring = Children[0].GetValue<string>();
                var inheiritedType = Type.GetType(typestring);
                if(inheiritedType != null)
                {
                    return CheckForBoundTypes(inheiritedType);
                }
            }

            return CheckForBoundTypes(type);
        }

        private Type CheckForBoundTypes(Type type)
        {
            var returnType = DeserializerSettings?.TypeBindings?.TryGetValue(type);
            returnType = returnType ?? GlobalSettings.TryGetTypeBinding(type);
            return returnType ?? type;
        }

        protected object CreateInstance(Type type)
        {
            if(type == null)
            {
                return null;
            }

            var elementType = type.GetElementType();
            if(type.IsArray && elementType != null)
            {
                JsonToken list = Children.FirstOrDefault(c => c.Name == "$values");
                list = list ?? this;
                return Array.CreateInstance(elementType, list.Children.Count);
            }

            return Activator.CreateInstance(type);
        }

        protected object GetDictionaryValues(Type type, IDictionary obj)
        {
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];
            foreach(var child in Children)
            {
                if(child.Name == "$type")
                {
                    continue;
                }

                var key = child.Name;
                var token = new Tokenizer().Tokenize(key, DeserializerSettings);
                token.OnNullValue = ReturnNull;
                var keyValue = token.GetValue(keyType);
                if(keyValue == null)
                {
                    if(DeserializerSettings != null && DeserializerSettings.SkipNullKeysInKeyValuedCollections)
                    {
                        continue;
                    }

                    throw new NullReferenceException("Cannot add null key to dictionary.");
                }

                obj.Add(keyValue, child.GetValue(valueType));
            }

            return obj;
        }

        private object ReturnNull(Type type)
        {
            return null;
        }

        protected object GetListValues(Type type, IList obj)
        {
            var list = Children.FirstOrDefault(c => c.Name == "$values");
            var elementType = (type.IsArray) ? type.GetElementType() : type.GetGenericArguments()[0];
            list = list ?? this;
            for(var i = 0; i < list.Children.Count; i++)
            {
                if(type.IsArray)
                {
                    obj[i] = list.Children[i].GetValue(elementType);
                }
                else
                {
                    obj.Add(list.Children[i].GetValue(elementType));
                }
            }

            return obj;
        }
    }
}
