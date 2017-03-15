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
        protected string command;
        protected Dictionary<string, Func<CommandEventArgs, Task>> methods;
        protected bool conversational = false;
        public async void runCommand(CommandEventArgs command)
        {
            if (command.Args.Length == 0)
            {
                await command.Channel.SendMessage(command.Message.User.NicknameMention + " needs a argument for that command");
            }
            else if (!methods.ContainsKey(command.GetArg(0)))
            {
                if (command.Args.Length == 1 && conversational)
                {
                    await methods[""](command);
                }
                else
                {
                    await command.Channel.SendMessage(command.GetArg(0) + " is not an command");
                }
            }
            else
            {
                try
                {
                    await methods[command.GetArg(0)](command);
                }catch(Exception e)
                {
                    error(e, command);
                }
            }
        }
        public String getCommand()
        {
            return command;
        }
        public virtual void error(Exception e, CommandEventArgs command)
        {
            command.User.SendMessage(e.Message);
        }
    }
}
