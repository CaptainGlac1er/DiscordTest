using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gwcWebConnect;
using Newtonsoft.Json;

namespace gwcImgurConnect
{
    public class ImgurAPI
    {
        private WebAPI webAccess;
        private static ImgurInfo connectionToken;
        public ImgurAPI(ImgurInfo token)
        {
            webAccess = new WebAPI();
            if(connectionToken == null)
                connectionToken = token;
        }
        public List<picture> querySearch(string search)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + connectionToken.ImgurConectToken);
            string json = webAccess.queryWebsiteGET("https://api.imgur.com/3/gallery/search/?q_any=" + search, headers);
            return JsonConvert.DeserializeObject<gallery>(json).data.ToList();
        }
        public ImgurInfo refreshToken()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + connectionToken.ImgurConectToken);
            string json = webAccess.queryWebsitePOST("https://api.imgur.com/oauth2/token", headers, "grant_type=refresh_token&client_id=" + connectionToken.ClientID + "&client_secret=" + connectionToken.ImgurSecret + "&refresh_token=" + connectionToken.ImgurRefreshToken);
            refreshJSON newInfo = JsonConvert.DeserializeObject<refreshJSON>(json);
            connectionToken = new ImgurInfo(newInfo.access_token, newInfo.refresh_token, connectionToken.ClientID, connectionToken.ImgurSecret);
            return connectionToken;
        }
    }
}
