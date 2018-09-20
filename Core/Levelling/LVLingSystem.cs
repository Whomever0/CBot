using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using CBot.Modules;
using System.Timers;

namespace CBot.Core.Levelling
{
    internal static class LVLingSystem{
        public static bool cooldownOver = false;
        private static Timer Loop;
        public static Task startTimer(){
            Loop = new Timer(){
                Interval = RepeatingTimer.SecondsToMilli(5),
                AutoReset = false,
                Enabled = true
            };
            Loop.Elapsed += onTimerTicked;
            return Task.CompletedTask;
        }

        private static void onTimerTicked(object sender, ElapsedEventArgs e){
            cooldownOver = true;
        }
        
        internal static void messageSent(SocketGuildUser user, SocketTextChannel channel){
            startTimer();
            if(cooldownOver == true){
                cooldownOver = false;
                var userAccount = UserAccounts.UserAccounts.getAccount(user);
                uint oldLvl = userAccount.LVL;
                userAccount.EXP += 50;
                UserAccounts.UserAccounts.saveAccounts();
                uint newLvl = userAccount.LVL;

                if(oldLvl != userAccount.LVL){
                    //Leveled up
                    userAccount.Points += 100;
                    UserAccounts.UserAccounts.saveAccounts();

                    var embed = new EmbedBuilder();
                    embed.WithColor(Misc.randomRGB());
                    embed.WithTitle($"{user.Username} has leveled up!");
                    embed.WithDescription($"Level: {newLvl}\nEXP: {userAccount.EXP}\nPoints: {userAccount.Points}");
                    embed.WithThumbnailUrl(user.GetAvatarUrl());

                    channel.SendMessageAsync("", embed:embed);
                }
            }else{
                Console.WriteLine("nono");
            }
        }
    }
}
