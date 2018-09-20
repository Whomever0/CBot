using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using CBot.Core;
using CBot.Modules;

namespace CBot
{
    class Program
    {
        DiscordSocketClient client;
        cmdHandler handler;

        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            Console.Title = "Discord Bot";
            if (Configuration.bot.token == "" || Configuration.bot.token == null)
            {
                return;
            }
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 20
            });
            client.Log += Log;
            client.Ready += RepeatingTimer.startTimer;
            client.MessageDeleted += onMsgDelete;
            handler = new cmdHandler();
            await client.LoginAsync(TokenType.Bot, Configuration.bot.token);
            await client.StartAsync();
            Global.client = this.client;
            await client.SetGameAsync("with your little sister");
            await handler.InitializeAsync(client);
            await Task.Delay(-1);
        }

        private async Task onMsgDelete(Cacheable<IMessage, ulong> cache, ISocketMessageChannel chnl)
        {
            Console.WriteLine("User deleted message.");
            ulong channelId = 492224390000541696;
            ulong guildId = 451461709744570378;
            var guild = client.GetGuild(guildId);
            var channel = guild.GetTextChannel(channelId);

            var embed = new EmbedBuilder();
            embed.WithTitle($"{cache.Value.Author} deleted message.");
            embed.WithDescription($"{cache.Value.Content}\n In channel {cache.Value.Channel.Name}");
            embed.WithColor(new Color(255, 20, 147));

            try
            {
                await channel.SendMessageAsync("", embed: embed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
