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
        public IList<Admin> admins { get; set; }
        public Dictionary<ulong, IList<string>> getChannelsCommands()
        {
            Dictionary<ulong, IList<string>> channelCommands = new Dictionary<ulong, IList<string>>();
            foreach (Channel channel in channels)
                channelCommands.Add(channel.token, channel.getCommands());
            return channelCommands;
        }
    }
}
