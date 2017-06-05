using System;
using System.Linq;
using System.Text;

namespace SFJson
{
    public static class Extension
    {
        public static string GetTypeAsString(this Type type)
        {
            var sb = new StringBuilder();
            if(type.IsGenericType)
            {
                sb.AppendFormat("{0}[[{1}]], {2}", type.FullName.Substring(0, type.FullName.IndexOf('[')),
                    string.Join(", ", type.GetGenericArguments()
                        .Select(t => GetTypeAsString(t))),
                    type.Assembly.GetName().Name);
            }
            else
            {
                sb.AppendFormat("{0}.{1}, {2}", type.Namespace, type.Name, type.Assembly.GetName().Name);
            }
            return sb.ToString();
        }
    }
}