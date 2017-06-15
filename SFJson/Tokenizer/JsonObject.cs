using System;
using System.Linq;
using System.Reflection;

namespace SFJson
{
    public class JsonObject : JsonToken
    {
        private MemberInfo[] _memberInfos;
        
        public override JsonType JsonType
        {
            get { return JsonType.Collection; }
        }

        public override object GetValue(Type type)
        {
            type = DetermineType(type);
            object obj = Activator.CreateInstance(type);
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
            var memberInfo = _memberInfos.FirstOrDefault(p => p.Name == child.Name);
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
    }
}
