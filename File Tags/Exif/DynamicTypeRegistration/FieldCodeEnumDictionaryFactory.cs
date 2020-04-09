using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon;
using BAFactory.Fx.FileTags.Exif.DynamicTypeRegistration;

namespace BAFactory.Fx.FileTags.Exif
{
    static class FieldCodeEnumDictionaryFactory
    {
        internal static TagCodeEnumDictionary GetFieldCodeEnumDictionary<T>()
        {
            TagCodeEnumDictionary result = null;
            switch (typeof(T).ToString())
            {
                case "BAFactory.Fx.FileTags.Exif.IFD.IFDTagCode":
                    result = new IFDTagCodeEnumDictionary();
                    break;
                case "BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon.NikonType1MakerNotesTagCode":
                    result = new NikonType3MakerNotesTagCodeEnumDictionary();
                    break;
                case "BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon.NikonType3MakerNotesTagCode":
                    result = new NikonType3MakerNotesTagCodeEnumDictionary();
                    break;
                case "BAFactory.Fx.FileTags.Exif.MakerNotes.Canon.CanonMakerNotesTagCode":
                    result = new CanonMakerNotesTagCodeEnumDictionary();
                    break;
            }
            return result;
        }

    }
}
