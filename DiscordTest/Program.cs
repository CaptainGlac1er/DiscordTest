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
            MyBot Matt = new MyBot(System.Configuration.ConfigurationManager.ConnectionStrings["matttoken"].ToString(), ulong.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["mattschannel"].ToString()));
            //MyBot my = new MyBot(System.Configuration.ConfigurationManager.ConnectionStrings["mytoken"].ToString(), ulong.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["mychannel"].ToString()));
            Console.ReadLine();
            

        }
        
    }
}

