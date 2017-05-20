using gwcFileSystem;
using gwcWebConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gwcWeatherConnect
{
    public class wuAPI
    {
        private WebAPI webAccess;
        private FileSystemFile configFile;
        private WeatherToken config;
        public wuAPI(FileSystemFile file)
        {
            configFile = file;

            webAccess = new WebAPI();

            if (config == null)
                config = JsonConvert.DeserializeObject<WeatherToken>(file.getFileContents());
        }
        public string getCurrentWeather(string search)
        {
            String getWeather = webAccess.queryWebsiteGET("http://api.wunderground.com/api/" + config.token + "/conditions/q/" + search + ".json");
            DataTypes.WeatherUndergroundResponse response = JsonConvert.DeserializeObject<DataTypes.WeatherUndergroundResponse>(getWeather);
            String reply = "error 404";
            if (response.current_observation != null)
                reply = "It is currently " + response.current_observation.temp_f + " F degrees in " + response.current_observation.display_location.full + "\r\nGet forecast at " + response.current_observation.forecast_url;
            else if (response.response.error != null)
                reply = response.response.error.type + " " + response.response.error.description;
            else if(response.response.results != null)
            {
                reply = "Please be more specific\r\n```";
                foreach(DataTypes.Result result in response.response.results)
                {
                    reply += result.city + " " + result.state + " " + result.country_name + "\r\n";
                }
                reply += "```";
            }
            return reply; 
        }
        public static double convertToFar(double tempKelvin)
        {
            double temp = ((tempKelvin - 273.15) * 1.8 + 32);
            temp = ((int)(temp * 100)) / 100.0;
            return temp;
        }
    }
}
