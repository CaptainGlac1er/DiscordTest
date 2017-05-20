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
        public FileSystem(DirectoryInfo dir)
        {
            filesystem = dir;
        }
        public DirectoryInfo getDirectory(String dirname)
        {
            DirectoryInfo[] dirs = filesystem.GetDirectories(dirname);
            if (dirs.Length == 1)
            {
                return dirs[0];
            }
            else
            {
                return null;
            }
        }
        public FileSystemFile getFile(String filename)
        {
            FileInfo[] files = filesystem.GetFiles(filename);
            if (files.Length == 1)
            {
                return new FileSystemFile(files[0]);
            }
            else
            {
                return new FileSystemFile(null);
            }
        }
        public FileSystemFile[] getFiles()
        {
            return FileSystemFile.ConvertArray(filesystem.GetFiles());
        }
        public DirectoryInfo[] getDirectories()
        {
            return filesystem.GetDirectories();
        }
    }
}
