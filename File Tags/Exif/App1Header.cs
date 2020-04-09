using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAFactory.Fx.FileTags.Exif;
using BAFactory.Fx.FileTags.Exif.IFD;

namespace BAFactory.Fx.FileTags.Exif
{
    class App1Header
    {
        static internal byte[] App1Marker
        {
            get { return new byte[2] { (byte)0xFF, (byte)0xE1 }; }
        }

        static internal byte[] ExifHeaderMark
        {
            get { return new byte[6] { (byte)0x45, (byte)0x78, (byte)0x69, (byte)0x66, (byte)0x00, (byte)0x00 }; }
        }

        /// <summary>
        /// Offset of the Tiff header. Is relative to the start of the APP1 block.
        /// </summary>
        internal static int TiffHeaderOffset { get { return 10; } }
        internal uint Length { get; set; }
        internal TiffHeader TiffHeader { get; set; }
        internal int ZerothHeaderOffset
        {
            get
            { return App1Header.TiffHeaderOffset + TiffHeader.Length + TiffHeader.OffsetToZerothIFD; }
        }
        internal IFDHeader ZerothHeader { get; set; }
        internal int ExifHeaderOffset
        {
            get
            {
                return App1Header.TiffHeaderOffset + TiffHeader.Length + ZerothHeader.OffsetTo1stIFD;
            }
        }
        internal IFDHeader ExifHeader { get; set; }
        internal int InteropHeaderOffset { get; set; }
        internal IFDHeader InteropHeader { get; set; }
        internal int GPSHeaderOffset { get; set; }
        internal IFDHeader GPSHeader { get; set; }
        internal int FirstHeaderOffset { get; set; }
        internal IFDHeader FirstHeader { get; set; }
    }
}
