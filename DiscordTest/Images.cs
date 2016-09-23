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
        private Dictionary<string, Func<CommandEventArgs, Task>> methods;
        public Images()
        {
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            queuesRunning = new List<bool>();
            methods.Add("search", async (command) => {
                List<DataType.picture> pics = imgur.querySearch(command.GetArg("arg2"));
                string link = pics[(new Random()).Next(pics.Count)].link;
                await command.Channel.SendMessage(link);
                return;
            });
            methods.Add("queue", async (command) => {
                /*Timer tm = new Timer((Object state) =>
                {
                */
                int delay;
                if(command.GetArg("arg3") == null || !Int32.TryParse(command.GetArg("arg3"), out delay))
                {
                    await command.Channel.SendMessage("!meme queue <search> <delay in mins>");
                    return;
                }
                await command.Channel.SendMessage(command.GetArg("arg2") + "queue has been started");
                int queue = queuesRunning.Count;
                queuesRunning.Add(true);
                while (queuesRunning[queue])
                {
                    List<DataType.picture> pics = imgur.querySearch(command.GetArg("arg2"));
                    string link = pics[(new Random()).Next(pics.Count)].link;
                    await command.Channel.SendMessage(link);
                    await Task.Delay(new TimeSpan(0, delay, 0));
                }
                /*}, new AutoResetEvent(true), Int32.Parse(command.GetArg("arg3")) * 60 * 1000, 30 * 60 * 1000);*/
            });
            methods.Add("queuestop", async (command) =>
            {
                for (int i = 0; i < queuesRunning.Count; i++)
                {
                    queuesRunning[i] = false;
                }
                await command.Channel.SendMessage("All Image queues stopped");
            });
        }
        ImgurAPI imgur = new ImgurAPI();
        public Dictionary<string, Func<CommandEventArgs, Task>> Methods
        {
            get
            {
                return methods;
            }

            set
            {
                methods = value;
            }
        }

        public string ModuleName
        {
            get
            {
                return "images";
            }

            set
            {
                
            }
        }

        public async void runCommand(CommandEventArgs command)
        {
            await methods[command.GetArg("arg1")](command);
        }
    }
}
