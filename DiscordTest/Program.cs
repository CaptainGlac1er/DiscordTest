using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DiscordTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            XmlTextReader xmlReader = new XmlTextReader(Directory.GetCurrentDirectory() + "\\DiscordConfigs.xml");
            XmlDocument xmlSettings = new XmlDocument();
            xmlSettings.Load(Directory.GetCurrentDirectory() + "\\DiscordConfigs.xml");
            
            Dictionary<String, XmlNode> servers = new Dictionary<string, XmlNode>(); 
            foreach(XmlNode node in xmlSettings.ChildNodes[1].ChildNodes)
            {
                servers.Add(node["discordName"].InnerText, node);
                Console.WriteLine(node["discordName"].InnerText);
            }
            String server = "";
            Console.WriteLine();
            while (!servers.ContainsKey(server))
            {
                Console.WriteLine("What server do you want to start?");
                server = Console.ReadLine();
            }
            //Console.Clear();
            Dictionary<String, XmlNode> channels = new Dictionary<string, XmlNode>();
            foreach (XmlNode node in servers[server]["discordChannels"].ChildNodes)
            {
                channels.Add(node["channelName"].InnerText, node);
                Console.WriteLine(node["channelName"].InnerText);
            }
            String channel = "";
            Console.WriteLine();
            while (!channels.ContainsKey(channel))
            {
                Console.WriteLine("What channel do you want to start?");
                channel = Console.ReadLine();
            }
            //Console.Clear();
            Thread newThread = new Thread(() =>
            {
                MyBot myBot = new MyBot(servers[server]["discordToken"].InnerText, ulong.Parse(channels[channel]["token"].InnerText));
            });
            Console.WriteLine("Start Bot? (y/n)");
            bool start = false;
            while (!start)
            {
                String read = Console.ReadLine();
                if (read.Equals("y") && (start = true)) ;
                if (read.Equals("n"))
                    System.Environment.Exit(0);

            }
            newThread.Start();

            //MyBot Matt = new MyBot(System.Configuration.ConfigurationManager.ConnectionStrings["matttoken"].ToString(), ulong.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["mattschannel"].ToString()));
            //MyBot my = new MyBot(System.Configuration.ConfigurationManager.ConnectionStrings["mytoken"].ToString(), ulong.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["mychannel"].ToString()));
            Console.ReadLine();
                

        }
        
    }
}

