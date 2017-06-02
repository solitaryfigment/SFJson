using System;
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
                var typeStringSplit = obj.GetType().AssemblyQualifiedName.Split(',');
//                Console.WriteLine("Here: " + typeStringSplit);
                sb.AppendFormat("\"$Type\":\"{0}\"", typeStringSplit[0] + "," + typeStringSplit[1]);
                SerializeProperties(sb, obj);
                sb.Append("}");
            }
        }

        private void SerializeProperties(StringBuilder sb, object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(var propertyInfo in properties)
            {
                SerializeProperty(sb, propertyInfo, obj);
            }
        }

        private void SerializeProperty(StringBuilder sb, PropertyInfo propertyInfo, object obj)
        {
            if(propertyInfo.CanWrite && propertyInfo.CanRead)
            {
                if(propertyInfo.PropertyType.IsPrimitive)
                {
                    sb.AppendFormat(",\"{0}\":{1}", propertyInfo.Name, propertyInfo.GetValue(obj));
                }
                else if(propertyInfo.PropertyType.IsArray)
                {
                    // TODO
                    sb.AppendFormat(",\"{0}\":{1}", propertyInfo.Name, propertyInfo.GetValue(obj));
                }
                else
                {
                    sb.AppendFormat(",\"{0}\":", propertyInfo.Name);
                    SerializeObject(sb, propertyInfo.GetValue(obj));
                }
            }
        }
    }
}