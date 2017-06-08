using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SFJson
{
    #region Classes

    public enum JsonType
    {
        Array,
        Object,
        Value
    }

    public abstract class JsonToken
    {
        public bool IsQuoted;
        public string Name;
        public List<JsonToken> ChildElements = new List<JsonToken>();

        public abstract JsonType JsonType { get; }
        
        public abstract T GetValue<T>();
        public abstract object GetValue(Type type);
    }

    public class JsonObject : JsonToken
    {
        public override JsonType JsonType
        {
            get
            {
                return JsonType.Object;
            }
        }

        public override T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }

        public override object GetValue(Type type)
        {
            Console.WriteLine("Get Value");
            object obj;
            PropertyInfo[] properties;
            if(ChildElements[0].Name == "$Type")
            {
                Console.WriteLine("Has $Type");
                var typestring = ChildElements[0].GetValue<string>();
                Console.WriteLine(typestring);
                var inheiritedType = Type.GetType(typestring);
                Console.WriteLine(inheiritedType);
                obj = Activator.CreateInstance(inheiritedType);
                properties = inheiritedType.GetProperties();
            }
            else
            {
                obj = Activator.CreateInstance(type);
                properties = type.GetType().GetProperties();
            }

            foreach(var child in ChildElements)
            {
                if(child.Name == "$Type")
                {
                    continue;
                }

                Console.WriteLine("Name: " + child.Name);
                var propertyInfo = properties.First(p => p.Name == child.Name);
                Console.WriteLine("Name: " + propertyInfo.Name);
                Console.WriteLine(propertyInfo.GetValue(obj));
                propertyInfo.SetValue(obj, child.GetValue(propertyInfo.PropertyType));
            }
            return obj;
        }
    }

    public class JsonArray : JsonToken
    {
        public override JsonType JsonType
        {
            get
            {
                return JsonType.Array;
            }
        }

        public override T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }

        public override object GetValue(Type type)
        {
            Console.WriteLine("Get Value: " + type);
            IList obj;
            var list = ChildElements[1];
            if(ChildElements[0].Name == "$Type")
            {
                Console.WriteLine("Has $Type");
                var typestring = ChildElements[0].GetValue<string>();
                Console.WriteLine(typestring);
                type = Type.GetType(typestring);
                Console.WriteLine(type);
                if(type.IsArray)
                {
                    obj = Array.CreateInstance(type.GetElementType(), list.ChildElements.Count) as IList;
                }
                else
                {
                    obj = Activator.CreateInstance(type) as IList;
                }
            }
            else
            {
                if(type.IsArray)
                {
                    obj = Array.CreateInstance(type.GetElementType(), list.ChildElements.Count) as IList;
                }
                else
                {
                    obj = Activator.CreateInstance(type) as IList;
                }
            }

            if(list.Name == "$Values")
            {
                Console.WriteLine("List: " + " : " + obj + " : " + type + " : " + list.JsonType + " : " + list.Name + " : " + list.ChildElements.Count);
                for(int i = 0; i < list.ChildElements.Count; i++)
                {
                    var child = list.ChildElements[i];
                    var elementType = (type.IsArray) ? type.GetElementType() : type.GetGenericArguments()[0];
                    Console.WriteLine("Name: " + child.Name + " : " + child.GetValue(elementType));
                    if(type.IsArray)
                    {
                        obj[i] = child.GetValue(elementType);
                    }
                    else
                    {
                        obj.Add(child.GetValue(elementType));
                    }
                }
            }
            return obj;
        }
    }

    public class JsonValue : JsonToken
    {
        public override JsonType JsonType
        {
            get
            {
                return _jsonType;
            }
        }

        public object Value;

        private JsonType _jsonType;

        public JsonValue(string name, object value, JsonType jsonType)
        {
            Name = name;
            Value = value;
            _jsonType = jsonType;
        }

        public override T GetValue<T>()
        {
            return (T)Value;
        }

        public override object GetValue(Type type)
        {
            if(type.IsEnum)
            {
                return Enum.Parse(type, Value.ToString());
            }
            
            return Convert.ChangeType(Value, type);
        }
    }

    #endregion

    public class Deserializer
    {
        public string StringToDeserialize { get; set; }

        public Deserializer()
        {

        }

        public Deserializer(string stringToSerialize)
        {
            StringToDeserialize = stringToSerialize;
        }

        public T Deserialize<T>()
        {
            var token = Parser.Tokenize(StringToDeserialize);
            Console.WriteLine("Start");
            PrintTokens(token);
            Console.WriteLine("End");

            var obj = token.GetValue<T>();
            
            return obj;
        }

        private void PrintTokens(JsonToken token)
        {
            Console.WriteLine(token.JsonType);
            Console.WriteLine(token.ChildElements.Count);
            Console.WriteLine("{0} : {1}", token.Name, token.GetValue<object>());
            foreach(var child in token.ChildElements)
            {
                PrintTokens(child);
            }
        }
    }
}