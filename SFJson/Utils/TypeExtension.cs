using System;
using System.Text;

namespace SFJson.Utils
{
    /// <summary>
    /// Defines extension methods for <c>Type</c>.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Calls <code>default(T)</code> for <paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// The default value of <paramref name="type" type="typeof(type)"/>
        /// </returns>
        public static object GetDefault(this Type type)
        {
            return DefaultHandler.DEFAULT_METHOD.MakeGenericMethod(type).Invoke(new DefaultHandler(), null);
        }

        /// <summary>
        /// Determine if <paramref name="type"/> implements <paramref name="interfaceType"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// <c>true</c> if <paramref name="type"/> implements <paramref name="interfaceType"/>
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool Implements<T>(this Type type, T interfaceType) where T : class
        {
            if(!(interfaceType is Type) || !(interfaceType as Type).IsInterface)
            {
                throw new ArgumentException("Only interfaces can be 'implemented'.");
            }

            return (interfaceType as Type).IsAssignableFrom(type);
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
    }
}
