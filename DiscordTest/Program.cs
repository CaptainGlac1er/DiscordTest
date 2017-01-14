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
            Program program = new Program();
            
            while (true)
            {
                //foreach(String name in program.bots.Keys.ToArray())
                //{
                //    Console.WriteLine(name + " currently running");
                //}
                Console.WriteLine("Type add to add another bot");
                String add = Console.ReadLine();
                switch (add)
                {
                    case "add":
                        program.addNewBot();
                        break;
                    case "remove":
                        String remove = "";
                        while (!program.removeBot(remove))
                        {
                            Console.WriteLine("Which one?");
                            remove = Console.ReadLine();
                            if (remove.Equals("quit"))
                                break;
                        }
                        break;
                    case "refresh":
                        foreach (String name in program.bots.Keys.ToArray())
                        {
                            Console.WriteLine(name + " currently running");
                        }
                        break;
                }
            }

            //MyBot Matt = new MyBot(System.Configuration.ConfigurationManager.ConnectionStrings["matttoken"].ToString(), ulong.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["mattschannel"].ToString()));
            //MyBot my = new MyBot(System.Configuration.ConfigurationManager.ConnectionStrings["mytoken"].ToString(), ulong.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["mychannel"].ToString()));
            Console.ReadLine();


        }
        Dictionary<String, Thread> bots;
        public Program()
        {
            bots = new Dictionary<string, Thread>();
        }
        public Dictionary<String, Thread> getBots()
        {
            return bots;
        }
        public bool removeBot(String name)
        {
            if (bots.ContainsKey(name))
            {
                bots[name].Abort();
                bots.Remove(name);
                return true;
            }
            else return false;
        }
        public void addNewBot()
        {
            Console.Clear();
            XmlTextReader xmlReader = new XmlTextReader(Directory.GetCurrentDirectory() + "\\DiscordConfigs.xml");
            XmlDocument xmlSettings = new XmlDocument();
            xmlSettings.Load(Directory.GetCurrentDirectory() + "\\DiscordConfigs.xml");

            Dictionary<String, XmlNode> servers = new Dictionary<string, XmlNode>();
            foreach (XmlNode node in xmlSettings.ChildNodes[1].ChildNodes)
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
                if (bots.ContainsKey(server))
                {
                    Console.WriteLine("Already has a bot running");
                    return;
                }
            }
            Console.Clear();
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
            Console.Clear();
            Thread newThread = new Thread(() =>
            {
                MyBot myBot = new MyBot(servers[server]["discordToken"].InnerText, ulong.Parse(channels[channel]["token"].InnerText));
            });
            bots.Add(server, newThread);
            Console.WriteLine("Start Bot? (y/n)");
            bool start = false;
            while (!start)
            {
                String read = Console.ReadLine();
                if (read.Equals("y") && (start = true)) ;
                if (read.Equals("n"))
                    return;

            }
            newThread.Start();
        }
        
    }
}

