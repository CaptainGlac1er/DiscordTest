using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordTest
{
    class ModuleBuilder
    {
        public Module getModule(String command)
        {
            switch (command)
            {
                case "pics":
                    return new Images();
                case "weather":
                    return new Weather();
            }
            return null;
        }
    }
}
