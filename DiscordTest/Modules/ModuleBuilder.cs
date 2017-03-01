using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTest
{
    class ModuleBuilder
    {
        Dictionary<String, Module> modules = new Dictionary<string, Module>();
        public ModuleBuilder()
        {
            Images imageSource = new Images();
            Weather weatherSource = new Weather();
            Magic8 magic8 = new Magic8();
            Cleverbot cleverbot = new Cleverbot();
            modules.Add(imageSource.getCommand(), imageSource);
            modules.Add(weatherSource.getCommand(), weatherSource);
            modules.Add(magic8.getCommand(), magic8);
            modules.Add(cleverbot.getCommand(), cleverbot);
        }
        public Module getModule(String command)
        {
            return modules[command];
        }
    }
}
