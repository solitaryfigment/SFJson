using System.Reflection;

namespace SFJson.Utils
{
    internal class DefaultHandler
    {
        internal static readonly MethodInfo DEFAULT_METHOD;

        static DefaultHandler()
        {
            DEFAULT_METHOD = typeof(DefaultHandler).GetMethod("GetDefault");
        }

        internal T GetDefault<T>()
        {
            return default(T);
        }
    }
}