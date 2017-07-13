using System;
using System.Collections;

namespace SFJson
{
    public class JsonCollection : JsonToken
    {
        public override JsonType JsonType
        {
            get { return JsonType.Collection; }
        }

        public override object GetValue(Type type)
        {
            try
            {
                type = DetermineType(type);
                var obj = CreateInstance(type);
                if(type.Implements(typeof(IList)))
                {
                    obj = GetListValues(type, obj as IList);
                }
                return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Token - {0} : {1}", Name, type);
                throw;
            }
        }
    }
}
