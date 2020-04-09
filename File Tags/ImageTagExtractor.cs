using System.Collections.Generic;
using System.Linq;
using BAFactory.Fx.FileTags.Exif;
using BAFactory.Fx.FileTags.Exif.IFD;
using System.Threading.Tasks;

namespace BAFactory.Fx.Utilities.ImageProcessing
{
    public class ImageTagExtractor : BaseFileTagExtractor
    {
        private ExifFileProcessor exifProcessor { get; set; }
        internal List<IFDField> Fields { get; set; }
        public ImageTagExtractor(string filePath)
            : base(filePath)
        {
            Fields = new List<IFDField>();
            exifProcessor = new ExifFileProcessor();
        }
        override protected internal async Task ProcessTagsAsync()
        {
            await Task.Run(() =>
            {
                ProcessingFile.App1HeaderOffset = exifProcessor.LocateApp1FileMarkIndex(ProcessingFile.FileBytes);

                if (ProcessingFile.App1HeaderOffset < 0)
                {
                    return;
                }

                ProcessingFile.App1 = exifProcessor.ExtractApp1Header(ProcessingFile.FileBytes, ProcessingFile.App1HeaderOffset);

                ProcessingFile.App1.TiffHeader = exifProcessor.GetTiffHeader(ProcessingFile.FileBytes, ProcessingFile.TiffHeaderOffset);

                ProcessingFile.App1.ZerothHeader = exifProcessor.GetIFDHeader<IFDTagCode>(IFDHeaderType.ZerothIFD, ProcessingFile.FileBytes, ProcessingFile.ZerothHeaderOffset, ProcessingFile.TiffHeaderOffset, ProcessingFile.App1.TiffHeader.ByteOrder);

                ProcessingFile.App1.ExifHeader = exifProcessor.GetIFDHeader<IFDTagCode>(IFDHeaderType.ExifIFD, ProcessingFile.FileBytes, ProcessingFile.ExifHeaderOffset, ProcessingFile.TiffHeaderOffset, ProcessingFile.App1.TiffHeader.ByteOrder);

                ProcessingFile.App1.InteropHeader = exifProcessor.GetIFDHeader<IFDTagCode>(IFDHeaderType.InteroperatibilityIFD, ProcessingFile.FileBytes, ProcessingFile.InteropHeaderOffset, ProcessingFile.TiffHeaderOffset, ProcessingFile.App1.TiffHeader.ByteOrder);

                ProcessingFile.App1.FirstHeader = exifProcessor.GetIFDHeader<IFDTagCode>(IFDHeaderType.FirstIFD, ProcessingFile.FileBytes, ProcessingFile.FirstHeaderOffset, ProcessingFile.TiffHeaderOffset, ProcessingFile.App1.TiffHeader.ByteOrder);

                IFDHeader exifHeaderRef = ProcessingFile.App1.ExifHeader;
                IFDField makeField = exifProcessor.FindField(ProcessingFile.App1.ZerothHeader.IFDFields, (uint)IFDTagCode.Make);
                if (makeField != null)
                {
                    bool processed = exifProcessor.LoadMakerNotesHeader(ref exifHeaderRef, makeField, ProcessingFile.FileBytes, ProcessingFile.App1.TiffHeader.ByteOrder, ProcessingFile);
                    if (processed)
                    {
                        ProcessingFile.App1.ExifHeader.RemoveMarkerNotesIFD();
                    }
                }

                Fields = new List<IFDField>();
                Fields.AddRange(ProcessingFile.App1.ZerothHeader.IFDFields);
                Fields.AddRange(ProcessingFile.App1.ExifHeader.IFDFields);

                if (ProcessingFile.App1.InteropHeader != null)
                    Fields.AddRange(ProcessingFile.App1.InteropHeader.IFDFields);

                if (ProcessingFile.App1.FirstHeader != null)
                    Fields.AddRange(ProcessingFile.App1.FirstHeader.IFDFields);

                if (ProcessingFile.App1.ExifHeader.MakerHeader != null)
                {
                    Fields.AddRange(ProcessingFile.App1.ExifHeader.MakerHeader.IFDFields);
                }
            });
        }
        public IFDField GetTiffTag(string filePath, int tagCode)
        {
            IFDField result = null;

            var field = from f in Fields
                        where f.FieldCode == tagCode
                        select f;
            if (field.Count() == 1)
            {
                result = field.First();
            }

            return result;
        }
        internal List<IFDField> GetAllTiffTags(string filePath)
        {
            return Fields;
        }
    }
}
