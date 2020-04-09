using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Utilities.DataTransformation
{
    public static class Numbers
    {
        public static uint GetDigitAt(decimal value, decimal pos)
        {
            return (uint)GetDigitsAt(value, pos, 1);
        }

        public static decimal GetDigitsAt(decimal value, decimal pos, decimal digits)
        {
            decimal stack = Round(value, pos);
            decimal mask = Round(value, pos + digits) * (decimal)Math.Pow(10, (double)digits);

            return stack - mask;
        }

        public static decimal Round(decimal n, decimal padding)
        {
            decimal divisor = (decimal)Math.Pow((double)10, (double)padding);
            decimal ceil = (decimal)Math.Ceiling(((n + 0.01m)) / divisor);
            return (ceil - 1);
        }

        public static ulong CountDigits(double n)
        {
            return (ulong)Math.Floor(Math.Log10(n) + 1);
        }

        public static NumbersToWordsConverter CreateNumbersToWordsConverter(string locale)
        {
            NumbersToWordsConverter result = null;
            switch(locale)
            {
                case "es":
                    result = new NumbersToSpanishWordsConverter();
                    break;
            }
            return result;
        }

        public static T CreateNumbersToWordsConverter<T>() where T: NumbersToWordsConverter
        {
            T concrete = Activator.CreateInstance<T>();
            return concrete;
        }
    }
}
