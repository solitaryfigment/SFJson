using System;
using System.Globalization;
using System.Text;

namespace SFJson.Utils
{
    internal static class StringExtension
    {
        internal static string EscapeQuotes(this string value)
        {
            if(String.IsNullOrEmpty(value))
            {
                return String.Empty;
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
                        if(Char.GetUnicodeCategory(c) != UnicodeCategory.Control)
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