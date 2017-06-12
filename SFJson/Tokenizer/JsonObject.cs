using System;
using System.Linq;
using System.Reflection;

namespace SFJson
{
    public class JsonObject : JsonToken
    {
        public override JsonType JsonType
        {
            get { return JsonType.Collection; }
        }

        public override T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }

        public override object GetValue(Type type)
        {
            object obj;
            MemberInfo[] memberInfos;
            if(Children.Count > 0 && Children[0].Name == "$type")
            {
                Console.WriteLine("Has $type");
                var typestring = Children[0].GetValue<string>();
                Console.WriteLine(typestring);
                var inheiritedType = Type.GetType(typestring);
                Console.WriteLine(inheiritedType);
                obj = Activator.CreateInstance(inheiritedType);
                memberInfos = inheiritedType.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            }
            else
            {
                Console.WriteLine("Does not have $type");
                obj = Activator.CreateInstance(type);
                memberInfos = type.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            }

            foreach(var child in Children)
            {
                if(child.Name == "$type")
                {
                    continue;
                }
                var memberInfo = memberInfos.FirstOrDefault(p => p.Name == child.Name);
                if(memberInfo is PropertyInfo)
                {
                    Console.WriteLine("PropertyInfo");
                    var propertyInfo = (PropertyInfo)memberInfo;
                    propertyInfo.SetValue(obj, child.GetValue(propertyInfo.PropertyType));
                }
                if(memberInfo is FieldInfo)
                {
                    Console.WriteLine("FieldInfo");
                    var fieldInfo = (FieldInfo)memberInfo;
                    fieldInfo.SetValue(obj, child.GetValue(fieldInfo.FieldType));
                }
            }
            return obj;
        }
    }
}