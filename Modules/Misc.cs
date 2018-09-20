using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using CBot.Core.UserAccounts;
using NReco.ImageGenerator;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using ClosedXML;
using Discord.Rest;

namespace CBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {   
        [Command("say")]
        public async Task say([Remainder]string message){
            var embed = new EmbedBuilder();
            embed.WithTitle(Context.User.Username + " said: ");
            embed.WithDescription(message);
            embed.WithColor(new Color(0, 0, 175));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("randomperson")]
        public async Task getRandomPerson(){
            string json = "";
            using(WebClient client = new WebClient()){
                json = client.DownloadString("https://randomuser.me/api/?nat=US");
            }
            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nat = dataObject.results[0].nat.ToString();
            string gender = dataObject.results[0].gender.ToString();
            string gender2 = new CultureInfo("en-US").TextInfo.ToTitleCase(gender);
            string title = dataObject.results[0].name.title.ToString();
            string title2 = new CultureInfo("en-US").TextInfo.ToTitleCase(title);
            string firstname = dataObject.results[0].name.first.ToString();
            string firstname2 = new CultureInfo("en-US").TextInfo.ToTitleCase(firstname);
            string lastname = dataObject.results[0].name.last.ToString();
            string lastname2 = new CultureInfo("en-US").TextInfo.ToTitleCase(lastname);
            string picture = dataObject.results[0].picture.large.ToString();
            string dateofbirth = dataObject.results[0].dob.date.ToString();
            string age = dataObject.results[0].dob.age.ToString();

            var embed = new EmbedBuilder();
            embed.WithDescription($"Full Name: {title2 + "."} {firstname2} {lastname2}\nNationality: {nat}\nGender: {gender2}\nDate of Birth: {dateofbirth}\nAge: {age}");
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(0, 255, 255));
            embed.WithThumbnailUrl(picture);
            embed.WithTitle("Random Person:");

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("html")]
        public async Task html(string color = "red"){
            string css = "<style>\nh1{\ncolor: " + color + ";\n}\n</style>\n";
            string html = String.Format("<h1>Hello {0}!</h1>", Context.User.Username);
            var converter = new HtmlToImageConverter{
                Width = 500,
                Height = 70
            };
            var jpgBytes = converter.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpgBytes), "hello.jpg");
        }

        [Command("pick")]
        public async Task Pick([Remainder]string message){
            string[] options = message.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            Random rand = new Random();
            string picked = options[rand.Next(0, options.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("I have chosen: ");
            embed.WithDescription(picked);
            embed.WithColor(new Color(0,255,255));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("pewpew")]
        public async Task pewpew([Remainder]string name){
            var embed = new EmbedBuilder();
            embed.WithTitle(Context.User.Username + " pewpew'd");
            embed.WithDescription(String.Format("{0} pewpew'd {1}", Context.User.Username, name));
            embed.WithColor(new Color(0,255,255));
            embed.WithThumbnailUrl("https://i.imgur.com/jamsei6.jpg");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("kill")]
        public async Task kill([Remainder]string Victim){
            String[] how = new String[7];
            how[0] = "by spraying them with a submachine gun!";
            how[1] = "by shooting them 5 times with a pistol!";
            how[2] = "by shaking them multiple times with a knife!";
            how[3] = "by beating them to death with a baseball bat!";
            how[4] = "by hiring a hitman!";
            how[5] = "by running them over!";
            how[6] = "by yeeting them **across the universe**";

            Random rand = new Random();
            string picked = how[rand.Next(0, how.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle(Context.User.Username + " has blood on their hands.");
            embed.WithDescription(String.Format("{0} killed {1} {2}", Context.User.Mention, Victim, picked));
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://myanimelist.cdn-dena.com/s/common/uploaded_files/1453949904-c2ea6ea591f839374da8993f0764f78b.jpeg");
            embed.WithColor(new Color(0,255,255));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("marry")]
        public async Task Marry([Remainder]string person) {
            var embed = new EmbedBuilder();
            embed.WithTitle("Congratulations!");
            embed.WithDescription(String.Format("{0} marries {1}", Context.User.Mention, person));
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://i.pinimg.com/236x/a7/ca/8c/a7ca8c84970f951d08b196bab75a4992--anime-wedding-kirito-asuna.jpg");
            embed.WithColor(new Color(255,105,180));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("divorce")]
        public async Task Divorce([Remainder]string person) {
            var embed = new EmbedBuilder();
            embed.WithTitle("I am sorry :(");
            embed.WithDescription(String.Format("{0} gets a divorce with {1}", Context.User.Mention, person));
            embed.WithCurrentTimestamp();
            embed.WithThumbnailUrl("https://media.giphy.com/media/3fmRTfVIKMRiM/giphy.gif");
            embed.WithColor(new Color(139,0,0));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("stats")]
        public async Task stats([Remainder]string arg = ""){
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            Random rand = new Random();
            int R = rand.Next(0,256);
            int G = rand.Next(0,256);
            int B = rand.Next(0,256);

            var account = UserAccounts.getAccount(target);

            var embed = new EmbedBuilder();
            embed.WithTitle(target + "'s stats: ");
            embed.WithThumbnailUrl(target.GetAvatarUrl());
            embed.WithColor(new Color(R,G,B));
            embed.WithDescription($"Points: {account.Points}\nEXP: {account.EXP}\nLevel: {account.LVL}");
            
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("addPoints")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task addPoints(uint Points, [Remainder]string args = ""){
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            Random rand = new Random();
            int R = rand.Next(0,256);
            int G = rand.Next(0,256);
            int B = rand.Next(0,256);

            var account = UserAccounts.getAccount(target);
            account.Points += Points;
            UserAccounts.saveAccounts();

            var embed = new EmbedBuilder();
            embed.WithTitle("Added points!");
            embed.WithThumbnailUrl(target.GetAvatarUrl());
            embed.WithColor(new Color(R,G,B));
            embed.WithDescription($"Given {target.Username} {Points} points!");
            
            await Context.Channel.SendMessageAsync("", false, embed);
        }
        [Command("addEXP")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task addEXP(uint EXP, [Remainder]string args = ""){
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            Random rand = new Random();
            int R = rand.Next(0,256);
            int G = rand.Next(0,256);
            int B = rand.Next(0,256);

            var account = UserAccounts.getAccount(target);
            account.EXP += EXP;
            UserAccounts.saveAccounts();

            var embed = new EmbedBuilder();
            embed.WithTitle("Added EXP!");
            embed.WithThumbnailUrl(target.GetAvatarUrl());
            embed.WithColor(new Color(R,G,B));
            embed.WithDescription($"Given {target.Username} {EXP} EXP!");
            
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("removeEXP")]
        [RequireUserPermission(GuildPermission.Administrator)]
            public async Task removeEXP(uint EXP, [Remainder]string args = ""){
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            Random rand = new Random();
            int R = rand.Next(0,256);
            int G = rand.Next(0,256);
            int B = rand.Next(0,256);

            var account = UserAccounts.getAccount(target);
            account.EXP -= EXP;
            if(account.EXP < 0) account.EXP = 0;
            UserAccounts.saveAccounts();

            var embed = new EmbedBuilder();
            embed.WithTitle("Stole EXP!");
            embed.WithThumbnailUrl(target.GetAvatarUrl());
            embed.WithColor(new Color(R,G,B));
            embed.WithDescription($"Stole {EXP} from {target.Username}! yeet'd");
            
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("removePoints")]
        [RequireUserPermission(GuildPermission.Administrator)]
            public async Task removePoints(uint Points, [Remainder]string args = ""){
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            Random rand = new Random();
            int R = rand.Next(0,256);
            int G = rand.Next(0,256);
            int B = rand.Next(0,256);

            var account = UserAccounts.getAccount(target);
            account.Points -= Points;
            if(account.Points < 0) account.Points = 0;
            UserAccounts.saveAccounts();

            var embed = new EmbedBuilder();
            embed.WithTitle("Stole points!");
            embed.WithThumbnailUrl(target.GetAvatarUrl());
            embed.WithColor(new Color(R,G,B));
            embed.WithDescription($"Stole {Points} from {target.Username}! yeet'd");
            
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("fuck")]
        public async Task fuck([Remainder]string name){
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? null;
            String[] gifs = new String[17];

            gifs[0] = "https://78.media.tumblr.com/931125762652e49075f26f954049c0da/tumblr_msvseafYc01sflbiso1_1280.gif";
			gifs[1] = "https://78.media.tumblr.com/bd1af323390c9dc5362a56a5ada27a3a/tumblr_p25vpc5DGO1vkkt9ro1_1280.gif";
			gifs[2] = "https://images.sex.com/images/pinporn/2017/07/02/620/17994288.gif";
			gifs[3] = "https://static.hentai-gifs.com/upload/20160703/19/36972/1.gif";
			gifs[4] = "https://78.media.tumblr.com/a39ae50d2ba1ee6441a2d038e37c3732/tumblr_p25vjpiFyU1vkkt9ro1_500.gif";
			gifs[5] = "http://blowjobgif.net/albums/2017/04/06/23/1/hentai-gif-24.gif";
			gifs[6] = "https://www.rencontresanslendemain.net/wp-content/uploads/2018/02/gif-hentai.gif";
			gifs[7] = "http://funkyimg.com/i/2bPEL.gif";
			gifs[8] = "http://25.media.tumblr.com/fb7f15eab9a017323d8f60a9f6295ca7/tumblr_mkrj98SaAR1s5fe8vo1_500.gif";
			gifs[9] = "https://i.pinimg.com/originals/22/46/bc/2246bce7214672b5087de891099c91ef.gif";
			gifs[10] = "https://cdnio.luscious.net/870/lusciousnet_gif-001_267085858.gif";
			gifs[11] = "http://1porngifs.com/wp-content/uploads/2017/03/tumblr_o25uxryNZ81v7p8nno1_500.gif";
			gifs[12] = "http://leagueoflegendshentai.net/wp-content/uploads/2016/01/Lulugif.gif";
			gifs[13] = "https://78.media.tumblr.com/tumblr_maban576Jr1rdw7hvo1_500.gif";
			gifs[14] = "https://i.pinimg.com/originals/39/29/4d/39294de85e73392f78a5973bd32b5c2f.gif";
			gifs[15] = "http://hentai.bestsexphotos.eu/wp-content/uploads/2017/04/tumblr_ogjr4lgzyQ1vkefi9o1_500.gif";
			gifs[16] = "https://thumbs.gfycat.com/DisfiguredUnfinishedEyas-max-14mb.gif";

            Random rand = new Random();
            string picked = gifs[rand.Next(0, gifs.Length)];


            var embed = new EmbedBuilder();
            embed.WithCurrentTimestamp();
            embed.WithImageUrl(picked);
            embed.WithTitle($"{Context.User.Username} fucks {target.Username}! Hot..");
            embed.WithColor(new Color(255,20,147));
            var embed2 = new EmbedBuilder();
            embed2.WithCurrentTimestamp();
            embed2.WithTitle("Error!");
            embed2.WithDescription("You are not allowed to perform this action in a non-nsfw channel!");
            embed2.WithColor(new Color(255, 0, 0));
            if(Context.Channel.IsNsfw == true){
                await Context.Channel.SendMessageAsync("", false, embed);
            }else{
                await Context.Channel.SendMessageAsync("", false, embed2);
            }
        }

        [Command("rule34")]
        public async Task getRandom([Remainder]string tags)
        {
            string randporn = "";
            using (WebClient client = new WebClient())
            {
                randporn = client.DownloadString(String.Format("https://rule34.xxx/index.php?page=dapi&s=post&tags={0}&q=index", tags));
            }
            List<string> links = new List<string>();

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(randporn);

            foreach (XmlNode node in xdoc.DocumentElement)
            {
                string name = node.Attributes[2].InnerText;
                links.Add(name);
            }
            String[] links2 = links.ToArray();

            Random random = new Random();
            string link = links[random.Next(0, links2.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("Here you go, horny cunt!");
            embed.WithCurrentTimestamp();
            embed.WithImageUrl(link);
            embed.WithColor(new Color(255, 255, 255));

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("rule34")]
        public async Task getRandom2([Remainder]string tags)
        {
            string randporn = "";
            using (WebClient client = new WebClient())
            {
                randporn = client.DownloadString("https://rule34.xxx/index.php?page=dapi&s=post&q=index");
            }
            List<string> links = new List<string>();

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(randporn);

            foreach (XmlNode node in xdoc.DocumentElement)
            {
                string name = node.Attributes[2].InnerText;
                links.Add(name);
            }
            String[] links2 = links.ToArray();

            Random random = new Random();
            string link = links[random.Next(0, links2.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("Here you go, horny cunt!");
            embed.WithCurrentTimestamp();
            embed.WithImageUrl(link);
            embed.WithColor(new Color(255, 255, 255));

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("furry")]
        public async Task getRandome621([Remainder]string tags)
        {
            string randporn = "";
            using (WebClient client = new WebClient())
            {
                randporn = client.DownloadString(String.Format("https://furry.booru.org/index.php?page=dapi&s=post&tags={0}&q=index", tags));
            }
            List<string> links = new List<string>();

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(randporn);

            foreach (XmlNode node in xdoc.DocumentElement)
            {
                string name = node.Attributes[2].InnerText;
                links.Add(name);
            }
            String[] links2 = links.ToArray();

            Random random = new Random();
            string link = links[random.Next(0, links2.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("Here you go, horny cunt!");
            embed.WithCurrentTimestamp();
            embed.WithImageUrl(link);
            embed.WithColor(new Color(255, 255, 255));

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("furry")]
        public async Task getRandome6212()
        {
            string randporn = "";
            using (WebClient client = new WebClient())
            {
                randporn = client.DownloadString("https://furry.booru.org/index.php?page=dapi&s=post&q=index");
            }
            List<string> links = new List<string>();

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(randporn);

            foreach (XmlNode node in xdoc.DocumentElement)
            {
                string name = node.Attributes[2].InnerText;
                links.Add(name);
            }
            String[] links2 = links.ToArray();

            Random random = new Random();
            string link = links[random.Next(0, links2.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle("Here you go, horny cunt!");
            embed.WithCurrentTimestamp();
            embed.WithImageUrl(link);
            embed.WithColor(new Color(255, 255, 255));

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("e621")]
        public async Task pic([Remainder]string tags){
            try
            {
                string getpicjson = "";
                using (WebClient userclient = new WebClient())
                {
                    userclient.Headers.Add("user-agent", "CBot/1.0 (by FurriesAreLovenLife on e621)");
                    getpicjson = userclient.DownloadString("https://e621.net/post/index.json?limit=30?tags=" + tags + "?password_hash=692c0e0e658338347dd9b38bb1be5005");
                }
                var avdataObject = JsonConvert.DeserializeObject<dynamic>(getpicjson);

                var random = new Random();
                var imageid = random.Next(0,30);

                string url = avdataObject[imageid].file_url;

                var embed = new EmbedBuilder();
                embed.WithTitle("Here you go, horny cunt!");
                embed.WithImageUrl(url);
                embed.WithCurrentTimestamp();
                embed.WithFooter($"https://e621.net/post/show/{avdataObject[imageid].id}");
                embed.WithColor(new Color(255, 255, 255));

                await ReplyAsync("", false, embed.Build());
            }
            catch(Exception e)
            {
                await ReplyAsync(e.ToString());
            }
        }

        [Command("e621")]
        public async Task e621(){
            try
            {
                string getpicjson = "";
                using (WebClient userclient = new WebClient())
                {
                    userclient.Headers.Add("user-agent", "CBot/1.0 (by FurriesAreLovenLife on e621)");
                    getpicjson = userclient.DownloadString("https://e621.net/post/index.json?limit=30?password_hash=692c0e0e658338347dd9b38bb1be5005");
                }
                var avdataObject = JsonConvert.DeserializeObject<dynamic>(getpicjson);

                var random = new Random();
                var imageid = random.Next(0,30);

                string url = avdataObject[imageid].file_url;

                var embed = new EmbedBuilder();
                embed.WithTitle("Here you go, horny cunt!");
                embed.WithImageUrl(url);
                embed.WithCurrentTimestamp();
                embed.WithFooter($"https://e621.net/post/show/{avdataObject[imageid].id}");
                embed.WithColor(new Color(255, 255, 255));

                await ReplyAsync("", false, embed.Build());
            }
            catch(Exception e)
            {
                await ReplyAsync(e.ToString());
            }
        }

        [Command("kiss")]
        public async Task kiss([Remainder]string name)
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? null;
            String[] gifs = new String[17];

            gifs[0] = "https://cdn.weeb.sh/images/SydfnauPb.gif";
            gifs[1] = "https://thumbs.gfycat.com/NeatMinorAnglerfish-small.gif";
            gifs[2] = "https://gifimage.net/wp-content/uploads/2017/10/cute-anime-kiss-gif-7.gif";
            gifs[3] = "http://33.media.tumblr.com/a8b3e9f706d5a509b09000fd736b9467/tumblr_n3qrm4N3S31siyfwio1_500.gif";
            gifs[4] = "https://media.giphy.com/media/gKvcHvv9NWiOY/source.gif";
            gifs[5] = "https://data.whicdn.com/images/242470319/original.gif";
            gifs[6] = "https://i.imgur.com/eisk88U.gif";
            gifs[7] = "https://i.pinimg.com/originals/22/46/e3/2246e33a30ba421982abb993afe9c1ad.gif";
            gifs[8] = "https://data.whicdn.com/images/231294836/original.gif";
            gifs[9] = "http://gifimage.net/wp-content/uploads/2017/09/anime-kiss-gif-1.gif";
            gifs[10] = "https://78.media.tumblr.com/61519c408b9984dcc807bdefa15f5a18/tumblr_o1henjRfTe1uapp8co1_400.gif";
            gifs[11] = "https://media3.giphy.com/media/hnNyVPIXgLdle/200.gif";
            gifs[12] = "https://78.media.tumblr.com/8f9959555582f3284cd384e4ceabcfea/tumblr_ox862fYY0o1s8v1m3o1_500.gif";
            gifs[13] = "https://gifer.com/i/HgKr.gif";
            gifs[14] = "https://thumbs.gfycat.com/FondEvergreenIcterinewarbler-max-1mb.gif";
            gifs[15] = "https://pa1.narvii.com/5823/3c59d0f9c627923616ca5811f159800016711c45_hq.gif";
            gifs[16] = "https://media1.tenor.com/images/f5167c56b1cca2814f9eca99c4f4fab8/tenor.gif?itemid=6155657";

            Random rand = new Random();
            string picked = gifs[rand.Next(0, gifs.Length)];


            var embed = new EmbedBuilder();
            embed.WithCurrentTimestamp();
            embed.WithImageUrl(picked);
            embed.WithTitle($"{Context.User.Username} kisses {target.Username}! Cute..");
            embed.WithColor(new Color(255, 20, 147));
            await Context.Channel.SendMessageAsync("", embed:embed);
        }

        [Command("EXPToLevel")]
        public async Task EXPToLevel(uint xp){
            uint level = (uint)Math.Sqrt(xp / 75);
            await Context.Channel.SendMessageAsync("The level is " + level);
        }

        [Command("give")]
        public async Task givePoints(uint points, [Remainder]string user){
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? null;

            var Giver = UserAccounts.getAccount(Context.User);
            Giver.Points -= points;
            var Reciever = UserAccounts.getAccount(target);
            Reciever.Points += points;

            var embed = new EmbedBuilder();
            embed.WithCurrentTimestamp();
            embed.WithTitle("Given points!");
            embed.WithThumbnailUrl("https://i.gyazo.com/b1597f074faae7fd6c6b4a27684597fa.png");
            embed.WithDescription($"{Context.User.Username} has given {target.Username} {points}!");
            embed.WithColor(new Color(255,20,147));

            await Context.Channel.SendMessageAsync("", embed:embed);
        }

        [Command("suicide")]
        public async Task CommitDie(){
            if(Context.User.Id == 216548509212475395){
                var embed = new EmbedBuilder();
                embed.WithTitle(Context.User.Username + " has died...");
                embed.WithDescription(String.Format($"{Context.User.Username} has commit suicide..."));
                embed.WithCurrentTimestamp();
                embed.WithThumbnailUrl("https://myanimelist.cdn-dena.com/s/common/uploaded_files/1453949904-c2ea6ea591f839374da8993f0764f78b.jpeg");
                embed.WithColor(new Color(139,0,0));

                await Context.Channel.SendMessageAsync("", embed:embed);
            }else{
                var embed = new EmbedBuilder();
                embed.WithTitle("Error!");
                embed.WithDescription(String.Format($":x: You do not have permission to use this command! Ask Malakai for permission."));
                embed.WithCurrentTimestamp();
                embed.WithColor(new Color(255,0,0));

                await Context.Channel.SendMessageAsync("", embed:embed);
            }
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task kickUser(IGuildUser user, bool warn = false, [Remainder]string reason = "No reason given."){
            ulong channelId = 492224390000541696;
            var channel = Global.client.GetChannel(channelId) as IMessageChannel;

            var embed = new EmbedBuilder();
            embed.WithTitle("User kicked!");
            if(warn == true){embed.WithDescription(String.Format($"{Context.User.Username} kicked {user}.\nReason: {reason} [From Warn]")); }
            else{embed.WithDescription(String.Format($"{Context.User.Username} kicked {user}.\nReason: {reason}"));}
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(255,0,0));
            user.KickAsync(reason);
            await channel.SendMessageAsync("", embed:embed);
        }

        [Command("mute")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task muteUser(IGuildUser user, bool warn = false, [Remainder]string reason = "No reason given."){
            ulong channelId = 492224390000541696;
            var channel = Global.client.GetChannel(channelId) as IMessageChannel; 
            var userAcc = UserAccounts.getAccount((SocketUser)user);
            var embed = new EmbedBuilder();
            
            embed.WithTitle("User muted!");
            if(warn == true){embed.WithDescription(String.Format($"{Context.User.Username} muted {user}.\nReason: {reason} [From Warn]")); }
            else{embed.WithDescription(String.Format($"{Context.User.Username} muted {user}.\nReason: {reason}"));}
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(255,0,0));
            userAcc.isMuted = true;
            await channel.SendMessageAsync("", embed:embed);
        }

        [Command("unmute")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task unmuteUser(IGuildUser user){
            ulong channelId = 492224390000541696;
            var channel = Global.client.GetChannel(channelId) as IMessageChannel; 
            var userAcc = UserAccounts.getAccount((SocketUser)user);
            var embed = new EmbedBuilder();

            embed.WithTitle("User unmuted!");
            embed.WithDescription(String.Format($"{Context.User.Username} unmuted {user}."));
            embed.WithCurrentTimestamp();
            embed.WithColor(new Color(255,0,0));
            userAcc.isMuted = false;
            await channel.SendMessageAsync("", embed:embed);
        }
        
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task banUser(IGuildUser user, bool warn = false, [Remainder]string reason = "No reason given."){

                ulong channelId = 492224390000541696;
                var channel = Global.client.GetChannel(channelId) as IMessageChannel;

                var embed = new EmbedBuilder();
                embed.WithTitle("User banned!");
                if(warn == true){embed.WithDescription(String.Format($"{Context.User.Username} banned {user}.\nReason: {reason} [From Warn]")); }
                else{embed.WithDescription(String.Format($"{Context.User.Username} banned {user}.\nReason: {reason}"));}                
                embed.WithCurrentTimestamp();
                embed.WithColor(new Color(255,0,0));
                user.Guild.AddBanAsync(user, 0, reason);
                await channel.SendMessageAsync("", embed:embed);
        }

        [Command("cmds")]
        public async Task SayCommands(){
            string cmds = "";
            using(WebClient client = new WebClient()){
                cmds = client.DownloadString("https://pastebin.com/raw/3bdjJnwP");
            }
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync("```" + cmds + "```");
        }

        public static Color randomRGB(){
            Random rand = new Random();
            int r = rand.Next(0,256);
            int g = rand.Next(0,256);
            int b = rand.Next(0,256);
            Color color = new Color(r,g,b);
            return color;
        }
    }
}
