using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gwcFileSystem
{
    public class FileSystemFile
    {
        FileInfo file;
        public FileSystemFile(FileInfo file)
        {
            this.file = file;
        }
        public string getFileContents()
        {
            if (file == null)
                return null;
            using(StreamReader fileReader = new StreamReader(file.FullName))
            {
                return fileReader.ReadToEnd();
            }
        }
        public async void writeFileContents(string toWrite)
        {
            Console.WriteLine(toWrite);
            using( StreamWriter fileWriter = new StreamWriter(file.FullName))
            {
                foreach (char c in toWrite)
                    await fileWriter.WriteAsync(c);
            }
        }
        public static FileSystemFile[] ConvertArray(FileInfo[] files)
        {
            FileSystemFile[] output = new FileSystemFile[files.Length];
            int i = 0;
            foreach (FileInfo file in files)
                output[i++] = new FileSystemFile(file);
            return output;
        }
        public StreamWriter getStreamWriter()
        {
            return new StreamWriter(file.FullName);
        }
        public void writeObject(object toWrite)
        {
            Thread update = new Thread(new ThreadStart(() =>
            {
                using (StreamWriter fileWrite = getStreamWriter()) using (JsonTextWriter jsonWriter = new JsonTextWriter(fileWrite))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    (new JsonSerializer()).Serialize(jsonWriter, toWrite);
                }
            }));
            update.Start();
        }

    }
}
