﻿using System;
using Newtonsoft.Json;
using gwcWebConnect;

namespace gwcCleverbotConnect
{
    public class CleverbotAPI
    {
        private string conversation;
        private string apiKey;
        private WebAPI webAPI;
        public CleverbotAPI(string apiKey)
        {
            webAPI = new WebAPI();
            this.apiKey = apiKey;
            conversation = "";
        }
        private cleverbotReply getReplyObject(string input)
        {
            string url = "http://www.cleverbot.com/getreply?key=" + apiKey + "&input=" + input + "&cs=" + conversation + "&callback=ProcessReply";
            Console.WriteLine(url);
            string jsonReply = webAPI.queryWebsiteGET(url);
            cleverbotReply output = JsonConvert.DeserializeObject<cleverbotReply>(jsonReply);
            conversation = output.conversation_id;
            return output;
        }
        public string getReply(string input)
        {
            cleverbotReply reply = getReplyObject(input);
            if (reply.clever_accuracy > 80)
                return reply.clever_output;
            else
                return reply.output;
        }

    }
}