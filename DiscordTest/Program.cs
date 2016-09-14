using Discord;
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
    class Program
    {
        static void Main(string[] args)
        {
            //var auth = (HttpWebRequest)WebRequest.Create("https://api.imgur.com/oauth2/authorize?client_id=7aba5f028647b94&response_type=token");
            //var authreply = auth.GetResponse();
            /*var http = (HttpWebRequest)WebRequest.Create("https://api.imgur.com/3/gallery/search/");
            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();*/
            /*
             * {"access_token":"402df92c7bc336c9b17ba618c9edf22e95694821","expires_in":2419200,"token_type":"bearer","scope":null,"refresh_token":"402037ad168297b4bdb3062ca947da49353ce59b","account_id":40122590,"account_username":"CaptainGlac1er"}
             * 
             */
            //Console.Write(getReply("https://api.imgur.com/oauth2/authorize?client_id=7aba5f028647b94&response_type=pin", "GET"));
            //Console.ReadLine();
            //Console.Write(getReplyPOST("https://api.imgur.com/oauth2/token", "client_id=7aba5f028647b94&client_secret=c58a6e91cdecef409cf5fbead8ba86afabdfd1ca&grant_type=pin&pin=46e960efe0"));
            //Console.ReadLine();
            //Console.WriteLine(getReplyPOST("https://discordapp.com/api/oauth2/authorize", "client_id=225390596863426560"));
            MyBot bot = new MyBot();
            Console.ReadLine();
            String getPhotos = getReply("https://api.imgur.com/3/gallery/search/?q_any=nerd", "GET");
            gallery stuff = getGalleryObject(getPhotos);
            foreach(picture thing in stuff.data)
                Console.WriteLine(thing.link);
            Console.ReadLine();

        }
        static String getReplyPOST(String url, String postdata)
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
        static String getReply(String url, String method)
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
        static gallery getGalleryObject(String json)
        {
            gallery data = JsonConvert.DeserializeObject<gallery>(json);
            return data;
        }
        
    }
}
