using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordTest
{
    interface Module
    {
        string ModuleName { get; set; }
        Dictionary<string, Func<CommandEventArgs, Task>> Methods {
            get;
            set;
        }
        void runCommand(CommandEventArgs command);
    }
}
