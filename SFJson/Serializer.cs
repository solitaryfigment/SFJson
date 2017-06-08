using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace SFJson
{
    public class Serializer
    {
        public object ObjectToSerialize { get; set; }

        public Serializer()
        {

        }

        public Serializer(object objectToSerialize)
        {
            ObjectToSerialize = objectToSerialize;
        }

        public override string ToString()
        {
            return Serialize();
        }

        public string Serialize()
        {
            var sb = new StringBuilder();
            SerializeObject(sb, ObjectToSerialize);
            return sb.ToString();
        }

        private void SerializeObject(StringBuilder sb, object obj)
        {
            if(obj == null)
            {
                sb.Append("null");
            }
            else
            {
                sb.Append("{");
                sb.AppendFormat("\"$Type\":\"{0}\"", obj.GetType().GetTypeAsString());
                SerializeProperties(sb, obj);
                sb.Append("}");
            }
        }
        
        private void SerializeList(StringBuilder sb, IList list)
        {
            if(list == null)
            {
                sb.Append("null");
            }
            else
            {
                Console.WriteLine("List: " + list.Count);
                sb.Append("[");
                sb.AppendFormat("\"$Type\":\"{0}\"", list.GetType().GetTypeAsString());
                sb.Append(",\"$Values\":[");
                for(int i = 0; i < list.Count; i++)
                {
                    if(i > 0)
                    {
                        sb.Append(",");
                    }
                    SerializeObject(sb, list[i].GetType(), list[i]);
                }
                sb.Append("]]");
            }
        }

        private void SerializeProperties(StringBuilder sb, object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(var propertyInfo in properties)
            {
                if(propertyInfo.CanWrite && propertyInfo.CanRead)
                {
                    sb.AppendFormat(",\"{0}\":", propertyInfo.Name);
                    SerializeObject(sb, propertyInfo.PropertyType, propertyInfo.GetValue(obj));
                }
            }
        }

        private void SerializeObject(StringBuilder sb, Type type, object value)
        {
            Console.WriteLine(type);
            if(type.IsPrimitive)
            {
                Console.WriteLine("IsPrimitive");
                sb.AppendFormat("{0}", value);
            }
            else if(type.IsEnum)
            {
                Console.WriteLine("IsEnum");
                sb.AppendFormat("{0}", value.ToString());
            }
            else if(type == typeof(string))
            {
                Console.WriteLine("string");
                sb.AppendFormat("\"{0}\"", value);
            }
            else if(type.IsArray || type.GetInterface("IList") != null)
            {
                Console.WriteLine("IList");
                SerializeList(sb, (IList)value);
            }
            else
            {
                Console.WriteLine("Else");
                SerializeObject(sb, value);
            }
        }
    }
}