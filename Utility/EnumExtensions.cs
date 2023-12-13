using Microsoft.AspNetCore.Mvc;
using System;

namespace WebPulse_WebManager.Utility
{
    public static class EnumExtensions
    {
        private static readonly Random random = new Random();


        public static T GetRandomEnumValue<T>() where T : struct, Enum
        {
            Array values = Enum.GetValues(typeof(T))!;
            return (T)values.GetValue(random.Next(values.Length))!;
        }


    }
}
