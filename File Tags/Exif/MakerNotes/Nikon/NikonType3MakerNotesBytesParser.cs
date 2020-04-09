using System.Collections.Generic;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif.IFD;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon
{
    class NikonType3MakerNotesBytesParser : MakerNotesBytesParser
    {
        private ExifFileProcessor exifProcessor;
        internal NikonType3MakerNotesBytesParser()
        {
            exifProcessor = new ExifFileProcessor();
        }
        internal override IFDHeader ParseBytes(List<byte> bytes, Endianness e)
        {
            return ParseBytes(bytes, e, 0);
        }
        internal override IFDHeader ParseBytes(List<byte> bytes, Endianness e, int offsetCorrectionIndex)
        {
            IFDHeader result = new IFDHeader(typeof(NikonType3MakerNotesTagCode));

            TiffHeader tiffHeader = exifProcessor.GetTiffHeader(bytes, 10);
            result = exifProcessor.GetIFDHeader<NikonType3MakerNotesTagCode>(IFDHeaderType.MakerNotesHeader, bytes, 10 + 8, 10 + 8, tiffHeader.ByteOrder);
            return result;
        }
    }
}
