using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using SFJson.Attributes;

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
            var obj = CreateInstance(type);
            
            if(type.Implements(typeof(IDictionary)))
            {
                return GetDictionaryValues(type, obj as IDictionary);
            }
            
            if(type.Implements(typeof(IList)))
            {
                return GetListValues(type, obj as IList);
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

        private void SetValue(JsonToken child, object obj)
        {
            var memberInfo = _memberInfos.FirstOrDefault(m => ((m.GetCustomAttributes().Any(a => a is JsonValueName && ((JsonValueName)a).Name == child.Name))) || (m.Name == child.Name));
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
