using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.Utilities.ImageProcessing;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon
{
    class NikonMakerInfo : MakerInfo
    {
        internal override IFDHeader GetMakerNotesHeader(byte[] makerNotesFieldBytes, Endianness e)
        {
            IFDHeader result = null;

            NikonMakerNotesHeaderFormat format = IdentifyMakerNoteHeaderFormat(makerNotesFieldBytes.ToArray(), e);

            MakerNotesBytesParser parser = GetMakerNotesBytesParser(format);

            if (parser != null)
            {
                result = parser.ParseBytes(makerNotesFieldBytes.ToList(), e);
            }

            return result;
        }
        internal override IFDHeader GetMakerNotesHeader(byte[] makerNotesFieldBytes, Endianness e, ImageFile processingFile)
        {
            IFDHeader result = null;

            NikonMakerNotesHeaderFormat format = IdentifyMakerNoteHeaderFormat(makerNotesFieldBytes.ToArray(), e);

            MakerNotesBytesParser parser = GetMakerNotesBytesParser(format);

            if (parser != null)
            {
                int offsetCorrectionIndex = -processingFile.MakerNotesTagOffset + processingFile.TiffHeaderOffset + 8;
                result = parser.ParseBytes(makerNotesFieldBytes.ToList(), e, offsetCorrectionIndex);
            }

            return result;
        }
        internal NikonMakerNotesHeaderFormat IdentifyMakerNoteHeaderFormat(byte[] makerNotesBytes, Endianness e)
        {
            if (makerNotesBytes[0] == 0x4e &&
                makerNotesBytes[1] == 0x69 &&
                makerNotesBytes[2] == 0x6b &&
                makerNotesBytes[3] == 0x6f &&
                makerNotesBytes[4] == 0x6e &&
                makerNotesBytes[5] == 0x00 &&
                makerNotesBytes[6] == 0x02 &&
                makerNotesBytes[7] == 0x00 &&
                makerNotesBytes[8] == 0x00 &&
                makerNotesBytes[9] == 0x00)
            {
                return NikonMakerNotesHeaderFormat.NikonType3;
            }
            else if (
               makerNotesBytes[0] == 0x4e &&
               makerNotesBytes[1] == 0x69 &&
               makerNotesBytes[2] == 0x6b &&
               makerNotesBytes[3] == 0x6f &&
               makerNotesBytes[4] == 0x6e &&
               makerNotesBytes[5] == 0x00 &&
               makerNotesBytes[6] == 0x01 &&
               makerNotesBytes[7] == 0x00)
            {
                return NikonMakerNotesHeaderFormat.NikonType1;
            }

            return NikonMakerNotesHeaderFormat.UNKNOWN;
        }
        private MakerNotesBytesParser GetMakerNotesBytesParser(NikonMakerNotesHeaderFormat format)
        {
            MakerNotesBytesParser result = null;
            switch (format)
            {
                case NikonMakerNotesHeaderFormat.NikonType1:
                    result = new NikonType1MakerNotesBytesParser();
                    break;
                case NikonMakerNotesHeaderFormat.NikonType3:
                    result = new NikonType3MakerNotesBytesParser();
                    break;
            }
            return result;
        }
    }
}
