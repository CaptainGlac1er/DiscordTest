using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using gwcCleverbotConnect;
using gwcFileSystem;
using System.IO;

namespace DiscordTest
{
    class Cleverbot : Module
    {
        private CleverbotAPI cleverbot;
        public Cleverbot(FileInfo file)
        {
            cleverbot = new CleverbotAPI(file);// (System.Configuration.ConfigurationManager.ConnectionStrings["cleverbot"].ToString());
            command = "chat";
            conversational = true;
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            methods.Add("", async (command) =>
            {
                await command.Channel.SendMessage(cleverbot.getReply(command.GetArg(0)));
            });

        }
        public override string getHelp()
        {
            string help = "!chat commands:\n" +
                   "!chat <query>: speak to cleverbot\n";
            return help;
        }
    }
}
