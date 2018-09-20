using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBot.Core.UserAccounts;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;

namespace CBot.Core.UserAccounts
{
    public static class DataStore
    {
        //Save all
        public static void saveAccount(IEnumerable<UserAccount> accounts, string filePath){
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath,json);
        }

        //Get all
        public static IEnumerable<UserAccount> loadAccounts(string filePath){
            if(!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccount>>(json);
        }

        public static bool saveExists(string filePath){
            return File.Exists(filePath);
        }
    }
}
