using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure
{
    public class FileHelper
    {
        public static async Task CreateFile(Stream stream, string name)
        {
            using (var source = File.Create(name))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(source);
            }
        }

        public static byte[] GetBytesFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File doesn't exist");
            }

            return File.ReadAllBytes(path);
        }

        //public static 
    }
}
