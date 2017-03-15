using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using gwcWebConnect;
using System.IO;

namespace gwcWeatherConnect
{
    public class owmAPI
    {
        private WebAPI webAccess;
        private FileInfo configFile;
        private WeatherToken config;
        public owmAPI(FileInfo file)
        {
            configFile = file;
            
            webAccess = new WebAPI();

            if (config == null)
                using (StreamReader reader = new StreamReader(file.FullName))
                {
                    string json = reader.ReadToEnd();
                    config = JsonConvert.DeserializeObject<WeatherToken>(json);
                }
        }
        public weatherToday querySearch(string search)
        {
            String pat = "([0-9]{5})";
            Match m = Regex.Match(search, pat);
            String getWeather = webAccess.queryWebsiteGET("http://api.openweathermap.org/data/2.5/weather?" + ((m.Success)? "zip" : "q") + "=" + search + "&appid=" + config.token);
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
