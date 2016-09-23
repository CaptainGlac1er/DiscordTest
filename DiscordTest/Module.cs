using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordTest
{
    abstract class Module
    {

        public abstract string getHelp();
        protected Dictionary<string, Func<CommandEventArgs, Task>> methods;
        public abstract void runCommand(CommandEventArgs command);
    }
}
