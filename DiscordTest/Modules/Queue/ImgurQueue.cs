using Discord.Commands;
using ImgurConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordTest
{
    class ImgurQueue : Queue
    {
        private string query;
        private bool allowDuplicates;
        private static Stack<String> previouslySeenImgSrc = new Stack<string>();
        CommandEventArgs command;
        ImgurAPI imgur;
        public ImgurQueue(string query, TimeSpan delay, bool allowDuplicates, CommandEventArgs command, ImgurAPI imgur) : base()
        {
            this.name = "Image queue for " + query;
            this.query = query;
            this.delay = delay;
            this.allowDuplicates = allowDuplicates;
            this.imgur = imgur;
            this.command = command;
            create();
        }
        private void create()
        {
            queueThread = new Thread(() =>
            {
                int tries = 5;
                while (running)
                {
                    lock (previouslySeenImgSrc)
                    {
                        List<picture> pics = imgur.querySearch(command.GetArg(1));
                        string link = pics[random.Next(pics.Count)].link;
                        if (!allowDuplicates && !previouslySeenImgSrc.Contains(link))
                        {
                            tries = 5;
                            previouslySeenImgSrc.Push(link);
                            if (previouslySeenImgSrc.Count > 30)
                                previouslySeenImgSrc.Pop();
                            command.Channel.SendMessage(link);
                        }
                        else
                        {
                            if (!allowDuplicates &&  tries-- > 0)
                                continue;
                            else
                                command.Channel.SendMessage(query + " queue doesnt have new pic");

                        }
                    }
                    Thread.Sleep(delay);
                }
            });
        }
    }
}
