
namespace BAFactory.Fx.Utilities.Encoding
{
    /// <summary>
    /// Byte ordering. II is equivalent to Little Endian, MM to BigEndian. 
    /// This is used to file bytes read only. After file content is read, the sorts the bytes, if needed, and works internally with Little Endiand ordering.
    /// </summary>
    enum Endianness
    {
        LittleEndian = 73 * 256 + 73,
        BigEndian = 77 * 256 + 77,
        II = LittleEndian,
        MM = BigEndian,
    }

}
