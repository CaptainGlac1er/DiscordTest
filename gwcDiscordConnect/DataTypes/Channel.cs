using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gwcDiscordConnect
{
    public class Channel
    {
        public Channel()
        {
            commands = new List<Command>();
        }

        public IList<Command> commands { get; set; }
        public string name { get; set; }
        public ulong token { get; set; }
        public IList<string> getCommands()
        {
            IList<string> channelCommands = new List<string>();
            foreach (Command c in commands)
                channelCommands.Add(c.command);
            return channelCommands;
        }
    }
}
