using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gwcFileSystem;
using gwcDiscordConnect;

namespace DiscordTest
{
    class ModuleBuilder
    {
        Dictionary<String, Module> modules = new Dictionary<string, Module>();
        public ModuleBuilder(FileSystem filesystem, IList<Admin> admins)
        {
            Images imageSource = new Images(filesystem.getFile("ImgurConfig.json"), admins);
            OpenWeatherMap weatherSource = new OpenWeatherMap(filesystem.getFile("OpenWeatherMapConfig.json"));
            WeatherUnderGround wuSource = new WeatherUnderGround(filesystem.getFile("WeatherUndergroundConfig.json"));
            Magic8 magic8 = new Magic8();
            Cleverbot cleverbot = new Cleverbot(filesystem.getFile("CleverbotConfig.json"));
            modules.Add(imageSource.getCommand(), imageSource);
            modules.Add(weatherSource.getCommand(), weatherSource);
            modules.Add(magic8.getCommand(), magic8);
            modules.Add(wuSource.getCommand(), wuSource);
            modules.Add(cleverbot.getCommand(), cleverbot);
        }
        public Module getModule(String command)
        {
            return modules[command];
        }
    }
}
