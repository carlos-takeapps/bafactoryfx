using System;
using System.Collections.Generic;
using System.Linq;

namespace BAFactory.Fx.FileTags.Exif.IFD
{
    class IFDHeader
    {
        internal Type FieldsCodesType
        {
            get;
            private set;
        }
        internal int FieldsCount { get; set; }
        internal List<IFDField> IFDFields { get; set; }
        internal int OffsetTo1stIFD { get; set; }
        internal List<byte> FieldsData { get; set; }
        internal int OffsetToNextIFD { get; set; }
        internal IFDHeader MakerHeader { get; set; }
        internal IFDHeader(Type fieldCodesType)
        {
            FieldsCodesType = fieldCodesType;
            IFDFields = new List<IFDField>();
        }
        internal void RemoveMarkerNotesIFD()
        {
            IEnumerable<IFDField> makerNotes = IFDFields.Where(x => x.FieldCode == (uint)IFDTagCode.MakerNote);
            if (makerNotes != null && makerNotes.Count() > 0)
            {
                IFDFields.Remove(makerNotes.First());
            }
        }
    }
}
