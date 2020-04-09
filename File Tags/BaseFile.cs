using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace BAFactory.Fx.Utilities.ImageProcessing
{
    public class BaseFile
    {
        private const int twoMB = 20480; 

        internal FileInfo SourceFile;
        internal List<byte> FileBytes { get; set; }
        internal BaseFile(string filePath)
        {
            SourceFile = new FileInfo(filePath);
            FileBytes = new List<byte>();
        }
        internal async Task LoadFileBeginingAsync()
        {
            await LoadBytesAsync(0, twoMB);
        }
        internal async Task LoadCompleteFileAsync()
        {
            await LoadBytesAsync(0, SourceFile.Length);
        }
        protected internal async Task LoadBytesAsync(int startIndex, long bytes)
        {
            byte[] buffer = new byte[bytes];
            int read = 0;
            using (FileStream fs = SourceFile.Open(FileMode.Open))
            {
                read = await fs.ReadAsync(buffer, startIndex, buffer.Length);
            }

            for (int i = 0; i < read; ++i)
            {
                FileBytes.Add(buffer[i]);
            }
        }
    }
}
