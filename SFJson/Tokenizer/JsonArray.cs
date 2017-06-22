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

        public override object GetValue(Type type)
        {
            type = DetermineType(type);
            var obj = CreateInstance(type);
            if(type.GetInterface("IList") != null)
            {
                obj = GetListValues(type, obj as IList);
            }
            else if(type.Implements(typeof(IDictionary)))
            {
                obj = GetDictionaryValues(type, obj as IDictionary);
            }
            return obj;
        }

        private object GetListValues(Type type, IList obj)
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
