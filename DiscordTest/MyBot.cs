﻿using Discord;
using Discord.Commands;
using Discord.Commands.Permissions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using gwcDiscordConnect;
using gwcWebConnect;
using gwcWeatherConnect;
using gwcFileSystem;

namespace DiscordTest
{
    class MyBot
    {
            DiscordClient discord;
        WebAPI webAPI = new WebAPI();
        List<ulong> allowedChannels = new List<ulong>();
        public MyBot(gwcDiscordConnect.Server server, FileSystem filesystem)
        {

            ModuleBuilder moduleBuilder = new ModuleBuilder(filesystem, server);
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });
            discord.UsingCommands(x=>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });
            discord.MessageReceived += async (s, e) =>
            {
                Console.WriteLine(e.Message.RawText);
                if (!e.Message.IsAuthor && server.getChannelsCommands().Keys.Contains(e.Channel.Id))
                {
                    if (e.Message.Text.Contains(":smile:"))
                    {
                        await e.Channel.SendMessage("You're Welcome. :D");
                    }
                }
            };
            var commands = discord.GetService<CommandService>();
            
            commands.CreateCommand("hello").Do( async(e) =>{
                await e.Channel.SendMessage("Hi");
            });
            commands.CreateCommand("pics").Parameter("arg1", ParameterType.Required).Parameter("arg2", ParameterType.Optional).Parameter("arg3", ParameterType.Optional).Do((e) =>
            {
                Module module = moduleBuilder.getModule("pics", e.Channel.Id);
                if(module != null)
                    module.runCommand(e);
            });
            commands.CreateCommand("magic8").Parameter("arg1",ParameterType.Multiple).Do((e) =>
            {
                Module module = moduleBuilder.getModule("magic8", e.Channel.Id);
                if (module != null)
                    module.runCommand(e);
            });
            commands.CreateCommand("owm").Parameter("arg1", ParameterType.Required).Parameter("arg2", ParameterType.Optional).Parameter("arg3", ParameterType.Optional).Do((e) =>
            {
                Module module = moduleBuilder.getModule("owm", e.Channel.Id);
                if (module != null)
                    module.runCommand(e);
            });
            commands.CreateCommand("wu").Parameter("arg1", ParameterType.Required).Parameter("arg2", ParameterType.Unparsed).Do((e) =>
            {
                Module module = moduleBuilder.getModule("wu", e.Channel.Id);
                if (module != null)
                    module.runCommand(e);
            });
            commands.CreateCommand("chat").Parameter("arg1", ParameterType.Unparsed).Do((e) =>
            {
                Module module = moduleBuilder.getModule("chat", e.Channel.Id);
                if (module != null)
                    module.runCommand(e);
            });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(server.token, TokenType.Bot);
            });
        }
        public void addChannel()
        {
            //discord.get
        }
        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        DataType.gallery getGalleryObject(String json)
        {
            DataType.gallery data = JsonConvert.DeserializeObject<DataType.gallery>(json);
            return data;
        }
        weatherToday getWeatherObject(String json)
        {
            return JsonConvert.DeserializeObject<weatherToday>(json);
        }
    }
}
