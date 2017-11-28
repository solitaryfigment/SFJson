using System.Reflection;

namespace SFJson.Utils
{
    internal class TypeUtilHandler
    {
        internal static readonly MethodInfo DEFAULT_METHOD;

        static TypeUtilHandler()
        {
            DEFAULT_METHOD = typeof(TypeUtilHandler).GetMethod("GetDefault");
        }

        internal T GetDefault<T>()
        {
            return default(T);
        }

        internal T ConvertToType<T>(object obj)
        {
            return (T)obj;
        }
    }
}
