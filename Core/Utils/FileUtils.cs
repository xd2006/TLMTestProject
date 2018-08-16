using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class FileUtils
    {
        public static void Deletefile(string path)
        {
            File.Delete(path);
        }

        public static long GetFilesLenght(string path)
        {
            return new FileInfo(path).Length;
        }
    }
}
