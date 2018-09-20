using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;

namespace CBot.Core
{
    internal static class RepeatingTimer
    {
        private static Timer Loop;
        private static SocketTextChannel channel;

        internal static Task startTimer()
        {
            channel = Global.client.GetGuild(451461709744570378).GetTextChannel(451461710717517826);

            Loop = new Timer(){
                Interval = SecondsToMilli(5),
                AutoReset = true,
                Enabled = true
            };
            Loop.Elapsed += onTimerTicked;

            return Task.CompletedTask;
        }

        private static void onTimerTicked(object sender, ElapsedEventArgs e){
        }

        

        public static int SecondsToMilli(int second){
            return second * 1000;
        }
    }
}