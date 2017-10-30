using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace SFJson.Utils
{
    internal class DefaultExtension
    {
        internal static readonly MethodInfo DEFAULT_METHOD;

        static DefaultExtension()
        {
            DEFAULT_METHOD = typeof(DefaultExtension).GetMethod("GetDefault");
        }

        internal T GetDefault<T>()
        {
            return default(T);
        }
    }

    internal static class Extension
    {
        internal static object GetDefault(this Type type)
        {
            return DefaultExtension.DEFAULT_METHOD.MakeGenericMethod(type).Invoke(new DefaultExtension(), null);
        }

        internal static string GetTypeAsString(this Type type, bool appendAssembly = true)
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

                // TODO: else throw exception
                if(type.FullName != null)
                {
                    sb.AppendFormat("{0}[{1}]", type.FullName.Substring(0, type.FullName.IndexOf('[')), genericStringBuilder);
                }
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

        public static bool Implements<T>(this Type type, T interfaceType) where T : class
        {
            if(!(interfaceType is Type) || !(interfaceType as Type).IsInterface)
            {
                throw new ArgumentException("Only interfaces can be 'implemented'.");
            }

            return (interfaceType as Type).IsAssignableFrom(type);
        }

        public static string EscapeQuotes(this string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach(var c in value)
            {
                if(c == Constants.QUOTE)
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

        internal static string ToLiteral(this string input)
        {
            var literal = new StringBuilder(input.Length);
            foreach(var c in input)
            {
                switch(c)
                {
                    case '\'':
                        literal.Append(@"\'");
                        break;
                    case '\"':
                        literal.Append("\\\"");
                        break;
                    case '\\':
                        literal.Append(@"\\");
                        break;
                    case '\0':
                        literal.Append(@"\0");
                        break;
                    case '\a':
                        literal.Append(@"\a");
                        break;
                    case '\b':
                        literal.Append(@"\b");
                        break;
                    case '\f':
                        literal.Append(@"\f");
                        break;
                    case '\n':
                        literal.Append(@"\n");
                        break;
                    case '\r':
                        literal.Append(@"\r");
                        break;
                    case '\t':
                        literal.Append(@"\t");
                        break;
                    case '\v':
                        literal.Append(@"\v");
                        break;
                    default:
                        if(char.GetUnicodeCategory(c) != UnicodeCategory.Control)
                        {
                            literal.Append(c);
                        }
                        else
                        {
                            literal.Append(@"\u");
                            literal.Append(((ushort) c).ToString("x4"));
                        }
                        break;
                }
            }

            return literal.ToString();
        }
    }
}
