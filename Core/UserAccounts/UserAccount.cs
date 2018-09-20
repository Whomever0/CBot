using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBot.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong userId{get;set;}
        public uint Points{get; set;}
        public uint EXP{get; set;}
        public uint LVL{get{return (uint)Math.Sqrt(EXP / 75);}}
        public bool isMuted{get;set;}
    }
}
