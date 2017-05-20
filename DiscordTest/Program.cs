using Discord;
using gwcDiscordConnect;
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
        private static Dictionary<string, string> commands = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            Program program = new Program();

            commands.Add("listen", "listen to new server");
            commands.Add("mute", "stop listening to server");
            commands.Add("refresh", "check what servers are running");
            commands.Add("quit", "check what servers are running");

            Console.WriteLine("DiscordTest by George Colgrove");
            Console.WriteLine("help for commands");
            Console.Write(">");
            while (true)
            {
                String add = Console.ReadLine();
                switch (add)
                {
                    case "listen":
                        program.addNewBot();
                        break;
                    case "mute":
                        String remove = "";
                        while (!program.removeBot(remove))
                        {
                            Console.WriteLine("Which one?");
                            remove = Console.ReadLine();
                            if (remove.Equals("quit"))
                                break;
                        }
                        break;
                    case "list":
                        foreach (String name in program.bots.Keys.ToArray())
                        {
                            Console.WriteLine(name + " currently running");
                        }
                        break;
                    case "help":
                        foreach (String name in commands.Keys)
                        {
                            Console.WriteLine(string.Format("{0, -20} {1}", name, commands[name]));
                        }
                        break;
                    case "quit":
                        Console.WriteLine("Shutting down...");
                        foreach (String name in program.bots.Keys.ToArray())
                            program.removeBot(name);
                        return;
                }
                Console.Write("\n>");
            }

        }
        private FileSystem config;
        private FileSystem filesystem;
        private Dictionary<String, Thread> bots;
        private Servers serversAvailable;
        public Program()
        {
            filesystem = new FileSystem(Directory.GetCurrentDirectory());
            config = new FileSystem(filesystem.getDirectory("Config"));
            DiscordConnectInfo connect = new DiscordConnectInfo(config.getFile("DiscordConfig.json"));
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
            Dictionary<String, gwcDiscordConnect.Server> servers = new Dictionary<string, gwcDiscordConnect.Server>();
            Dictionary<String, gwcDiscordConnect.Channel> channels = new Dictionary<string, gwcDiscordConnect.Channel>();
            
            foreach (gwcDiscordConnect.Server Dserver in serversAvailable.servers)
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
            foreach (gwcDiscordConnect.Channel Dchannel in servers[server].channels)
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
                MyBot myBot = new MyBot(servers[server], config);
            });
            bots.Add(server, newThread);
            Console.WriteLine("Start Bot? (y/n)");
            bool start = false;
            while (!start)
            {
                String read = Console.ReadLine();
                if (read.Equals("y"))
                    start = true;
                if (read.Equals("n"))
                    return;
            }
            newThread.Start();
        }
        
    }
}

