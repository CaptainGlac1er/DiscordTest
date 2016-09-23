using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordTest
{
    class Weather : Module
    {
        private Dictionary<string, Func<CommandEventArgs, Task>> methods;
        APIs.owmAPI owm;
        public Weather()
        {
            owm = new APIs.owmAPI();
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            methods.Add("get", async (command) =>
            {
                DataType.weatherToday curWeather = owm.querySearch(command.GetArg(1));
                await command.Channel.SendMessage(curWeather.name + " is " + ((curWeather.main.temp - 273.15) * 1.8 + 32) + " F");
            });
        }
        public Dictionary<string, Func<CommandEventArgs, Task>> Methods
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string ModuleName
        {
            get
            {
                return "Weather";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public async void runCommand(CommandEventArgs command)
        {
            if (!methods.ContainsKey(command.GetArg(0)))
                await command.Channel.SendMessage(command.GetArg(0) + " is not an command");
            else 
                await methods[command.GetArg(0)](command);
        }
    }
}
