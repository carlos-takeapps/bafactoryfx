using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.FileTags.Exif
{
    class TagElementTypeMapping 
    {
        internal uint Id { get; set; }
        internal int BytesLength { get; set; }
        internal Type DestinationType { get; set; }
        internal TagElementTypeMapping(uint type, int length, Type destinationType)
        {
            Id = type;
            BytesLength = length;
            DestinationType = destinationType;
        }
    }

}
