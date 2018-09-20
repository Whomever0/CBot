using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Discord;
using CBot.Core.UserAccounts;
using CBot.Core.Levelling;

namespace CBot
{
    class cmdHandler
    {
        DiscordSocketClient client;
        CommandService service;

        public async Task InitializeAsync(DiscordSocketClient client){
            this.client = client;
            service = new CommandService();
            await service.AddModulesAsync(Assembly.GetEntryAssembly());
            this.client.MessageReceived += handleCmdAsync;
        }
        
        private async Task handleCmdAsync(SocketMessage sm){
            var msg = sm as SocketUserMessage;
            if(msg == null) return;
            var context = new SocketCommandContext(this.client, msg);
            if(context.User.IsBot) return;

            var userAcc = UserAccounts.getAccount(context.User);
            if(userAcc.isMuted){
                await context.Message.DeleteAsync();
                return;
            }

            LVLingSystem.messageSent((SocketGuildUser)context.User, (SocketTextChannel)context.Channel);

            int argPos = 0;
            if(msg.HasStringPrefix(Configuration.bot.prefix, ref argPos) || msg.HasMentionPrefix(client.CurrentUser, ref argPos) || msg.HasStringPrefix(Configuration.bot.prefix2, ref argPos)){
                var result = await service.ExecuteAsync(context, argPos);
                if(!result.IsSuccess && result.Error != CommandError.UnknownCommand){
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
