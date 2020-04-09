using System;
using System.Collections.Generic;
using System.IO;
using BAFactory.Fx.FileTags.Common;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.Utilities.Encoding;
using System.Threading.Tasks;

namespace BAFactory.Fx.Utilities.ImageProcessing
{
    public abstract class BaseFileTagExtractor
    {
        protected internal FileInfo fileInfo;
        protected internal ImageFile ProcessingFile { get; set; }
        public BaseFileTagExtractor(string filePath)
        {
            ProcessingFile = new ImageFile(filePath);
            ProcessingFile.LoadFileBeginingAsync().Wait();
        }
        abstract protected internal Task ProcessTagsAsync();
    }
}
