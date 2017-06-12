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
            type = DetermineType(type);
            object obj = Activator.CreateInstance(type);
            MemberInfo[] memberInfos = type.GetMembers(BindingFlags.Instance | BindingFlags.Public);
            
            foreach(var child in Children)
            {
                if(child.Name == "$type")
                {
                    continue;
                }
                var memberInfo = memberInfos.FirstOrDefault(p => p.Name == child.Name);
                if(memberInfo is PropertyInfo)
                {
                    var propertyInfo = (PropertyInfo)memberInfo;
                    propertyInfo.SetValue(obj, child.GetValue(propertyInfo.PropertyType));
                }
                else if(memberInfo is FieldInfo)
                {
                    var fieldInfo = (FieldInfo)memberInfo;
                    fieldInfo.SetValue(obj, child.GetValue(fieldInfo.FieldType));
                }
            }
            
            return obj;
        }
    }
}
