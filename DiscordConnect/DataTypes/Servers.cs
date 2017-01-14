using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordConnect
{
    public class Servers
    {
        public Servers()
        {
            servers = new List<Server>();
        }
        public IList<Server> servers { get; set; }
    }
}
