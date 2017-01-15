using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gwcDiscordConnect
{
    public class Server
    {
        public Server()
        {
            channels = new List<Channel>();
        }
        public string name { get; set; }
        public string token { get; set; }
        public IList<Channel> channels { get; set; }
    }
}
