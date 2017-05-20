using gwcFileSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gwcDiscordConnect
{
    public class DiscordConnectInfo
    {
        private Servers servers;
        public DiscordConnectInfo(FileSystemFile file)
        {
            servers = new Servers();
            servers = JsonConvert.DeserializeObject<Servers>(file.getFileContents());

        }
        public Servers getServers()
        {
            return servers;
        }
        
    }
}
