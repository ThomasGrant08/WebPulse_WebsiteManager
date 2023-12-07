using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Text;

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
    }
}
