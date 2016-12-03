using Discord;
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

namespace DiscordTest
{
    class MyBot
    {
            DiscordClient discord;
        public MyBot(String token, ulong allowedChannel)
        {
            ModuleBuilder moduleBuilder = new ModuleBuilder();
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
                if (!e.Message.IsAuthor &&  e.Message.Channel.Id == allowedChannel)
                {
                    if (e.Message.Text.Contains("meme") && !e.Message.Text.Contains("!meme") && e.Message.IsAuthor)
                    {
                        String getPhotos = getReply("https://api.imgur.com/3/gallery/search/?q_any=meme", "GET");
                        DataType.gallery stuff = getGalleryObject(getPhotos);
                        Random random = new Random();
                        await e.Channel.SendMessage(stuff.data[random.Next(stuff.data.Count)].link);
                    }
                    else if (e.Message.Text.Contains("sad"))
                    {

                        String getPhotos = getReply("https://api.imgur.com/3/gallery/search/?q_any=happy", "GET");
                        DataType.gallery stuff = getGalleryObject(getPhotos);
                        Random random = new Random();
                        
                        await e.Channel.SendMessage(stuff.data[random.Next(stuff.data.Count)].link);
                    }else if (e.Message.Text.Contains(":smile:"))
                    {
                        await e.Channel.SendMessage("You're Welcome. :D");
                    }
                }
            };
            var commands = discord.GetService<CommandService>();
            commands.CreateCommand("hello").AddCheck((cmd, user, channel) => channel.Id == allowedChannel || allowedChannel == 0).Do( async(e) =>{
                await e.Channel.SendMessage("Hi");
            });
            commands.CreateCommand("pics").AddCheck((cmd, user, channel) => channel.Id == allowedChannel || allowedChannel == 0).Parameter("arg1", ParameterType.Required).Parameter("arg2", ParameterType.Optional).Parameter("arg3", ParameterType.Optional).Do((e) =>
            {
                moduleBuilder.getModule("pics").runCommand(e);
            });
            commands.CreateCommand("magic8").AddCheck((cmd, user, channel) => channel.Id == allowedChannel || allowedChannel == 0).Parameter("arg1",ParameterType.Multiple).Do((e) =>
            {
                moduleBuilder.getModule("magic8").runCommand(e);
            });
            commands.CreateCommand("weather").AddCheck((cmd, user, channel) => channel.Id == allowedChannel || allowedChannel == 0).Parameter("arg1", ParameterType.Required).Parameter("arg2", ParameterType.Optional).Do((e) =>
            {
                moduleBuilder.getModule("weather").runCommand(e); 
            });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(token, TokenType.Bot);
            });
        }
        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        String getReplyPOST(String url, String postdata)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
            http.Method = "POST";
            http.ContentType = "application/x-www-form-urlencoded"; ;
            http.ContentLength = byteArray.Length;
            Stream datastream = http.GetRequestStream();
            datastream.Write(byteArray, 0, byteArray.Length);
            datastream.Close();

            var response = http.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            return content;
        }
        String getReply(String url, String method)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            http.Method = method;
            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            return content;
        }
        DataType.gallery getGalleryObject(String json)
        {
            DataType.gallery data = JsonConvert.DeserializeObject<DataType.gallery>(json);
            return data;
        }
        DataType.weatherToday getWeatherObject(String json)
        {
            return JsonConvert.DeserializeObject<DataType.weatherToday>(json);
        }
    }
}
