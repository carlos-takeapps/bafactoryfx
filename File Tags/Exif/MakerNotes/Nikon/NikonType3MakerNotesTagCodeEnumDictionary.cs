
namespace BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon
{
    class NikonType3MakerNotesTagCodeEnumDictionary : TagCodeEnumDictionary
    {
        internal  NikonType3MakerNotesTagCodeEnumDictionary()
            : base()
        {
            this.Add((uint)NikonType3MakerNotesTagCode.Saturation, typeof(NikonType3MakerNotesSaturation));
        }
    }
}
