using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Impress
{
    public static class StringExtensions
    {
        public static string Repeat(this string text, int count)
        {
            if (count == 0)
            {
                return "";
            }
            else if (count == 1)
            {
                return text;
            }
            else
            {
                var builder = new StringBuilder();

                for (int i = 0; i < count; i++)
                {
                    builder.Append(text);
                }

                return builder.ToString();
            }
        }

        public static string JoinWith(this IEnumerable<string> texts, string separator)
        {
            return string.Join(separator, texts);
        }

        public static string JoinWith<T>(this IEnumerable<T> texts, string separator)
        {
            return string.Join(separator, texts.Select(t => t == null ? "" : t.ToString()));
        }

        public static bool ToBool(this string text)
        {
            return ToBool(text, false);
        }

        /// <summary>
        /// Returns the boolean value of the text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="emptyValue">The value to use if the text is null or empty</param>
        /// <returns></returns>
        public static bool ToBool(this string text, bool emptyValue)
        {
            return string.IsNullOrEmpty(text) ? emptyValue : "True".Equals(text);
        }

        public static string[] ParseArguments(this string text, string separator)
        {
            return text.Contains(separator) ? text.Split(new string[] { separator }, System.StringSplitOptions.None) : new string[] { text };
        }

        public static Maybe<string>[] MaybeParseArguments(this string text, string separator)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Maybe<string>[0];
            }
            return text.Contains(separator) ? text.Split(new string[] { separator }, System.StringSplitOptions.None).Select(s => s.ToMaybe()).ToArray() : new Maybe<string>[] { text.ToMaybe() };
        }

        public static string FirstLetterUpperCase(this string text)
        {
            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }

        public static string Interpolate(this string template, params object[] values)
        {
            return string.Format(template, values);
        }

        public static IList<string> SplitByLineSize(this string text, int maxCharSize)
        {
            var splitText = text.Split(' ');
            var textLines = new List<string>();


            var sb = new StringBuilder(splitText[0]);

            for (int i = 1; i < splitText.Length; i++)
            {
                var word = splitText[i];
                if (sb.Length + 1 + word.Length <= maxCharSize)
                {
                    sb.Append(" ").Append(word);
                }
                else
                {
                    textLines.Add(sb.ToString());
                    sb = new StringBuilder(word);
                }
            }
            if (sb.Length > 0)
            {
                textLines.Add(sb.ToString());
            }

            return textLines;
        }

        public static string GetOnlyNumbers(this string text)
        {
            return Regex.Match(text, @"\d+").Value;
        }

        public static string EnsureBeginsWith(this string value, string prefix)
        {
            if (!value.StartsWith(prefix))
            {
                return prefix + value;
            }
            return value;
        }

        public static string EnsureEndsWith(this string value, string sufix)
        {
            if (!value.EndsWith(sufix))
            {
                return value + sufix;
            }
            return value;
        }


        private static string ToSQLDateTime(DateTime dateTime)
        {
            return "{0}-{1}-{2} {3}:{4}:{5}".Interpolate(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static string InterpolateData(this string command, object dataHolder)
        {
            var type = dataHolder.GetType();
            var pos = command.IndexOf('@');
            while (pos >= 0)
            {
                var end = IndexOf(command, pos + 1, " ", ",", ")");

                var name = end < 0 ? command.Substring(pos + 1) : command.Substring(pos + 1, end - pos - 1);

                object value = type.GetProperty(name).GetGetMethod().Invoke(dataHolder, null);

                if (value == null)
                {
                    value = "null";
                }
                else if (value is string)
                {
                    value = "'" + value.ToString() + "'";
                }
                else if (value is bool)
                {
                    value = ((bool)value) ? 1 : 0;
                }
                else if (value is double)
                {
                    value = ((double)value).ToString(CultureInfo.InvariantCulture);
                }
                else if (value is float)
                {
                    value = ((float)value).ToString(CultureInfo.InvariantCulture);
                }
                else if (value is decimal)
                {
                    value = ((decimal)value).ToString(CultureInfo.InvariantCulture);
                }
                else if (value is DateTime)
                {
                    value = "'" + ToSQLDateTime((DateTime)value) + "'";
                }

                command = command.Replace("@" + name, value.ToString());
                pos = command.IndexOf('@');
            }
            return command;
        }

        private static int IndexOf(string s, int start, params string[] values)
        {
            var min = -1;
            for (var i = 0; i < values.Length; i++)
            {
                var pos = s.IndexOf(values[i], start);
                if (min < 0 && pos > 0)
                {
                    min = pos;
                }
                else if (pos > 0 && pos < min)
                {
                    min = pos;
                }
            }

            return min;
        }

        public static bool EqualsIgnoreCase(this string a, string b)
        {
            return a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string Limit(this string str, int limit)
        {
            if (str.Length > limit && str.Length > limit - 3)
            {
                return str.Substring(0, limit) + "...";
            }
            return str;
        }

        public static string TakeLast(this string str, int lastAmount)
        {
            if (str == null || lastAmount >= str.Length)
            {
                return str;
            }
            return str.Substring(str.Length - lastAmount);
        }
    }
}
