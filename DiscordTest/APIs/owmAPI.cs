using System;
using Newtonsoft.Json;

namespace DiscordTest.APIs
{
    class owmAPI
    {
        private WebAPI webAccess;
        public owmAPI()
        {
            webAccess = new WebAPI();
        }
        public DataType.weatherToday querySearch(string search)
        {
            String getWeather = webAccess.queryWebsiteGET("http://api.openweathermap.org/data/2.5/weather?q=" + search + "&appid=" + System.Configuration.ConfigurationManager.ConnectionStrings["weathertoken"].ToString());
            return JsonConvert.DeserializeObject<DataType.weatherToday>(getWeather);
        }
    }
}
