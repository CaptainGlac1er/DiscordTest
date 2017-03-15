using System;
using Newtonsoft.Json;
using gwcWebConnect;
using System.IO;

namespace gwcCleverbotConnect
{
    public class CleverbotAPI
    {
        private static CleverbotToken config;
        private string conversation;
        private WebAPI webAPI;
        public CleverbotAPI(FileInfo file)
        {
            webAPI = new WebAPI();
            conversation = "";
            if(config == null)
                using (StreamReader reader = new StreamReader(file.FullName))
                {
                    string json = reader.ReadToEnd();
                    config = JsonConvert.DeserializeObject<CleverbotToken>(json);
                }
        }
        private cleverbotReply getReplyObject(string input)
        {
            string url = "http://www.cleverbot.com/getreply?key=" + config.token + "&input=" + input + "&cs=" + conversation;
            string jsonReply = webAPI.queryWebsiteGET(url);
            cleverbotReply output = JsonConvert.DeserializeObject<cleverbotReply>(jsonReply);
            conversation = output.cs;
            return output;
        }
        public string getReply(string input)
        {
            cleverbotReply reply = getReplyObject(input);
            if (reply.clever_accuracy > 90)
                return reply.clever_output;
            else
                return reply.output;
        }

    }
}
