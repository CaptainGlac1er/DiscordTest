using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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
            String pat = "([0-9]{5})";
            Match m = Regex.Match(search, pat);
            String getWeather = webAccess.queryWebsiteGET("http://api.openweathermap.org/data/2.5/weather?" + ((m.Success)? "zip" : "q") + "=" + search + "&appid=" + System.Configuration.ConfigurationManager.ConnectionStrings["weathertoken"].ToString());
            return JsonConvert.DeserializeObject<DataType.weatherToday>(getWeather);
        }
        public static double convertToFar(double tempKelvin)
        {
            double temp = ((tempKelvin - 273.15) * 1.8 + 32);
            temp = ((int)(temp * 100)) / 100.0;
            return temp;
        }
    }
}
