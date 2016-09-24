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
        APIs.owmAPI owm;
        public Weather()
        {
            owm = new APIs.owmAPI();
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            methods.Add("get", async (command) =>
            {
                if(command.GetArg(1).Equals(""))
                {
                    await command.Channel.SendMessage(command.Message.User.Mention + " location was not specified.");
                    return;
                }
                DataType.weatherToday curWeather = owm.querySearch(command.GetArg(1));
                if (curWeather.main == null)
                {
                    await command.Channel.SendMessage("Location not found");
                }else
                {
                    await command.Channel.SendMessage(command.User.Name + " searched for weather for " + command.GetArg(1));
                    await command.Channel.SendMessage(curWeather.name + " is " + convertToFar( curWeather.main.temp) + " F");
                }
                await command.Message.Delete();
            });
            methods.Add("help", async (command) =>
            {
                await command.Channel.SendMessage(getHelp());
            });
        }

        public override string getHelp()
        {
            string help = "!weather commands:\n" +
                "get <query>: query weather for location <query>\n";
            return help;
        }

        public override async void runCommand(CommandEventArgs command)
        {
            if (!methods.ContainsKey(command.GetArg(0)) || methods[command.GetArg(0)] == null)
                await command.Channel.SendMessage(command.GetArg(0) + " is not an command");
            else 
                await methods[command.GetArg(0)](command);
        }
        public double convertToFar(double tempKelvin)
        {
            double temp = ((tempKelvin - 273.15) * 1.8 + 32);
            temp = ((int)(temp * 100)) /100.0;
            return temp;
        }
    }
}
