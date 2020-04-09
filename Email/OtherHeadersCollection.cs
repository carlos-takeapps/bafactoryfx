using System;
using System.Collections.Generic;
using System.Text;

namespace BAFactory.Fx.Network.Email
{
    [Serializable]
    public class OtherHeadersCollection
    {
        private string[] collection;
        public string[] Collection
        {
            get { return collection; }
            set { collection = value; }
        }

        public OtherHeadersCollection()
        {
            collection = new string[0];
        }

        public void Add(string Value)
        {
            string[] newCollection = new string[this.collection.Length + 1];
            this.collection.CopyTo(newCollection, 0);
            newCollection[newCollection.Length - 1] = Value;
            this.collection = newCollection;
            return;
        }
    }
}
