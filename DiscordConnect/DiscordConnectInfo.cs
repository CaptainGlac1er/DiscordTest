using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordConnect
{
    public class DiscordConnectInfo
    {
        private Servers servers;
        public DiscordConnectInfo(FileInfo file)
        {
            servers = new Servers();
            Console.WriteLine(file.FullName);
            using(StreamReader reader = new StreamReader(file.FullName))
            {
                string json = reader.ReadToEnd();
                servers = JsonConvert.DeserializeObject<Servers>(json);
            }

        }
        public Servers getServers()
        {
            return servers;
        }
        
    }
}
