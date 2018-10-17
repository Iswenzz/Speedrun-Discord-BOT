using Discord;
using System;
using System.IO;

namespace Speedrun_BOT
{
    class MapTimes
    {
        public EmbedBuilder sendTimes(string map, string speed, string way)
        {
            StreamReader reader = new StreamReader(map + "_fastesttimes_" + way + "_" + speed + ".txt");
            string text = reader.ReadToEnd();
            string[] line = text.Split('\n');

            EmbedBuilder em = new EmbedBuilder();
            em.ThumbnailUrl = "https://cdn.discordapp.com/icons/335075122467700740/8152834be097199cff8d46a2ae1e5588.png";
            em.Color = new Color(164, 22, 248);
            em.Title = map + " " + speed + " " + way;
            em.Description = "\n";

            for (int i = 0; i < line.Length; i++)
            {
                if (i == 20)
                    break;

                string[] tkn = line[i].Split('\\');

                em.Description += "#" + (i + 1) + "‌‌ ‌‌ ‌‌ ‌‌ ‌‌ ‌‌‌‌ ‌‌ " 
                    + RealTime(Convert.ToInt32(tkn[2])) + "‌‌ ‌‌ ‌‌ ‌‌ ‌‌ ‌‌‌‌ ‌‌ " + tkn[3] + "\n";
            }

            em.Build();
            return em;
        }

        private static string RealTime(int time)
        {
            int original = time;
            int miliseconds = time;
            int minutes = time / 60000;
            miliseconds = miliseconds % 60000;
            int seconds = miliseconds / 1000;
            miliseconds = miliseconds % 1000;

            return minutes + ":" + seconds + "." + miliseconds;
        }
    }
}