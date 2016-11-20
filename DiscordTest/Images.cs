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
        private Dictionary<string, bool> queuesRunning;
        bool queueLocked = false;
        public Images()
        {
            imgur = new APIs.ImgurAPI();
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            queuesRunning = new Dictionary<string, bool>();
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
                string query = command.GetArg(1);
                if (command.GetArg(2) == null || !Int32.TryParse(command.GetArg(2), out delay))
                {
                    await command.Channel.SendMessage("!pics queue <search> <delay in mins>");
                    return;
                }
                await command.Message.Delete();
                if (!queuesRunning.ContainsKey(query))
                {
                    await command.Channel.SendMessage(query + " queue has been started");
                    queuesRunning.Add(query, true);
                    Thread myThread = new Thread(() =>
                    {
                        while (queuesRunning[query])
                        {
                            List<DataType.picture> pics = imgur.querySearch(command.GetArg(1));
                            string link = pics[(new Random()).Next(pics.Count)].link;
                            command.Channel.SendMessage(link);
                            Thread.Sleep(new TimeSpan(0, delay, 0));
                            while (queueLocked)
                                Thread.Sleep(1);
                        }
                        queuesRunning.Remove(query);
                    });
                    myThread.Start();
                }else{
                    await command.Channel.SendMessage(query + " queue has already been started");
                }
            });
            methods.Add("stopqueue", async (command) =>
            {
                string query = command.GetArg(1);
                await command.Message.Delete();
                if (queuesRunning.ContainsKey(query)){
                    queuesRunning[query] = false;
                    await command.Channel.SendMessage(query + " queue stopped");
                }else{
                    await command.Channel.SendMessage(query + " queue not found");
                }
            });
            methods.Add("stopqueues", async (command) =>
            {
                queueLocked = true;
                for(int i = 0; i < queuesRunning.Keys.Count; i++)
                {
                    string entry = queuesRunning.Keys.ToList()[i];
                    await command.Channel.SendMessage(entry + " queue stopped");
                    if (queuesRunning.ContainsKey(entry))
                        queuesRunning[entry] = false;
                    else
                        await command.Channel.SendMessage("huh where did it go? " + entry);
                }
                queueLocked = false;
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
