using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Cryptography.CheckDigit
{
    public class CBUCheckDigit : Module10CheckDigit
    {
        public CBUCheckDigit()
            : base()
        { 
            Ponderer = new byte[] { 3, 1, 7, 9 };
        }

    }
}
