using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.Utilities.Encoding;

namespace BAFactory.Fx.FileTags.Exif.IFD
{
    class IFDTagElementTypeMappingDictionary : TagElementTypeMappingDictionary
    {
        internal IFDTagElementTypeMappingDictionary()
            : base()
        {
            this.Add((uint)IFDTagElementType.BYTE, new TagElementTypeMapping((uint)IFDTagElementType.BYTE, 1, typeof(byte)));
            this.Add((uint)IFDTagElementType.ASCII, new TagElementTypeMapping((uint)IFDTagElementType.ASCII, 1, typeof(string)));
            this.Add((uint)IFDTagElementType.SHORT, new TagElementTypeMapping((uint)IFDTagElementType.SHORT, 2, typeof(ushort)));
            this.Add((uint)IFDTagElementType.LONG, new TagElementTypeMapping((uint)IFDTagElementType.LONG, 4, typeof(uint)));
            this.Add((uint)IFDTagElementType.RATIONAL, new TagElementTypeMapping((uint)IFDTagElementType.RATIONAL, 8, typeof(Rational)));
            this.Add((uint)IFDTagElementType.UNDEFINED, new TagElementTypeMapping((uint)IFDTagElementType.UNDEFINED, 1, typeof(byte)));
            this.Add((uint)IFDTagElementType.SLONG, new TagElementTypeMapping((uint)IFDTagElementType.SLONG, 4, typeof(int)));
            this.Add((uint)IFDTagElementType.SRATIONAL, new TagElementTypeMapping((uint)IFDTagElementType.SRATIONAL, 8, typeof(Rational)));
        }
    }
}
