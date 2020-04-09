using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.FileTags.OutputTypes
{
    [Flags]
    public enum FilePropertySource
    {
        All,
        Exif,
        GPS,
        XMP,
    }
}
