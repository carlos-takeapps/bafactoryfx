using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.FileTags.OutputTypes
{
    public class FileProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public FilePropertySource FieldSource {get; set;}
        public FilePropertyGroup FieldGroup { get; set; }

        public FileProperty(string n, string v)
        {
            Name = n;
            Value = v;
        }
    }
}
