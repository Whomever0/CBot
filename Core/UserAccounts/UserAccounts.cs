using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CBot.Core.UserAccounts
{
    public static class UserAccounts
    {
        private static List<UserAccount> accounts;

        private static string accountsFile = "Resources/accounts.json";
        static UserAccounts(){
            if(DataStore.saveExists(accountsFile)){
                accounts = DataStore.loadAccounts(accountsFile).ToList();
            }else{
                accounts = new List<UserAccount>();
                saveAccounts();
            }
        }

        public static void saveAccounts(){
            DataStore.saveAccount(accounts, accountsFile);
        }

        public static UserAccount getAccount(SocketUser user){
            return getOrCreateAccount(user.Id);
        }

        private static UserAccount getOrCreateAccount(ulong id){
            var result = from a in accounts
                                   where a.userId == id
                                   select a;
            var account = result.FirstOrDefault();
            if(account == null){
                account = createAccount(id);
            }
            return account;
        }

        private static UserAccount createAccount(ulong id){
            var newAcc = new UserAccount(){
                userId = id,
                Points = 0,
                EXP = 0
            };

            accounts.Add(newAcc);
            saveAccounts();
            return newAcc;
        }
    }
}
