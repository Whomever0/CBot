using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace CBot
{
    class DataStore
    {
        private static Dictionary<string, string> pairs = new Dictionary<string, string>();

        public static void addPair(string key, string value){
            pairs.Add(key, value);
            SaveData();
        }

        public static int getPairsInt(){
            return pairs.Count;
        }

        static DataStore(){
            //Loading Data
            if(validateStoreFile("DataStore.json")) return;
            string json = File.ReadAllText("DataStore.json");
            pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static void SaveData(){
            //Saving Data
            string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText("DataStore.json", json);
        }

        private static bool validateStoreFile(string file){
            if(!File.Exists(file)){
                File.WriteAllText(file,"");
                SaveData();
                return false;
            }
            return true;
        }
    }
}
