using System;
using System.Linq;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.Utilities.ImageProcessing;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes.Canon
{
    class CanonMakerInfo : MakerInfo
    {
        internal override IFDHeader GetMakerNotesHeader(byte[] makerNotesFieldBytes, Endianness e, ImageFile processingFile)
        {
            IFDHeader result = null;

            CanonMakerNotesBytesParser parser = GetMakerNotesBytesParser();

            if (parser != null)
            {
                int offset = 8 + processingFile.TiffHeaderOffset - processingFile.MakerNotesTagOffset;
                result = parser.ParseBytes(makerNotesFieldBytes.ToList(), Endianness.BigEndian, offset);
            }

            return result;
        }

        internal override IFDHeader GetMakerNotesHeader(byte[] makerNotesFieldBytes, Endianness e)
        {
            throw new NotImplementedException();
        }

        private CanonMakerNotesBytesParser GetMakerNotesBytesParser()
        {
            return new CanonMakerNotesBytesParser();
        }

    }
}
