using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif;
using BAFactory.Fx.FileTags.Exif.IFD;

namespace BAFactory.Fx.Utilities.ImageProcessing
{
    public class ImageFile : BaseFile
    {
        internal int App1HeaderOffset { get; set; }
        internal int TiffHeaderOffset
        {
            get { return App1HeaderOffset + App1Header.TiffHeaderOffset; }
        }
        internal int ZerothHeaderOffset
        {
            get { return App1HeaderOffset + App1.ZerothHeaderOffset; }
        }
        internal int ExifHeaderOffset
        {
            get { return App1HeaderOffset + App1.ZerothHeaderOffset + App1.ZerothHeader.OffsetToNextIFD; }
        }
        internal int InteropHeaderOffset
        {
            get { return TiffHeaderOffset + App1.ExifHeader.OffsetToNextIFD; }
        }
        internal int FirstHeaderOffset
        {
            get { return TiffHeaderOffset + App1.ZerothHeader.OffsetTo1stIFD; }
        }
        internal int MakerNotesTagOffset
        {
            get
            {
                var makerNoteQuery = from f in this.App1.ExifHeader.IFDFields where f.FieldCode == (uint)IFDTagCode.MakerNote select f;
                if (makerNoteQuery.Count() == 1)
                {
                    return makerNoteQuery.First().ValueElementsOffset + TiffHeaderOffset;
                }
                return 0;
            }
        }
        internal App1Header App1 { get; set; }
        internal ImageFile(string filePath)
            : base(filePath)
        { }

    }
}
