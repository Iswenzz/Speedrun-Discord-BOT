using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Speedrun_BOT
{
    public class Commands : ModuleBase
    {
        [Command("help")]
        public async Task Help()
        {
            EmbedBuilder em = new EmbedBuilder();
            em.ThumbnailUrl = "https://cdn.discordapp.com/icons/335075122467700740/8152834be097199cff8d46a2ae1e5588.png";
            em.Color = new Color(164, 22, 248);
            em.Title = "Help Commands";
            em.AddField("!times <map name> <speed> <way>", "Show map times: <speed> = 210 or 190, <way> is ns0 ns1 ns2.. or s0 s1 s2..", false);
            em.AddField("!pb <discord name>", "Show personal best: <discord name> = @Iswenzz#3906", false);
            em.AddField("!speedrun", "Show the current map and players connected in SR Speedrun", false);
            em.Build();

            await Context.Channel.SendMessageAsync("", false, em);
        }

        [Command("times")]
        public async Task Times(string map, string speed, string way)
        {
            MapTimes times = new MapTimes();
            await Context.Channel.SendMessageAsync("", false, times.sendTimes(map, speed, way));
        }

        [Command("speedrun")]
        public async Task Speedrun()
        {
            Server serv = new Server();
            await Context.Channel.SendMessageAsync("", false, serv.Query("213.32.18.205", 28960));
        }
    }
}