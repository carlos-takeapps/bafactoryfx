using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.Utilities.Encoding;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes.Canon
{
    class CanonMakerNotesBytesParser : MakerNotesBytesParser
    {
        private ExifFileProcessor exifProcessor;
        internal CanonMakerNotesBytesParser()
        {
            exifProcessor = new ExifFileProcessor();
        }
        internal override IFDHeader ParseBytes(List<byte> ifdFieldBytes, Endianness e)
        {
            // using 12 as default because 12 is the minimum number of bytes before a TIFF Header
            return ParseBytes(ifdFieldBytes, e, 12);
        }
        internal override IFDHeader ParseBytes(List<byte> bytes, Endianness e, int offset)
        {
            IFDHeader result = new IFDHeader(typeof(CanonMakerNotesTagCode));

            byte[] fieldsNumberBytes = new byte[2];
            fieldsNumberBytes[0] = ByteManager.ChopByteArray(bytes.ToArray(), 0, 1)[0];
            int fieldsNumber = ByteManager.GetShortValue(fieldsNumberBytes);

            int voided = 0;
            byte[][] fieldsBytes = exifProcessor.ExtractTagsBytes(ByteManager.ChopByteArray(bytes.ToArray(), 1, fieldsNumber * 12).ToList(), fieldsNumber).ToArray();

            foreach (byte[] fieldBytes in fieldsBytes)
            {
                byte[] orderedBytes = OrderIFDFieldBytes(fieldBytes);

                IFDField field = exifProcessor.GetIFDField<CanonMakerNotesTagCode>(orderedBytes, e);

                field.Block = IFDHeaderType.MakerNotesHeader;

                field.ValueElementsOffset = ByteManager.GetIntValue(field.FieldValueElements);

                LoadFieldTextValue<CanonMakerNotesTagCode>(ref field, bytes, offset, offset, e, voided);

                result.IFDFields.Add(field);
            }

            return result;
        }
        internal int LoadFieldTextValue<T>(ref IFDField field, List<byte> fileBytes, int fieldDataOffset, int tiffHeaderOffset, Endianness e, int lastBlockByteFileOffset)
        {
            if (field.FieldCode == (int)CanonMakerNotesTagCode.CameraSettings1)
            {
                return GetCameraSettings1TextValue(ref field, fileBytes, fieldDataOffset, tiffHeaderOffset, e, lastBlockByteFileOffset);
            }
            else
            {
                return exifProcessor.LoadFieldTextValue<CanonMakerNotesTagCode>(ref field, fileBytes, fieldDataOffset, tiffHeaderOffset, e, lastBlockByteFileOffset);
            }
        }
        private int GetCameraSettings1TextValue(ref IFDField field, List<byte> fileBytes, int fieldDataOffset, int tiffHeaderOffset, Endianness e, int lastBlockByteFileOffset)
        {
            byte[] valueBytes;
            lastBlockByteFileOffset = 0;

            long fixedOffset = fieldDataOffset + (long)field.ValueElementsOffset - 8;

            valueBytes = ByteManager.ChopByteArray(fileBytes.ToArray(), fixedOffset, 32);

            byte[] format = ByteManager.ChopByteArray(valueBytes, 0, 2);
            StringBuilder strBldr = new StringBuilder();
            strBldr.AppendFormat("Macro Mode: {0}; ", Enum.ToObject(typeof(CanonMakerInfoCammeraSettings1MacroMode), ByteManager.GetIntValue(format)));

            byte[] quality = ByteManager.ChopByteArray(valueBytes, 3, 2);
            strBldr.AppendFormat("Quality: {0}; ", Enum.ToObject(typeof(CanonMakerInfoCammeraSettings1Quality), ByteManager.GetIntValue(quality)));

            field.Text = strBldr.ToString();
            return 0;
        }
        byte[] OrderIFDFieldBytes(byte[] orig)
        {
            byte[] result = new byte[12];

            result[0] = orig[0];
            result[1] = orig[1];
            result[2] = orig[2];
            result[3] = orig[3];
            result[4] = 0;
            result[5] = 0;
            result[6] = orig[4];
            result[7] = orig[5];
            result[8] = orig[8];
            result[9] = 0;
            result[10] = orig[10];
            result[11] = orig[9];

            return result;
        }

    }
}
