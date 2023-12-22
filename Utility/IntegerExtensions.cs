using Microsoft.AspNetCore.Mvc;

namespace WebPulse_WebManager.Utility
{

    public static class IntegerExtensions
    {
        public static int CreateRandomInteger(int digits = 4)
        {
            if (digits <= 0)
            {
                throw new ArgumentException("Number of digits must be greater than zero", nameof(digits));
            }

            Random random = new Random();
            int minValue = (int)Math.Pow(10, digits - 1);
            int maxValue = (int)Math.Pow(10, digits) - 1;

            return random.Next(minValue, maxValue + 1);
        }

    }
}
