using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files.Manager
{
    public class FileManager
    {
        static void ResteFilesTimestamps(string path) { 
        
        }
        static void ResteFilesTimestamps(string path, DateTime date)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            DirectoryInfo currentDir = new DirectoryInfo(@"D:\Network Shared Folder\Deliver - Copy");
            ResetDirectory(currentDir, date);
        }
        static void ResetDirectory(DirectoryInfo dir, DateTime date)
        {
            var directories = dir.GetDirectories();
            foreach (var d in directories)
            {
                ResetDirectory(d, date);
            }
            ResetFiles(dir.GetFiles(), date);
        }
        static void ResetFiles(FileInfo[] files, DateTime date)
        {
            foreach (var file in files)
            {
                file.CreationTime = date;
                file.CreationTimeUtc = date;
                file.LastAccessTime = date;
                file.LastAccessTimeUtc = date;
                file.LastWriteTime = date;
                file.LastWriteTimeUtc = date;
            }
        }

    }
}
