
namespace BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon
{
    enum NikonType3MakerNotesTagElementType
    {
            BYTE = 1,       // An 8-bit unsigned integer.
            ASCII = 2,      // An 8-bit byte containing one 7-bit ASCII code. The final byte is terminated with NULL.
            SHORT = 3,      // A 16-bit (2-byte) unsigned integer.
            LONG = 4,       // A 32-bit (4-byte) unsigned integer.
            RATIONAL = 5,   // Two LONGs. The first LONG is the numerator and the second LONG expresses the denominator.
            UNDEFINED = 7,  // An 8-bit byte that can take any value depending on the field definition.
            SLONG = 9,      // A 32-bit (4-byte) signed integer (2's complement notation).
            SSHORT = 8,
            SRATIONAL = 10, // Two SLONGs. The first SLONG is the numerator and the second SLONG is the denominator.
    }
}
