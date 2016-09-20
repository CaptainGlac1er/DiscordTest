﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTest
{
    class WebAPI
    {
        public WebAPI()
        {

        }
        public string queryWebsiteGET(string url)
        {
            return queryWebsiteGET(url, new Dictionary<string, string>());
        }
        public string queryWebsiteGET(string url, Dictionary<string, string> headers)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            foreach (KeyValuePair<string, string> pair in headers)
            {
                http.Headers.Add(pair.Key, pair.Value);
            }
            //http.Headers.Add("Authorization", "Bearer 402df92c7bc336c9b17ba618c9edf22e95694821");
            http.Method = "GET";
            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            return content;
        }
    }
}
