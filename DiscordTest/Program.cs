using Discord;
using DiscordConnect;
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
using gwcFileSystem;



namespace DiscordTest
{
    class Program
    {
        private FileSystem filesystem;
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
        private Dictionary<String, Thread> bots;
        private Servers serversAvailable;
        public Program()
        {
            FileSystem filesystem = new FileSystem(Directory.GetCurrentDirectory());
            DiscordConnectInfo connect = new DiscordConnectInfo(filesystem.getFile("DiscordConfig.json"));
            serversAvailable = connect.getServers();
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
            Dictionary<String, DiscordConnect.Server> servers = new Dictionary<string, DiscordConnect.Server>();
            Dictionary<String, DiscordConnect.Channel> channels = new Dictionary<string, DiscordConnect.Channel>();
            
            foreach (DiscordConnect.Server Dserver in serversAvailable.servers)
            {
                servers.Add(Dserver.name, Dserver);
                Console.WriteLine(Dserver.name);
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
            foreach (DiscordConnect.Channel Dchannel in servers[server].channels)
            {
                channels.Add(Dchannel.name, Dchannel);
                Console.WriteLine(Dchannel.name);
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
                MyBot myBot = new MyBot(servers[server], ulong.Parse(channels[channel].token));
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

