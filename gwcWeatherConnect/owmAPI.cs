using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using gwcWebConnect;

namespace gwcWeatherConnect
{
    public class owmAPI
    {
        private WebAPI webAccess;
        private string token;
        public owmAPI(string token)
        {
            this.token = token;
            webAccess = new WebAPI();
        }
        public weatherToday querySearch(string search)
        {
            String pat = "([0-9]{5})";
            Match m = Regex.Match(search, pat);
            String getWeather = webAccess.queryWebsiteGET("http://api.openweathermap.org/data/2.5/weather?" + ((m.Success)? "zip" : "q") + "=" + search + "&appid=" + token);
            return JsonConvert.DeserializeObject<weatherToday>(getWeather);
        }
        public static double convertToFar(double tempKelvin)
        {
            double temp = ((tempKelvin - 273.15) * 1.8 + 32);
            temp = ((int)(temp * 100)) / 100.0;
            return temp;
        }
    }
}
