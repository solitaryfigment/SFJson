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
            type = DetermineType(type);
            var elementType = (type.IsArray) ? type.GetElementType() : type.GetGenericArguments()[0];
            var obj = CreateInstance(type) as IList;
            var list = Children.FirstOrDefault(c => c.Name == "$values");
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
