using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gwcFileSystem
{
    public class FileSystem
    {
        private DirectoryInfo filesystem;
        public FileSystem(String dir)
        {
            if (Directory.Exists(dir))
                filesystem = new DirectoryInfo(dir);
        }
        public FileInfo getFile(String filename)
        {
            FileInfo[] files = filesystem.GetFiles(filename);

            if (files.Length == 1)
            {
                return files[0];
            }
            else
            {
                return null;
            }
        }
        public FileInfo[] getFiles()
        {
            return filesystem.GetFiles();
        }
    }
}
