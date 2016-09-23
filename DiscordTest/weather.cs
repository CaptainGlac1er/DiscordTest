using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordTest
{
    class Weather : Module
    {
        public Dictionary<string, Func<CommandEventArgs, Task>> Methods
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string ModuleName
        {
            get
            {
                return "Weather";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void runCommand(CommandEventArgs command)
        {
            throw new NotImplementedException();
        }
    }
}
