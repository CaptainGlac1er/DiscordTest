using System;
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
            return queryWebsiteGET(url, null);
        }
        public string queryWebsiteGET(string url, Dictionary<string, string> headers)
        {
            var response = getWebsiteGET(url, headers);
            var stream = response.GetResponseStream();
            if (false) // set true if you want to check headers coming back
            {
                foreach (string v in response.Headers.Keys)
                    Console.WriteLine(v + " " + response.Headers[v]);
            }
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            return content;
        }
        public WebResponse getWebsiteGET(string url, Dictionary<string, string> headers)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            addHeaders(http, headers);
            http.Method = "GET";
            return http.GetResponse();

        }
        public WebResponse getWebsitePOST(string url, Dictionary<string, string> headers, string postdata)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            addHeaders(http, headers);
            byte[] byteArray = Encoding.UTF8.GetBytes(postdata); 
            http.Method = "POST"; 
            http.ContentType = "application/x-www-form-urlencoded"; ; 
            http.ContentLength = byteArray.Length; 
            Stream datastream = http.GetRequestStream(); 
            datastream.Write(byteArray, 0, byteArray.Length); 
            datastream.Close(); 
            return http.GetResponse(); 
        }
        public String queryWebsitePOST(string url, Dictionary<string, string> headers, string postdata){
            var response = getWebsitePOST(url, headers, postdata); 
            var stream = response.GetResponseStream(); 
            var sr = new StreamReader(stream); 
            var content = sr.ReadToEnd(); 
            return content; 

        }
        private void addHeaders(HttpWebRequest request, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> pair in headers)
                {
                    request.Headers.Add(pair.Key, pair.Value);
                }
            }

        }
    }
}
