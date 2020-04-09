
namespace BAFactory.Fx.Utilities.Encoding
{
    /// <summary>
    /// Equivalent to two LONGs (8 bytes total). Value returns the cotient.
    /// </summary>
    public struct Rational
    {
        public long Numerator { get; set; }
        public long Denominator { get; set; }
        public double Value
        {
            get
            {
                if (Denominator == 0)
                {
                    return 0;
                }
                else
                {
                    return (double)Numerator / (double)Denominator;
                }
            }
        }
    }
}
