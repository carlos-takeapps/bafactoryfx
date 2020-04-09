using System;
using BAFactory.Fx.FileTags;
using BAFactory.Fx.FileTags.OutputTypes;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tiff_Tag_Extraction_Test_Tool
{
    class Program
    {
        static string headLine = "============================================================";
        static void Main(string[] args)
        {
            //string file = @"D:\TEMP\canon-ixus.JPG";
            //string file = @"D:\TEMP\DSCN3462.JPG";
            string file = @"D:\TEMP\FB_20140415_21_00_23_Saved_Picture.jpg";

            FilePropertiesExtractor extractor = new FilePropertiesExtractor(file);

            Console.WriteLine(headLine);
            Console.WriteLine("Image Tags:");
            Console.WriteLine(headLine);

            List<FileProperty> properties = extractor.GetAllFilePropertiesAsync().Result;


            foreach (FileProperty tag in properties)
            {
                Console.WriteLine("{0,-50}{1}", string.Format("{0}-{1}-{2}:",tag.FieldGroup, tag.FieldSource, tag.Name), tag.Value);
            }
            Console.ReadLine();
        }
    }
}
