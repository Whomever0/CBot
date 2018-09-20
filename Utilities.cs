using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace CBot
{
    class Utilities
    {
        static Dictionary<string, string> alerts;

        static Utilities(){
            string json = File.ReadAllText("SysLang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        public static string getJson(string key){
            if(alerts.ContainsKey(key)) return alerts[key];
            return "";
        }

        public static string getFormattedAlert(string key, object parameter){
            if(alerts.ContainsKey(key)){ 
                return String.Format(alerts[key], parameter);
            }
            return "";
        }
    }
}
