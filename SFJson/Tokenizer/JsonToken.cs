using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SFJson
{
    public abstract class JsonToken
    {
        public string Name;
        public List<JsonToken> Children = new List<JsonToken>();

        public abstract JsonType JsonType { get; }
        
        public T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }
        
        public abstract object GetValue(Type type);

        protected Type DetermineType(Type type)
        {
            if(Children.Count > 0 && Children[0].Name == "$type")
            {
                var typestring = Children[0].GetValue<string>();
                var inheiritedType = Type.GetType(typestring);
                return inheiritedType;
            }

            return type;
        }

        protected object CreateInstance(Type type)
        {
            if(type.IsArray)
            {
                JsonToken list = Children.FirstOrDefault(c => c.Name == "$values");
                list = (list == null) ? this : list;
                return Array.CreateInstance(type.GetElementType(), list.Children.Count) as IList;
            }
            return Activator.CreateInstance(type);
        }
        
        protected object GetDictionaryValues(Type type, IDictionary obj)
        {
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];
            
            for(int i = 0; i < Children.Count; i++)
            {
                if(Children[i].Name != "$type")
                {
                    var key = Children[i].Name;
                    var token = new Tokenizer().Tokenize(key);
                    obj.Add(token.GetValue(keyType), Children[i].GetValue(valueType));
                }
            }
            
            return obj;
        }

        protected object GetListValues(Type type, IList obj)
        {
            var list = Children.FirstOrDefault(c => c.Name == "$values");
            var elementType = (type.IsArray) ? type.GetElementType() : type.GetGenericArguments()[0];

            list = (list == null) ? this : list;
            
            for(int i = 0; i < list.Children.Count; i++)
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
