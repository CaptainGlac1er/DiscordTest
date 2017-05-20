﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Threading;
using gwcImgurConnect;
using System.Collections;
using System.Net;
using gwcFileSystem;
using gwcDiscordConnect;
using System.IO;

namespace DiscordTest
{
    class Images : Module
    {
        private Dictionary<string, Queue> queuesRunning;
        Random random = new Random();
        private IList<Admin> admins;
        public Images(FileSystemFile settings, IList<Admin> admins)
        {
            this.admins = admins;
            command = "pics";
            //gets token and starts an Imgur connection
            imgur = new ImgurAPI(settings);
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            queuesRunning = new Dictionary<string, Queue>();
            methods.Add("search", async (command) => {
                List<picture> pics = imgur.querySearch(command.GetArg(1));
                await command.Channel.SendMessage(command.User.Name + " searched for " + command.GetArg(1) + " has " + pics.Count + " results");
                if (pics.Count > 0)
                {
                    string link = pics[(new Random()).Next(pics.Count)].link;
                    await command.Channel.SendMessage(link);
                    await command.Message.Delete();
                }
                else
                {
                    await command.Message.Delete();
                }

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
                     output.Append(s + " every " + queuesRunning[s].getDelay() + "\r\n");
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
                    queuesRunning.Add(query, new ImgurQueue(query, new TimeSpan(0, delay, 0), false, command, imgur));



                    queuesRunning[query].Start();
                }
                else{
                    await command.Channel.SendMessage(query + " queue has already been started");
                }
            });
            methods.Add("stopqueue", async (command) =>
            {
                string query = command.GetArg(1);
                await command.Message.Delete();
                if (queuesRunning.ContainsKey(query))
                {
                    queuesRunning[query].Stop();
                    queuesRunning.Remove(query);
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
                        queuesRunning[entry].Stop();
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
        ImgurAPI imgur;
        

        public override string getHelp()
        {
            string help = "!pics commands:\n" +
                "search <query>: query for a random picture with the title of <query>\n" +
                "queue <query> <time delay>: Create a queue that displays pictures with title <query> that fires every <time delay> minutes.\n" +
                "stopqueue: stops all the picture queues\n";
            return help;
        }

        public override void error(Exception e, CommandEventArgs command)
        {
            foreach(Admin a in admins)
                if (a.id == command.User.Id)
                {
                    command.User.SendMessage(e.GetType().ToString());
                    command.User.SendMessage(e.StackTrace);
                }
        }
    }
}
