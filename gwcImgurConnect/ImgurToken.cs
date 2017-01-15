using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gwcImgurConnect
{
    public class ImgurInfo
    {
        private string imgurConectToken, imgurRefreshToken, clientID, imgurSecret;
        public ImgurInfo(string token, string refresh, string client, string secret)
        {
            ImgurConectToken = token;
            ImgurRefreshToken = refresh;
            ClientID = client;
            ImgurSecret = secret;
        }
        public string ImgurConectToken
        {
            get
            {
                return imgurConectToken;
            }
            set
            {
                imgurConectToken = value;
            }
        }
        public string ImgurRefreshToken
        {
            get
            {
                return imgurRefreshToken;
            }
            set
            {
                imgurRefreshToken = value;
            }
        }
        public string ClientID
        {
            get
            {
                return clientID;
            }
            set
            {
                clientID = value;
            }
        }
        public string ImgurSecret
        {
            get
            {
                return imgurSecret;
            }
            set
            {
                imgurSecret = value;
            }
        }
        public override string ToString()
        {
            return "connect: " + ImgurConectToken + " refresh: " + ImgurRefreshToken;
        }
    }
}
