using System;
using System.Collections;
using System.Text;

namespace SFJson.Utils
{
    /// <summary>
    /// Defines extension methods for <c>Type</c>.
    /// </summary>
    public static class TypeExtension
    {
        internal static string StackTypeFormat = "System.Collections.Generic.Stack`1[[{0}]], System";
        internal static string QueueTypeFormat = "System.Collections.Generic.Queue`1[[{0}]], System";
        
        internal static void Reverse(this IList list)
        {
            for(var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                list.RemoveAt(i);
                list.Insert(0,element);
            }
        }
        
        /// <summary>
        /// Calls <code>default(T)</code> for <paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// The default value of <paramref name="type" type="typeof(type)"/>
        /// </returns>
        public static object GetDefault(this Type type)
        {
            return TypeUtilHandler.DEFAULT_METHOD.MakeGenericMethod(type).Invoke(new TypeUtilHandler(), null);
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

        internal static bool IsStack(this Type type)
        {
            return type.GetGenericArguments().Length == 1 &&
                   type.IsAssignableFrom(Type.GetType(string.Format(StackTypeFormat, type.GetGenericArguments()[0].AssemblyQualifiedName)));
        }

        internal static bool IsQueue(this Type type)
        {
            return type.GetGenericArguments().Length == 1 &&
                   type.IsAssignableFrom(Type.GetType(string.Format(QueueTypeFormat, type.GetGenericArguments()[0].AssemblyQualifiedName)));
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
                else
                {
                    sb.AppendFormat("{0}[{1}]", type.Name.Substring(0, type.Name.IndexOf('[')), genericStringBuilder);
                    throw new Exception("What is going on???");
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
