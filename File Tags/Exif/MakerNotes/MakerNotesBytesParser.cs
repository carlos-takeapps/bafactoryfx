using System.Collections.Generic;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif.IFD;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes
{
    abstract class MakerNotesBytesParser
    {
        internal abstract IFDHeader ParseBytes(List<byte> ifdFieldBytes, Endianness e);
        internal abstract IFDHeader ParseBytes(List<byte> ifdFieldBytes, Endianness e, int offsetCorrectionIndex);
    }
}
