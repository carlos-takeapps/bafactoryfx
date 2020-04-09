using BAFactory.Fx.FileTags.Common;

namespace BAFactory.Fx.FileTags.Exif
{
    class TiffHeader
    {
        internal static int Length { get { return 8; } }
        internal Endianness ByteOrder { get; set; }
        internal byte[] TiffId
        {
            get
            {
                return (new byte[2] { (byte)0x00, (byte)0x2A });
            }
            private set { return; }
        }
        internal int OffsetToZerothIFD { get; set; }
    }
}
