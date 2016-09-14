using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTest
{
    class MyBot
    {
            DiscordClient discord;
        public MyBot()
        {
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
                if (!e.Message.IsAuthor)
                {
                    if (e.Message.Text.Contains("meme") && !e.Message.Text.Contains("!meme"))
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
            commands.CreateCommand("hello").Do( async(e) =>{
                await e.Channel.SendMessage("Hi");
            });
            commands.CreateCommand("meme").Parameter("Type").Do(async (e) =>
            {
                String getPhotos = getReply("https://api.imgur.com/3/gallery/search/?q_any=" + e.GetArg("Type"), "GET");
                gallery stuff = getGalleryObject(getPhotos);
                Random random = new Random();
                await e.Channel.SendMessage(stuff.data[random.Next(stuff.data.Count)].link);
            });
            commands.CreateCommand("weather").Parameter("zip").Do(async (e) =>
            {
                //Console.WriteLine("testing");
                String getWeather = getReply("http://api.openweathermap.org/data/2.5/weather?q=" + e.GetArg("zip") + "&appid=458b06cb384e991e14c7bbd0c2ca1ccc", "GET");
                //Console.WriteLine(getWeather);
                weatherToday w = getWeatherObject(getWeather);

                await e.Channel.SendMessage(w.name + " is " + w.main.temp);
            });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MjI1Mzk4NzgxMzY4MDc0MjQx.Cronaw.PQmHl5-vT8gNNkDrEbbCJszLpQc", TokenType.Bot);
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
            http.Headers.Add("Authorization", "Bearer 402df92c7bc336c9b17ba618c9edf22e95694821");
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
