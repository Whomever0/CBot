using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CBot
{
    class Configuration
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.json";
        public static BotConfiguration bot;

        static Configuration(){
            if(!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);
        
            if(!File.Exists(configFolder + "/" + configFile)){
                bot = new BotConfiguration();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }else{
                string json = File.ReadAllText(configFolder + "/" + configFile);
                bot = JsonConvert.DeserializeObject<BotConfiguration>(json);
            }
        }
    }

    public struct BotConfiguration{
        public string token;
        public string prefix;
        public string prefix2;
    }
}
