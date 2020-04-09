using BAFactory.Fx.FileTags.OutputTypes;

namespace BAFactory.Fx.FileTags.Exif.IFD
{
    public class IFDField
    {
        internal uint FieldCode { get; set; }
        internal uint ValueElementType { get; set; }
        internal int ValueElementsCount { get; set; }
        internal byte[] FieldValueElements { get; set; }
        public string FieldName { get; set; }
        public string Text { get; set; }
        internal IFDHeaderType Block { get; set; }
        internal bool IsOffset { get; set; }
        internal int ValueElementsOffset { get; set; }
        internal FilePropertySource FieldSource
        {
            get { return FilePropertySource.Exif; }
            private set { return; }
        }
        internal FilePropertyGroup FieldGroup
        {
            get { return FilePropertyGroup.Image; }
            private set { return; }
        }

        static public explicit operator FileProperty(IFDField f)
        {
            FileProperty result = null;

            if (f != null)
            {
                string fieldName = string.IsNullOrEmpty(f.FieldName) ? "???" : f.FieldName.ToString().Replace("_", string.Empty);
                result = new FileProperty(string.Concat("(", f.FieldCode, ") ", fieldName), f.Text);
            }

            result.FieldGroup = f.FieldGroup;
            result.FieldSource = f.FieldSource;

            return result;
        }
    }
}
