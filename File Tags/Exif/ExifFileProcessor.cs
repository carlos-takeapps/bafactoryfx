using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.FileTags.Exif.MakerNotes;
using BAFactory.Fx.Utilities.Encoding;
using BAFactory.Fx.Utilities.ImageProcessing;

namespace BAFactory.Fx.FileTags.Exif
{
    class ExifFileProcessor
    {
        internal int LocateApp1FileMarkIndex(List<byte> fileBytes)
        {
            int result = -1;
            for (int i = 0; i < fileBytes.Count - 6; ++i)
            {
                if (fileBytes[i] == App1Header.App1Marker[0] &&
                    fileBytes[i + 1] == App1Header.App1Marker[1])
                {
                    result = i;
                    break;
                }
                ++i;
            }
            return result;
        }
        internal App1Header ExtractApp1Header(List<byte> fileBytes, int app1HeaderFileOffset)
        {
            App1Header header = new App1Header();

            header.Length = GetApp1SectionLength(fileBytes, app1HeaderFileOffset);

            if (!CheckExifIdentifierPosition(fileBytes, app1HeaderFileOffset))
            {
                throw new InvalidDataException("Exif ID is not properly located on APP1 Header");
            }

            return header;
        }
        internal TiffHeader GetTiffHeader(List<byte> fileBytes, int tiffHeaderIndex)
        {
            TiffHeader result = new TiffHeader();

            byte[] tiffHeaderBytes = ByteManager.ChopByteArray(fileBytes.ToArray(), tiffHeaderIndex, TiffHeader.Length);

            result.ByteOrder = GetEndiannes(tiffHeaderBytes.ToList());

            if (!CheckTiffId(tiffHeaderBytes.ToList(), result.ByteOrder))
            {
                throw new InvalidDataException("TIFF ID is not properly located on TIFF Header");
            }

            result.OffsetToZerothIFD = GetOffsetToZerothIFD(tiffHeaderBytes, result.ByteOrder);

            return result;
        }
        internal IFDHeader GetIFDHeader<T>(IFDHeaderType blockType, List<byte> fileBytes, int blockDataOffset, int tiffHeaderOffset, Endianness e)
        {
            if ((blockType == IFDHeaderType.InteroperatibilityIFD || blockType == IFDHeaderType.FirstIFD)
                && blockDataOffset == tiffHeaderOffset)
            {
                return null;
            }

            int lastBlockByteFileOffset = 0;

            IFDHeader result = new IFDHeader(typeof(T));

            byte[] fieldsCountBytes = ByteManager.ChopByteArray(fileBytes.ToArray(), blockDataOffset, 2);
            byte[] sortedFieldsCountBytes = SortBytes(e, fieldsCountBytes);
            result.FieldsCount = ByteManager.GetIntValue(sortedFieldsCountBytes);

            result.IFDFields = GetIFDFields<T>(blockType, fileBytes, blockDataOffset, (int)result.FieldsCount, tiffHeaderOffset, e, out lastBlockByteFileOffset);

            if (blockType == IFDHeaderType.ZerothIFD)
            {
                result.OffsetToNextIFD = lastBlockByteFileOffset - blockDataOffset;
            }
            else if (blockType == IFDHeaderType.ExifIFD)
            {
                var query = from f in result.IFDFields where f.FieldCode == (uint)IFDTagCode.InteropOffset select f;
                if (query != null && query.Count() > 0)
                {
                    IFDField interopField = query.First();
                    result.OffsetToNextIFD = ByteManager.GetIntValue(interopField.FieldValueElements);
                }
            }

            byte[] offsetToNextIFDHeader = ByteManager.ChopByteArray(fileBytes.ToArray(), blockDataOffset + 2 + result.FieldsCount * 12, 4);
            byte[] sortedOffsetToNextIFDHeader = SortBytes(e, offsetToNextIFDHeader);
            int offset = ByteManager.GetIntValue(sortedOffsetToNextIFDHeader);

            result.OffsetTo1stIFD = offset;

            return result;
        }
        internal bool LoadMakerNotesHeader(ref IFDHeader exifHeaderRef, IFDField makeField, List<byte> fileBytes, Endianness e, ImageFile processingFile)
        {
            if (exifHeaderRef == null) return true;

            IFDField makerNoteField = FindField(exifHeaderRef.IFDFields, (uint)IFDTagCode.MakerNote);

            if (makerNoteField != null)
            {
                byte[] makerNotesBytes = ByteManager.ChopByteArray(fileBytes.ToArray(), processingFile.TiffHeaderOffset + makerNoteField.ValueElementsOffset, makerNoteField.ValueElementsCount);

                exifHeaderRef.MakerHeader = GetMakerNoteIFDHeader(makeField, makerNotesBytes, e, processingFile);
            }
            return (exifHeaderRef.MakerHeader != null && exifHeaderRef.MakerHeader.IFDFields != null);
        }
        internal IFDField FindFirstField(List<IFDField> fields, params uint[] codes)
        {
            IFDField result = null;
            int i = 0;
            while (!(result != null || i == codes.Count()))
            {
                result = FindField(fields, codes[i++]);
            }
            return result;
        }
        internal IFDField FindField(List<IFDField> fields, uint code)
        {
            IEnumerable<IFDField> fieldQuery = fields.Where(field => field.FieldCode == code);

            IFDField result = null;
            if (fieldQuery != null && fieldQuery.Count() > 0)
            {
                result = fieldQuery.First();
            }

            return result;
        }
        internal IFDHeader GetMakerNoteIFDHeader(IFDField makeField, byte[] makerNotesFieldBytes, Endianness e, ImageFile processingFile)
        {
            IFDHeader result = null;

            MakerInfo makerInfo = MakerInfo.GetMakerInfo(makeField);

            result = makerInfo.GetMakerNotesHeader(makerNotesFieldBytes, e, processingFile);

            return result;
        }
        internal byte[] GetIFDBytesValue<T>(List<byte> fileBytes, int blockDataOffset, IFDField field, Endianness endianness)
        {
            byte[] result;

            if (field.IsOffset)
            {
                TagElementTypeMappingDictionary typeMappingDictionary = TagElementTypeMappingDictionaryFactory.GetMappingDictionary(typeof(T));

                if (field.Block == IFDHeaderType.ExifIFD)
                {
                    result = ByteManager.ChopByteArray(fileBytes.ToArray(), 18 + (long)field.ValueElementsOffset - 8, (int)field.ValueElementsCount * typeMappingDictionary[field.ValueElementType].BytesLength);
                }
                else
                {
                    result = ByteManager.ChopByteArray(fileBytes.ToArray(), blockDataOffset + (long)field.ValueElementsOffset - 8, (int)field.ValueElementsCount * typeMappingDictionary[field.ValueElementType].BytesLength);
                }
            }
            else
            {
                result = field.FieldValueElements;
            }

            return result;
        }
        internal byte[] SortIFDFieldBytes(Endianness e, byte[] bytes)
        {
            if (e == Endianness.BigEndian)
            {
                byte[] result = new byte[12];
                ShiftBytes(ref result, bytes, 0, 2);
                ShiftBytes(ref result, bytes, 2, 2);
                ShiftBytes(ref result, bytes, 4, 4);
                ShiftBytes(ref result, bytes, 8, 4);
                return result;
            }
            else
            {
                return bytes;
            }
        }
        internal void ShiftBytes(ref byte[] result, byte[] bytes, int start, int count)
        {
            byte[] temp = ByteManager.ChopByteArray(bytes, start, count).Reverse().ToArray();
            temp.CopyTo(result, start);
        }
        internal uint GetApp1SectionLength(List<byte> bytes, int app1SectionIndex)
        {
            byte[] app1LengthBytes = ByteManager.ChopByteArray(bytes.ToArray(), app1SectionIndex + 2, 2);

            int app1Lenght = BitConverter.ToInt16(app1LengthBytes, 0);

            return (uint)app1Lenght;
        }
        internal bool CheckExifIdentifierPosition(List<byte> lb, int app1SectionIndex)
        {
            byte[] exifIdentifierBytes = ByteManager.ChopByteArray(lb.ToArray(), app1SectionIndex + 4, 6);

            return (exifIdentifierBytes[0] == App1Header.ExifHeaderMark[0] &&
                exifIdentifierBytes[1] == App1Header.ExifHeaderMark[1] &&
                exifIdentifierBytes[2] == App1Header.ExifHeaderMark[2] &&
                exifIdentifierBytes[3] == App1Header.ExifHeaderMark[3] &&
                exifIdentifierBytes[4] == App1Header.ExifHeaderMark[4] &&
                exifIdentifierBytes[5] == App1Header.ExifHeaderMark[5]);
        }
        internal int GetOffsetToZerothIFD(byte[] tiffHeaderBytes, Endianness endianness)
        {
            int result = 0;
            byte[] OffsetToCerothIFDBytes = ByteManager.ChopByteArray(tiffHeaderBytes, 4, 4);

            if (endianness == Endianness.BigEndian)
            {
                OffsetToCerothIFDBytes = OffsetToCerothIFDBytes.Reverse().ToArray();
            }
            int idx = ByteManager.GetIntValue(OffsetToCerothIFDBytes);

            if (idx == 8)
            {
                idx = 0;
            }
            result = idx;

            return result;
        }
        internal bool CheckTiffId(List<byte> bytes, Endianness e)
        {
            byte[] tiffIdBytes = ByteManager.ChopByteArray(bytes.ToArray(), 2, 2);
            byte[] sortedTiffBytes = SortBytes(e, tiffIdBytes);
            return (sortedTiffBytes[0] == 0x2A &&
                sortedTiffBytes[1] == 0x00);
        }
        internal Endianness GetEndiannes(List<byte> bytes)
        {
            byte[] endiannessBytes = ByteManager.ChopByteArray(bytes.ToArray(), 0, 2);

            int endiannessValue = (int)BitConverter.ToInt16(endiannessBytes, 0);

            return (Endianness)endiannessValue;
        }
        internal bool CheckEndianness(List<byte> bytes, Endianness endianness, int tiffHeaderIdx)
        {
            byte[] checkBytes = ByteManager.ChopByteArray(bytes.ToArray(), tiffHeaderIdx + 2, 2);

            if (endianness == Endianness.LittleEndian)
            {
                if (EndiannesCheckBytes.LittleEndian == (EndiannesCheckBytes)BitConverter.ToInt16(checkBytes, 0))
                {
                    return true;
                }
            }
            else if (endianness == Endianness.BigEndian)
            {
                if (EndiannesCheckBytes.BigEndian == (EndiannesCheckBytes)BitConverter.ToInt16(checkBytes, 0))
                {
                    return true;
                }
            }
            return false;
        }
        internal List<byte> GetIFDFieldsBytes(List<byte> bytes, int p, int count)
        {
            List<byte> blockDataBytes = ByteManager.ChopByteArray(bytes.ToArray(), p + 2, count * 12).ToList();
            return blockDataBytes;
        }
        internal byte[] SortBytes(Endianness e, byte[] fieldsCountBytes)
        {
            byte[] result = new byte[fieldsCountBytes.Length];

            if (e == Endianness.BigEndian)
            {
                ShiftBytes(ref result, fieldsCountBytes, 0, fieldsCountBytes.Length);
            }
            else
            {
                result = fieldsCountBytes;
            }

            return result;
        }
        internal List<IFDField> GetIFDFields<T>(IFDHeaderType block, List<byte> fileBytes, int blockDataOffset, int fieldsCount, int currentBlockTiffHeaderOffset, Endianness e, out int lastBlockByteFileOffset)
        {
            lastBlockByteFileOffset = 0;
            List<IFDField> result = new List<IFDField>();

            byte[][] fieldsBytes = ExtractTagsBytes(ByteManager.ChopByteArray(fileBytes.ToArray(), blockDataOffset + 2, fieldsCount * 12).ToList(), fieldsCount).ToArray();

            foreach (byte[] fieldBytes in fieldsBytes)
            {
                IFDField field = GetIFDField<T>(fieldBytes, e);
                field.Block = block;
                lastBlockByteFileOffset = LoadFieldTextValue<T>(ref field, fileBytes, blockDataOffset, currentBlockTiffHeaderOffset, e, lastBlockByteFileOffset);
                result.Add(field);
            }

            return result;
        }
        internal int LoadFieldTextValue<T>(ref IFDField field, List<byte> fileBytes, int fieldDataOffset, int tiffHeaderOffset, Endianness e, int lastBlockByteFileOffset)
        {
            int result = lastBlockByteFileOffset;
            byte[] valueBytes;

            TagElementTypeMappingDictionary typeDictionary = TagElementTypeMappingDictionaryFactory.GetMappingDictionary(typeof(T));

            if (field.IsOffset)
            {
                long fixedOffset = 0;
                if (field.Block == IFDHeaderType.ExifIFD)
                {
                    fixedOffset = tiffHeaderOffset + (long)field.ValueElementsOffset;
                }
                else
                {
                    fixedOffset = fieldDataOffset + (long)field.ValueElementsOffset - 8;
                }

                valueBytes = ByteManager.ChopByteArray(fileBytes.ToArray(), fixedOffset, (int)field.ValueElementsCount * typeDictionary[field.ValueElementType].BytesLength);

                if (lastBlockByteFileOffset < fieldDataOffset + (long)field.ValueElementsOffset - 8 + (int)field.ValueElementsCount * typeDictionary[field.ValueElementType].BytesLength)
                {
                    result = fieldDataOffset + field.ValueElementsOffset - 8 + field.ValueElementsCount * typeDictionary[field.ValueElementType].BytesLength;
                }
            }
            else
            {
                valueBytes = field.FieldValueElements;
            }

            TagCodeEnumDictionary enumsDictiornary = FieldCodeEnumDictionaryFactory.GetFieldCodeEnumDictionary<T>();

            Type enumType = null;
            if (enumsDictiornary.ContainsKey(field.FieldCode))
            {
                enumType = enumsDictiornary[field.FieldCode];
            }

            field.Text = GetIFDTextValue<T>(enumType, field.ValueElementType, valueBytes);

            return result;
        }
        internal byte[][] ExtractTagsBytes(List<byte> bytes, int tagsNumber)
        {
            byte[][] result = new byte[tagsNumber][];
            byte[] tagBytes;

            for (int i = 0; i < tagsNumber; ++i)
            {
                tagBytes = new byte[12];
                bytes.CopyTo(i * 12, tagBytes, 0, 12);
                result[i] = tagBytes;
            }

            return result;
        }
        internal IFDField GetIFDField<T>(byte[] fieldBytes, Endianness e)
        {
            IFDField result = new IFDField();

            byte[] sortedFieldsBytes = SortIFDFieldBytes(e, fieldBytes);

            result.FieldCode = ByteManager.GetUIntValue(ByteManager.ChopByteArray(sortedFieldsBytes, 0, 2));
            LoadFieldName<T>(ref result);
            result.ValueElementType = ByteManager.GetUIntValue(ByteManager.ChopByteArray(sortedFieldsBytes, 2, 2));
            result.ValueElementsCount = ByteManager.GetIntValue(ByteManager.ChopByteArray(sortedFieldsBytes, 4, 4));
            result.FieldValueElements = ByteManager.ChopByteArray(sortedFieldsBytes, 8, 4);
            result.ValueElementsOffset = 0;

            TagElementTypeMappingDictionary typesDictionary = TagElementTypeMappingDictionaryFactory.GetMappingDictionary(typeof(T));

            if (result.FieldCode > 0 && typesDictionary.ContainsKey(result.ValueElementType))
            {
                result.IsOffset = (result.ValueElementsCount * typesDictionary[result.ValueElementType].BytesLength > 4);
                result.FieldValueElements = ByteManager.ChopByteArray(sortedFieldsBytes, 8, 4);
            }

            if (result.IsOffset)
            {
                result.ValueElementsOffset = ByteManager.GetIntValue(result.FieldValueElements);
            }

            return result;
        }
        private void LoadFieldName<T>(ref IFDField result)
        {
            if (result == null)
            {
                return;
            }

            result.FieldName = Enum.GetName(typeof(T), result.FieldCode);
        }
        internal string GetIFDTextValue<T>(Type enumType, uint code, byte[] b)
        {
            object value = null;
            string result = string.Empty;

            TagElementTypeMappingDictionary typeDictionary = TagElementTypeMappingDictionaryFactory.GetMappingDictionary(typeof(T));
            if (typeDictionary.ContainsKey(code))
            {
                switch (typeDictionary[code].DestinationType.ToString())
                {
                    case "System.String":
                    case "System.Byte":
                        value = ByteManager.GetValue<string>(b);
                        break;
                    case "System.UInt16":
                    case "System.UInt32":
                        value = ByteManager.GetValue<uint>(b);
                        break;
                    case "System.Int16":
                    case "System.Int32":
                        value = ByteManager.GetValue<int>(b);
                        break;
                    case "System.ULong":
                        value = ByteManager.GetValue<ulong>(b);
                        break;
                    case "System.Long":
                        value = ByteManager.GetValue<long>(b);
                        break;
                    case "BAFactory.Fx.Utilities.Encoding.Rational":
                        Rational rat = ByteManager.GetValue<Rational>(b);
                        value = string.Format("{0:F} ({1}/{2})", rat.Value, rat.Numerator, rat.Denominator);
                        break;
                }
            }


            if (value != null)
            {
                if (enumType != null)
                {
                    result = Enum.ToObject(enumType, value).ToString();
                }
                else
                {
                    result = value.ToString();
                }
            }
            return result;
        }
        internal IFDTagElementType GetTagValueType(byte[] array)
        {
            return (IFDTagElementType)BitConverter.ToInt16(ByteManager.ChopByteArray(array, 0, 2), 0);
        }
    }
}
