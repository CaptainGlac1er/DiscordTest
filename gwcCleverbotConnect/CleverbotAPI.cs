using System;
using Newtonsoft.Json;
using gwcWebConnect;
using System.IO;
using gwcFileSystem;

namespace gwcCleverbotConnect
{
    public class CleverbotAPI
    {
        private static CleverbotToken config;
        private string conversation;
        private WebAPI webAPI;
        public CleverbotAPI(FileSystemFile file)
        {
            webAPI = new WebAPI();
            conversation = "";
            if(config == null)
                config = JsonConvert.DeserializeObject<CleverbotToken>(file.getFileContents());
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
