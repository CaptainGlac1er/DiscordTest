using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTest
{
    class ModuleBuilder
    {
        Images imageSource;
        Weather weatherSource;
        Magic8 magic8;
        public ModuleBuilder()
        {
            imageSource = new Images();
            weatherSource = new Weather();
            magic8 = new Magic8();
        }
        public Module getModule(String command)
        {
            switch (command)
            {
                case "pics":
                    return imageSource;
                case "weather":
                    return weatherSource;
                case "magic8":
                    return magic8;
            }
            return null;
        }
    }
}
