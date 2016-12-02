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
        bool stackLocked = false;
        Random random = new Random();
        public Images()
        {
            imgur = new APIs.ImgurAPI();
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            queuesRunning = new Dictionary<string, bool>();
            Stack<String> previouslySeenImgur = new Stack<string>();
            methods.Add("search", async (command) => {
                List<DataType.picture> pics = imgur.querySearch(command.GetArg(1));
                await command.Channel.SendMessage(command.User.Name + " searched for " + command.GetArg(1));
                string link = pics[(new Random()).Next(pics.Count)].link;
                await command.Channel.SendMessage(link);
                await command.Message.Delete();
                return;
            });
            methods.Add("queues", async (command) =>
             {
                 await command.Message.Delete();
                 await command.Channel.SendMessage(command.Message.User.NicknameMention + " current queues playing are");
                 StringBuilder output = new StringBuilder("```\r\n");
                 if (queuesRunning.Keys.Count == 0)
                     output.Append("No queues running");
                 foreach(string s in queuesRunning.Keys.ToArray())
                 {
                     output.Append(s + "\r\n");
                 }
                 output.Append("```");
                 await command.Channel.SendMessage(output.ToString());
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
                        int tries = 5;
                        bool keepRunning = true;
                        lock (queuesRunning) {
                            keepRunning = queuesRunning[query];
                        }
                        while (keepRunning)
                        {
                            lock (previouslySeenImgur)
                            {
                                List<DataType.picture> pics = imgur.querySearch(command.GetArg(1));
                                string link = pics[random.Next(pics.Count)].link;
                                if (!previouslySeenImgur.Contains(link))
                                {
                                    tries = 5;
                                    previouslySeenImgur.Push(link);
                                    if (previouslySeenImgur.Count > 30)
                                        previouslySeenImgur.Pop();
                                    command.Channel.SendMessage(link);
                                }else
                                {
                                    if (tries-- > 0)
                                        continue;
                                    else
                                        command.Channel.SendMessage(query + " queue doesnt have new pic");

                                }
                            }
                            Thread.Sleep(new TimeSpan(0, delay, 0));
                            lock (queuesRunning)
                            {
                                if (!queuesRunning.ContainsKey(query))
                                    keepRunning = false;
                                else
                                    keepRunning = queuesRunning[query];
                            }
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
                if (queuesRunning.ContainsKey(query))
                {
                    lock (queuesRunning)
                    {
                        queuesRunning[query] = false;
                    }
                    await command.Channel.SendMessage(query + " queue stopped");
                }else{
                    await command.Channel.SendMessage(query + " queue not found");
                }
            });
            methods.Add("stopqueues", async (command) =>
        {
            lock (queuesRunning)
            {
                foreach (string entry in queuesRunning.Keys.ToArray())
                {
                    command.Channel.SendMessage(entry + " queue stopped");
                    if (queuesRunning.ContainsKey(entry))
                    {
                        queuesRunning.Remove(entry);
                    }
                    else
                        command.Channel.SendMessage("huh where did it go? " + entry);
                }
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
