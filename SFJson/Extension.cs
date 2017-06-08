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
                sb.AppendFormat("{0}[[{1}]]", type.FullName.Substring(0, type.FullName.IndexOf('[')),
                    string.Join(", ", type.GetGenericArguments()
                        .Select(t => t.GetTypeAsString())));
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
    }
}