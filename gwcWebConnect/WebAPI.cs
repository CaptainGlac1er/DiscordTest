using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace gwcWebConnect
{
    public class WebAPI
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
            url = HttpUtility.HtmlEncode(url);
            var http = (HttpWebRequest)WebRequest.Create(url);
            addHeaders(http, headers);
            http.Method = "GET";
            WebResponse reply = http.GetResponse();
            return reply;

        }
        public WebResponse getWebsitePOST(string url, Dictionary<string, string> headers, string postdata)
        {
            url = HttpUtility.HtmlEncode(url);
            var http = (HttpWebRequest)WebRequest.Create(url);
            addHeaders(http, headers);
            byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
            http.Method = "POST";
            http.ContentType = "application/x-www-form-urlencoded"; ;
            http.ContentLength = byteArray.Length;
            Stream datastream = http.GetRequestStream();
            datastream.Write(byteArray, 0, byteArray.Length);
            datastream.Close();
            Console.WriteLine(postdata);
            WebResponse webResponse = null;
            try
            { 
                webResponse = http.GetResponse();
            }catch(WebException e)
            {
                string contents = "error";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (var response = (HttpWebResponse)e.Response)
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    HttpStatusCode statusCode = ((HttpWebResponse)response).StatusCode;
                    contents = reader.ReadToEnd();
                }
                Console.WriteLine(contents);
            }
            return webResponse;
        }
        public String queryWebsitePOST(string url, Dictionary<string, string> headers, string postdata)
        {
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
