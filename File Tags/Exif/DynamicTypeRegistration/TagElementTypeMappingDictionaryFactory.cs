using System;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon;
using BAFactory.Fx.FileTags.Exif.DynamicTypeRegistration;

namespace BAFactory.Fx.FileTags.Exif
{
    static class TagElementTypeMappingDictionaryFactory
    {
        internal static TagElementTypeMappingDictionary GetMappingDictionary(Type t)
        {
            TagElementTypeMappingDictionary result = null;
            switch (t.ToString())
            {
                case "BAFactory.Fx.FileTags.Exif.IFD.IFDTagCode":
                    result = new IFDTagElementTypeMappingDictionary();
                    break;
                case "BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon.NikonType1MakerNotesTagCode":
                    result = new NikonType3MakerNotesTagElementTypeMappingDictionary();
                    break;
                case "BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon.NikonType3MakerNotesTagCode":
                    result = new NikonType3MakerNotesTagElementTypeMappingDictionary();
                    break;
                case "BAFactory.Fx.FileTags.Exif.MakerNotes.Canon.CanonMakerNotesTagCode":
                    result = new CanonMakerNotesTagElementTypeMappingDictionary();
                    break;
            }
            return result;
        }
    }
}
