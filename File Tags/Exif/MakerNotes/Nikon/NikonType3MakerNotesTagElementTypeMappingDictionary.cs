using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.Utilities.Encoding;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon
{
    class NikonType3MakerNotesTagElementTypeMappingDictionary : TagElementTypeMappingDictionary
    {
        internal  NikonType3MakerNotesTagElementTypeMappingDictionary()
            : base()
        {
            this.Add((uint)NikonType3MakerNotesTagElementType.BYTE, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.BYTE, 1, typeof(byte)));
            this.Add((uint)NikonType3MakerNotesTagElementType.ASCII, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.ASCII, 1, typeof(string)));
            this.Add((uint)NikonType3MakerNotesTagElementType.SHORT, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.SHORT, 2, typeof(ushort)));
            this.Add((uint)NikonType3MakerNotesTagElementType.LONG, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.LONG, 4, typeof(uint)));
            this.Add((uint)NikonType3MakerNotesTagElementType.RATIONAL, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.RATIONAL, 8, typeof(Rational)));
            this.Add((uint)NikonType3MakerNotesTagElementType.UNDEFINED, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.UNDEFINED, 1, typeof(byte)));
            this.Add((uint)NikonType3MakerNotesTagElementType.SLONG, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.SLONG, 4, typeof(int)));
            this.Add((uint)NikonType3MakerNotesTagElementType.SRATIONAL, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.SRATIONAL, 8, typeof(Rational)));
            this.Add((uint)NikonType3MakerNotesTagElementType.SSHORT, new TagElementTypeMapping((uint)NikonType3MakerNotesTagElementType.SHORT, 2, typeof(short)));
        }
    }
}
