using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SFJson
{
    internal class DefaultExtension
    {
        public static MethodInfo DefaultMethod;
        
        static DefaultExtension()
        {
            DefaultMethod = typeof(DefaultExtension).GetMethod("GetDefault");
        }

        public T GetDefault<T>()
        {
            return default(T);
        }
    }
    
    internal static class Extension
    {
        public static object GetDefault(this Type type)
        {
            return DefaultExtension.DefaultMethod.MakeGenericMethod(type).Invoke(new DefaultExtension(), null);
        }
        
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

        public static string EscapeQuotes(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            foreach (var c in value)
            {
                if (c == Constants.QUOTE)
                {
                    sb.AppendFormat(@"\""");
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        
        public static string ToLiteral(this string input)
        {
            var literal = new StringBuilder(input.Length);
            foreach (var c in input)
            {
                switch (c)
                {
                    case '\'': literal.Append(@"\'"); break;
                    case '\"': literal.Append("\\\""); break;
                    case '\\': literal.Append(@"\\"); break;
                    case '\0': literal.Append(@"\0"); break;
                    case '\a': literal.Append(@"\a"); break;
                    case '\b': literal.Append(@"\b"); break;
                    case '\f': literal.Append(@"\f"); break;
                    case '\n': literal.Append(@"\n"); break;
                    case '\r': literal.Append(@"\r"); break;
                    case '\t': literal.Append(@"\t"); break;
                    case '\v': literal.Append(@"\v"); break;
                    default:
                        if (Char.GetUnicodeCategory(c) != UnicodeCategory.Control)
                        {
                            literal.Append(c);
                        }
                        else
                        {
                            literal.Append(@"\u");
                            literal.Append(((ushort)c).ToString("x4"));
                        }
                        break;
                }
            }
            return literal.ToString();
        }
    }
}
