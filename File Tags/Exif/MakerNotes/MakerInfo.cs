using System;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.FileTags.Exif.MakerNotes.Canon;
using BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon;
using BAFactory.Fx.Utilities.ImageProcessing;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes
{
    abstract class MakerInfo 
    {
        internal abstract IFDHeader GetMakerNotesHeader(byte[] makerNotesFieldBytes, Endianness e);
        internal abstract IFDHeader GetMakerNotesHeader(byte[] makerNotesFieldBytes, Endianness e, ImageFile processingFile);
        internal static MakerInfo GetMakerInfo(IFDField makeField)
        {
            MakerInfo result = null;
            Maker maker = (Maker)Enum.Parse(typeof(Maker), makeField.Text, true);

            switch (maker)
            {
                case Maker.Nikon:
                    result = new NikonMakerInfo();
                    break;
                case Maker.Canon:
                    result = new CanonMakerInfo();
                    break;
            }

            return result;
        }
    }
}
