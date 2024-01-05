using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Text;
using WebPulse_WebManager.Enums;

namespace WebPulse_WebManager.Utility
{
    public static class StringExtensions
    {
        public static string SpaceCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            StringBuilder sb = new StringBuilder(value.Length * 2);
            sb.Append(value[0]);

            for (int i = 1; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    sb.Append(' ');
                }

                sb.Append(value[i]);
            }

            return sb.ToString();
        }


        public static string GenerateRandomUsername()
        {
            string username = string.Empty;

            string adjective = EnumExtensions.GetRandomEnumValue<ApprovedAdjectives>().ToString();
            string noun = EnumExtensions.GetRandomEnumValue<ApprovedNouns>().ToString();

            username = adjective + noun;
            return username;
        }

        public static string Invert(this string value)
        {
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string Uglify(this string value)
        {
            string uglyString = string.Empty;

            foreach (char c in value)
            {
                uglyString += c + " ";
            }

            return uglyString;
        }

        public static string ToBase64(this string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64(this string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
