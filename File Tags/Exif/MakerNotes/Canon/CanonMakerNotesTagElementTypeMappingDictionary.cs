using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAFactory.Fx.FileTags.Exif.MakerNotes.Canon;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.Utilities.Encoding;

namespace BAFactory.Fx.FileTags.Exif.DynamicTypeRegistration
{
    internal class CanonMakerNotesTagElementTypeMappingDictionary : TagElementTypeMappingDictionary
    {
        internal CanonMakerNotesTagElementTypeMappingDictionary()
            : base()
        {
            this.Add((uint)CanonMakerNotesTagElementType.BYTE, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.BYTE, 1, typeof(byte)));
            this.Add((uint)CanonMakerNotesTagElementType.ASCII, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.ASCII, 1, typeof(string)));
            this.Add((uint)CanonMakerNotesTagElementType.SHORT, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.SHORT, 2, typeof(ushort)));
            this.Add((uint)CanonMakerNotesTagElementType.LONG, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.LONG, 4, typeof(uint)));
            this.Add((uint)CanonMakerNotesTagElementType.RATIONAL, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.RATIONAL, 8, typeof(Rational)));
            this.Add((uint)CanonMakerNotesTagElementType.UNDEFINED, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.UNDEFINED, 1, typeof(byte)));
            this.Add((uint)CanonMakerNotesTagElementType.SLONG, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.SLONG, 4, typeof(int)));
            this.Add((uint)CanonMakerNotesTagElementType.SRATIONAL, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.SRATIONAL, 8, typeof(Rational)));
            this.Add((uint)CanonMakerNotesTagElementType.SSHORT, new TagElementTypeMapping((uint)CanonMakerNotesTagElementType.SHORT, 2, typeof(short)));
        }
    }
}