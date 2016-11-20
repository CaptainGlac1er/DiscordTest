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
        public ModuleBuilder()
        {
            imageSource = new Images();
            weatherSource = new Weather();
        }
        public Module getModule(String command)
        {
            switch (command)
            {
                case "pics":
                    return imageSource;
                case "weather":
                    return weatherSource;
            }
            return null;
        }
    }
}
