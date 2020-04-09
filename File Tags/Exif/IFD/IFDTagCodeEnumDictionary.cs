
namespace BAFactory.Fx.FileTags.Exif.IFD
{
    class IFDTagCodeEnumDictionary : TagCodeEnumDictionary
    {
        internal IFDTagCodeEnumDictionary()
            : base()
        {
            this.Add((uint)IFDTagCode.Flash, typeof(ExifFlash));
        }
    }
}
