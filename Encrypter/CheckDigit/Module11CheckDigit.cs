using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAFactory.Fx.Utilities.DataTransformation;
using BAFactory.Fx.Security.Cryptography;

namespace BAFactory.Fx.Cryptography.CheckDigit
{
    public class Module11CheckDigit : CheckDigitCalculator
    {
        public Module11CheckDigit()
        {
            Ponderer = new byte[] { 2, 3, 4, 5, 6, 7, 2, 3, 4, 5 };
        }

        public override uint CalculateCheckDigit(decimal v)
        {
            uint sum = 0;
            ulong digitsCount = Numbers.CountDigits((double)v);

            int pos = 0;
            for (uint i = 0; i <= digitsCount; ++i)
            {
                sum += Numbers.GetDigitAt(v, i) * Ponderer[pos];
                pos = pos == Ponderer.Length ? 0 : pos++;
            }

            uint remainder = sum % 11;
            uint result = 11 - remainder;

            if (result == 11)
            {
                result = 0;
            }
            else if (result == 10)
            {
                result = 9;
            }

            return result;
        }
    }
}
