using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace SFJson
{
    public enum TypeHandler
    {
        None,
        Collections,
        Objects,
        All
    }
    public class SerializerSettings
    {
        public TypeHandler TypeHandler { get; set; }
    }
    
    public class Serializer
    {
        public object ObjectToSerialize { get; set; }
        private SerializerSettings _serializerSettings;

        public Serializer()
        {

        }

        public Serializer(object objectToSerialize)
        {
            ObjectToSerialize = objectToSerialize;
        }

        public override string ToString()
        {
            return Serialize(new SerializerSettings { TypeHandler = TypeHandler.None });
        }

        public string Serialize()
        {
            return Serialize(new SerializerSettings { TypeHandler = TypeHandler.None });
        }

        public string Serialize(SerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
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
                if(_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Objects)
                {
                    sb.AppendFormat("\"$type\":\"{0}\"", obj.GetType().GetTypeAsString());
                    SerializeProperties(sb, obj);
                }
                else
                {
                    SerializeProperties(sb, obj);
                }
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

                if(_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Collections)
                {
                    sb.AppendFormat("\"$type\":\"{0}\"", list.GetType().GetTypeAsString());
                    sb.Append(",\"$values\":[");
                }
                for(int i = 0; i < list.Count; i++)
                {
                    if(i > 0)
                    {
                        sb.Append(",");
                    }
                    SerializeObject(sb, list[i].GetType(), list[i]);
                }
                if(_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Collections)
                {
                    sb.Append("]");
                }
                sb.Append("]");
            }
        }

        private void SerializeProperties(StringBuilder sb, object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for(int i = 0; i < properties.Length; i++)
            {
                var propertyInfo = properties[i];
                if(i > 0 || (_serializerSettings.TypeHandler == TypeHandler.All || _serializerSettings.TypeHandler == TypeHandler.Objects))
                {
                    sb.Append(",");
                }
                if(propertyInfo.CanWrite && propertyInfo.CanRead)
                {
                    sb.AppendFormat("\"{0}\":", propertyInfo.Name);
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