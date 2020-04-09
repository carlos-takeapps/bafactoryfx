using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAFactory.Fx.Utilities.DataTransformation;

namespace BAFactory.Fx.Cryptography.CheckDigit
{
    public class Module10CheckDigit : CheckDigitCalculator
    {
        public Module10CheckDigit()
            : base()
        {
            Ponderer = new byte[] { 2, 1 };
        }

        public override uint CalculateCheckDigit(decimal v)
        {
            uint checkDigit = 0;
            uint sum = 0;
            ulong digitsCount = Numbers.CountDigits((double)v);

            int pos = 0;
            for (uint i = 0; i < digitsCount; ++i)
            {
                sum += Numbers.GetDigitAt(v, i) * Ponderer[pos];
                pos = pos == Ponderer.Length ? 0 : pos++;
            }

            uint last = Numbers.GetDigitAt(sum, 0);
            checkDigit = 10 - last;

            if (checkDigit == 10)
            {
                checkDigit = 0;
            }

            return checkDigit;
        }
    }
}
