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
                Console.WriteLine("Type String: " + typestring);
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
    }
}