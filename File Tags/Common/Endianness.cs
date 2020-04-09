
namespace BAFactory.Fx.FileTags.Common
{
    /// <summary>
    /// Byte ordering. II is equivalent to Little Endian, MM to BigEndian. 
    /// </summary>
    enum Endianness
    {
        LittleEndian = 73 * 256 + 73,
        BigEndian = 77 * 256 + 77,
        II = LittleEndian,
        MM = BigEndian,
    }

}
