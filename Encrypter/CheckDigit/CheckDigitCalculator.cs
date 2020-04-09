using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Cryptography.CheckDigit
{
    public abstract class CheckDigitCalculator
    {
        public byte[] Ponderer { get; set; }

        public abstract uint CalculateCheckDigit(decimal v);
    }
}
