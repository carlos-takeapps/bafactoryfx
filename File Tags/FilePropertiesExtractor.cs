using System.Collections.Generic;
using BAFactory.Fx.FileTags.Exif.IFD;
using BAFactory.Fx.FileTags.OutputTypes;
using BAFactory.Fx.Utilities.ImageProcessing;
using System.Threading.Tasks;

namespace BAFactory.Fx.FileTags
{
    public class FilePropertiesExtractor
    {
        private string Path { get; set; }
        public FilePropertiesExtractor(string path)
        {
            Path = path;
        }
        public List<FileProperty> GetAllFileProperties()
        {
            return Task.Run(() => GetAllFilePropertiesAsync()).Result;
        }
        public async Task<FilePropertiesList> GetAllFilePropertiesAsync()
        {
            return await GetFilePropertiesAsync(FilePropertySource.All);
        }
        public FilePropertiesList GetFileProperties(FilePropertySource t)
        {
            return GetFilePropertiesAsync(t).Result;
        }
        public async Task<FilePropertiesList> GetFilePropertiesAsync(FilePropertySource t)
        {
            FilePropertiesList result = new FilePropertiesList();

            if ((t & FilePropertySource.All) == FilePropertySource.All
                || (t & FilePropertySource.Exif) == FilePropertySource.Exif)
            {
                ImageTagExtractor imageTagsExtractor = new ImageTagExtractor(Path);
                await imageTagsExtractor.ProcessTagsAsync();
                Parallel.ForEach(imageTagsExtractor.Fields, field =>
                {
                    result.Add((FileProperty)field);
                });
            }

            return result;
        }
    }
}
