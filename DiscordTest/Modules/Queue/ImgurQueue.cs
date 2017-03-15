using Discord.Commands;
using gwcImgurConnect;
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
        private static Stack<string> previouslySeenImgSrc = new Stack<string>();
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
                while (running)
                {
                    lock (previouslySeenImgSrc)
                    {
                        List<picture> pics = imgur.querySearch(command.GetArg(1));
                        bool posted = false;
                        if (!allowDuplicates)
                        {
                            foreach (picture pic in pics)
                            {
                                if (!previouslySeenImgSrc.Contains(pic.link)) { 
                                    command.Channel.SendMessage(pic.link);
                                    previouslySeenImgSrc.Push(pic.link);
                                    if (previouslySeenImgSrc.Count > 30)
                                        previouslySeenImgSrc.Pop();
                                    posted = true;
                                    break;
                                }
                            }
                        }else if(pics.Count > 0)
                        {
                            command.Channel.SendMessage(pics[random.Next(pics.Count - 1)].link);
                            posted = true;
                        }
                        if (!posted)
                            command.Channel.SendMessage(query + " queue doesnt have new pic");
                    }
                    Thread.Sleep(delay);
                }
            });
        }
    }
}
