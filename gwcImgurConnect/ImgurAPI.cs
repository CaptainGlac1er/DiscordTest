﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gwcWebConnect;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using gwcFileSystem;
using System.Threading;

namespace gwcImgurConnect
{
    public class ImgurAPI
    {
        private WebAPI webAccess;
        private static ImgurInfo connectionToken;
        private static FileSystemFile configFile;
        public ImgurAPI(FileSystemFile file)
        {
            configFile = file;
            webAccess = new WebAPI();
            if(connectionToken == null)
                connectionToken = JsonConvert.DeserializeObject<ImgurInfo>(configFile.getFileContents());
        }
        public List<picture> querySearch(string search)
        {
            try
            {
                string json = webAccess.queryWebsiteGET("https://api.imgur.com/3/gallery/search/?q_any=" + search, createHeader());
                return JsonConvert.DeserializeObject<gallery>(json).data.ToList();
            }
            catch (WebException e)
            {
                if (((HttpWebResponse)((WebException)e).Response).StatusCode == HttpStatusCode.Forbidden)
                {
                    refreshToken();
                    return querySearch(search);
                }else
                {
                    return new List<picture>();
                }
            }
        }
        public ImgurInfo refreshToken()
        {
            string json = webAccess.queryWebsitePOST("https://api.imgur.com/oauth2/token", createHeader(), "grant_type=refresh_token&client_id=" + connectionToken.ClientID + "&client_secret=" + connectionToken.ImgurSecret + "&refresh_token=" + connectionToken.ImgurRefreshToken);
            refreshJSON newInfo = JsonConvert.DeserializeObject<refreshJSON>(json);
            connectionToken = new ImgurInfo(newInfo.access_token, newInfo.refresh_token, connectionToken.ClientID, connectionToken.ImgurSecret);
            configFile.writeObject(connectionToken);
            //updateFile();
            return connectionToken;
        }
        private Dictionary<string, string> createHeader()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + connectionToken.ImgurConectToken);
            return headers;

        }
    }
}
