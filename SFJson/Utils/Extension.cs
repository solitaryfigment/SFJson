using System;
using System.Linq;
using System.Text;

namespace SFJson
{
    public static class Extension
    {
        public static string GetTypeAsString(this Type type, bool appendAssembly = true)
        {
            var sb = new StringBuilder();
            
            if(type.IsGenericType)
            {
                var appendSeparator = false;
                var genericStringBuilder = new StringBuilder();
                foreach(var genericType in type.GetGenericArguments())
                {
                    if(appendSeparator)
                    {
                        genericStringBuilder.Append(",");
                    }
                    genericStringBuilder.Append("[");
                    genericStringBuilder.Append(genericType.GetTypeAsString());
                    genericStringBuilder.Append("]");
                    appendSeparator = true;
                }
                sb.AppendFormat("{0}[{1}]", type.FullName.Substring(0, type.FullName.IndexOf('[')), genericStringBuilder.ToString());
            }
            else if(type.IsNested)
            {
                sb.AppendFormat("{0}+{1}", type.DeclaringType.GetTypeAsString(false), type.Name);
            }
            else
            {
                sb.AppendFormat("{0}.{1}", type.Namespace, type.Name);
            }

            if(appendAssembly)
            {
                sb.AppendFormat(", {0}", type.Assembly.GetName().Name);
            }
            
            return sb.ToString();
        }
        
        public static bool Implements<I>(this Type type, I s) where I : class  
        {
            if(((s as Type)==null) || !(s as Type).IsInterface)
                throw new ArgumentException("Only interfaces can be 'implemented'.");

            return (s as Type).IsAssignableFrom(type);
        }
    }
}
