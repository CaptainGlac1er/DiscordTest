using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordTest
{
    class WeatherQueue : Queue
    {
        private string query;
        private bool allowDuplicates;
        private static Stack<String> previouslySeenImgSrc = new Stack<string>();
        CommandEventArgs command;
        APIs.owmAPI weather;
        public WeatherQueue(string query, TimeSpan delay, bool allowDuplicates, CommandEventArgs command, APIs.owmAPI weather) : base()
        {
            this.name = "Weather queue for " + query;
            this.query = query;
            this.delay = delay;
            this.allowDuplicates = allowDuplicates;
            this.weather = weather;
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
                        DataType.weatherToday currentWeather = weather.querySearch(command.GetArg(1));
                        command.Channel.SendMessage(currentWeather.name + " is " + APIs.owmAPI.convertToFar(currentWeather.main.temp) + " F");

                    }
                    Thread.Sleep(delay);
                }
            });
        }
    }
}
