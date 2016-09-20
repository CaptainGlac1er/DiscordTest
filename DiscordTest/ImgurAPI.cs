using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTest
{
    class ImgurAPI
    {
        private WebAPI webAccess;
        public ImgurAPI()
        {
            webAccess = new WebAPI();
        }
        public List<picture> querySearch(string search)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + System.Configuration.ConfigurationManager.ConnectionStrings["imgur"]);
            string json = webAccess.queryWebsiteGET("https://api.imgur.com/3/gallery/search/?q_any=" + search, headers);
            return JsonConvert.DeserializeObject<gallery>(json).data.ToList();

        }
    }
}
