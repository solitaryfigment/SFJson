using System;
using System.Collections;
using System.Linq;

namespace SFJson
{
    public class JsonArray : JsonToken
    {
        public override JsonType JsonType
        {
            get { return JsonType.Array; }
        }

        public override T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }

        public override object GetValue(Type type)
        {
            Console.WriteLine("Get Value: " + type);
            IList obj;
            JsonToken list = Children.FirstOrDefault(c => c.Name == "$values");
            if(Children.Count > 0 && Children[0].Name == "$type")
            {
                Console.WriteLine("Has $type");
                var typestring = Children[0].GetValue<string>();
                Console.WriteLine(typestring);
                type = Type.GetType(typestring);
                Console.WriteLine(type);
                // TODO: Check if type exists and fallback
                if(type.IsArray)
                {
                    obj = Array.CreateInstance(type.GetElementType(), (list != null) ? list.Children.Count : Children.Count) as IList;
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
                    obj = Array.CreateInstance(type.GetElementType(), (list != null) ? list.Children.Count : Children.Count) as IList;
                }
                else
                {
                    obj = Activator.CreateInstance(type) as IList;
                }
            }

            if(list != null && list.Name == "$values")
            {
                Console.WriteLine("List: " + " : " + obj + " : " + type + " : " + list.JsonType + " : " + list.Name + " : " + list.Children.Count);
                for(int i = 0; i < list.Children.Count; i++)
                {
                    var child = list.Children[i];
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
            else
            {
                Console.WriteLine("List: " + " : " + obj + " : " + type);
                for(int i = 0; i < Children.Count; i++)
                {
                    var child = Children[i];
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
}