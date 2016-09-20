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
        Images images;
        public MyBot(String token, ulong allowedChannel)
        {
            images = new Images();
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });
            var resetEvent = new AutoResetEvent(true);
            Timer tm = new Timer((Object state) =>
            {
                String getPhotos = getReply("https://api.imgur.com/3/gallery/search/?q_any=meme", "GET");
                gallery stuff = getGalleryObject(getPhotos);
                Random random = new Random();
                Console.WriteLine("fired");
                discord.GetChannel(allowedChannel).SendMessage(stuff.data[random.Next(stuff.data.Count)].link);
            }, resetEvent,30*60*1000, 30 * 60 * 1000); 
            discord.UsingCommands(x=>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });
            discord.MessageReceived += async (s, e) =>
            {
                Console.WriteLine(e.Message.Channel.Id);
                if (!e.Message.IsAuthor &&  e.Message.Channel.Id == allowedChannel)
                {
                    if (e.Message.Text.Contains("meme") && !e.Message.Text.Contains("!meme") && e.Message.IsAuthor)
                    {
                        String getPhotos = getReply("https://api.imgur.com/3/gallery/search/?q_any=meme", "GET");
                        gallery stuff = getGalleryObject(getPhotos);
                        Random random = new Random();
                        await e.Channel.SendMessage(stuff.data[random.Next(stuff.data.Count)].link);
                    }
                    if (e.Message.Text.Contains("sad"))
                    {

                        String getPhotos = getReply("https://api.imgur.com/3/gallery/search/?q_any=happy", "GET");
                        gallery stuff = getGalleryObject(getPhotos);
                        Random random = new Random();
                        
                        await e.Channel.SendMessage(stuff.data[random.Next(stuff.data.Count)].link);
                    }
                }
            };
            var commands = discord.GetService<CommandService>();
            commands.CreateCommand("hello").AddCheck((cmd, user, channel) => channel.Id == allowedChannel || allowedChannel == 0).Do( async(e) =>{
                await e.Channel.SendMessage("Hi");
            });
            commands.CreateCommand("pics").AddCheck((cmd, user, channel) => channel.Id == allowedChannel || allowedChannel == 0).Parameter("arg1", ParameterType.Required).Parameter("arg2", ParameterType.Optional).Parameter("arg3", ParameterType.Optional).Do( (e) =>
            {
                images.runCommand(e);
            });
            commands.CreateCommand("weather").AddCheck((cmd, user, channel) => channel.Id == allowedChannel || allowedChannel == 0).Parameter("zip").Do(async (e) =>
            {
                //Console.WriteLine("testing");
                String getWeather = getReply("http://api.openweathermap.org/data/2.5/weather?q=" + e.GetArg("zip") + "&appid=" + System.Configuration.ConfigurationManager.ConnectionStrings["weathertoken"].ToString(), "GET");
                //Console.WriteLine(getWeather);
                weatherToday w = getWeatherObject(getWeather);

                await e.Channel.SendMessage(w.name + " is " + ((w.main.temp - 273.15) * 1.8 + 32) + " F");
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
        gallery getGalleryObject(String json)
        {
            gallery data = JsonConvert.DeserializeObject<gallery>(json);
            return data;
        }
        weatherToday getWeatherObject(String json)
        {
            return JsonConvert.DeserializeObject<weatherToday>(json);
        }
    }
}
