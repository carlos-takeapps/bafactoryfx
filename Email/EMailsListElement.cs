using System;
using System.Collections.Generic;
using System.Text;

namespace BAFactory.Fx.Network.Email
{
    [Serializable]
    public class EMailsListElement
    {
        private int number;
        private long bytes;
        private bool retrieved;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public long Bytes
        {
            get { return bytes; }
            set { bytes = value; }
        }
        public bool Retrieved
        {
            get { return retrieved; }
            set { retrieved = value; }
        }

        public EMailsListElement()
        { }
    }
}
