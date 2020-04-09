using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.Utilities.Encoding;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes.Nikon
{
    class NikonType1MakerNotesBytesParser : MakerNotesBytesParser
    {
        private ExifFileProcessor exifProcessor;
        internal NikonType1MakerNotesBytesParser()
        {
            exifProcessor = new ExifFileProcessor();
        }
        internal override IFDHeader ParseBytes(List<byte> bytes, Endianness e)
        {
            return ParseBytes(bytes, e, 0);
        }
        internal override IFDHeader ParseBytes(List<byte> bytes, Endianness e, int offsetCorrectionIndex)
        {
            IFDHeader result = new IFDHeader(typeof(NikonType1MakerNotesTagCode));

            byte[] fieldsCountBytes = ByteManager.ChopByteArray(bytes.ToArray(), 8, 2);
            byte[] sortedFieldsCountBytes = exifProcessor.SortBytes(e, fieldsCountBytes);
            result.FieldsCount = ByteManager.GetIntValue(sortedFieldsCountBytes);

            int voided = 0;

            byte[][] fieldsBytes = exifProcessor.ExtractTagsBytes(ByteManager.ChopByteArray(bytes.ToArray(), 8 + 2, result.FieldsCount * 12).ToList(), result.FieldsCount).ToArray();

            foreach (byte[] fieldBytes in fieldsBytes)
            {
                IFDField field = exifProcessor.GetIFDField<NikonType1MakerNotesTagCode>(fieldBytes, e);
                field.Block = IFDHeaderType.MakerNotesHeader;
                exifProcessor.LoadFieldTextValue<NikonType1MakerNotesTagCode>(ref field, bytes, offsetCorrectionIndex, 0, e, voided);
                result.IFDFields.Add(field);
            }

            return result;
        }
    }
}
