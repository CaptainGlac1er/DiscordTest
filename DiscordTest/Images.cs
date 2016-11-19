using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Threading;

namespace DiscordTest
{
    class Images : Module
    {
        private List<bool> queuesRunning;
        public Images()
        {
            imgur = new APIs.ImgurAPI();
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            queuesRunning = new List<bool>();
            methods.Add("search", async (command) => {
                List<DataType.picture> pics = imgur.querySearch(command.GetArg(1));
                await command.Channel.SendMessage(command.User.Name + " searched for " + command.GetArg(1));
                string link = pics[(new Random()).Next(pics.Count)].link;
                await command.Channel.SendMessage(link);
                await command.Message.Delete();
                return;
            });
            methods.Add("queue", async (command) => {
                int delay;
                if(command.GetArg(2) == null || !Int32.TryParse(command.GetArg(2), out delay))
                {
                    await command.Channel.SendMessage("!pics queue <search> <delay in mins>");
                    return;
                }
                await command.Channel.SendMessage(command.GetArg(1) + " queue has been started");
                int queue = queuesRunning.Count;
                queuesRunning.Add(true);
                await command.Message.Delete();
                while (queuesRunning[queue])
                {
                    List<DataType.picture> pics = imgur.querySearch(command.GetArg(1));
                    string link = pics[(new Random()).Next(pics.Count)].link;
                    await command.Channel.SendMessage(link);
                    await Task.Delay(new TimeSpan(0, delay, 0));
                }
                /*}, new AutoResetEvent(true), Int32.Parse(command.GetArg("arg3")) * 60 * 1000, 30 * 60 * 1000);*/
            });
            methods.Add("stopqueues", async (command) =>
            {
                for (int i = 0; i < queuesRunning.Count; i++)
                {
                    queuesRunning[i] = false;
                }
                await command.Message.Delete();
                await command.Channel.SendMessage("All Image queues stopped");
            });
            methods.Add("help", async (command) =>
            {
                await command.Channel.SendMessage(getHelp());
                await command.Message.Delete();
            });
        }
        APIs.ImgurAPI imgur;

        public override async void runCommand(CommandEventArgs command)
        {
            if (!methods.ContainsKey(command.GetArg(0)))
            {
                await command.Channel.SendMessage(command.GetArg(0) + " is not an command");
            }
            else
                await methods[command.GetArg(0)](command);
        }

        public override string getHelp()
        {
            string help = "!pics commands:\n" +
                "search <query>: query for a random picture with the title of <query>\n" +
                "queue <query> <time delay>: Create a queue that displays pictures with title <query> that fires every <time delay> minutes.\n" +
                "stopqueue: stops all the picture queues\n";
            return help;
        }
    }
}
