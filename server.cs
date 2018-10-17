using Discord;
using Discord.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Speedrun_BOT
{
    public class Server
    {
        private Hashtable info = new Hashtable();
        private ArrayList players = new ArrayList();

        private byte[] PACKET_HEADER = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };

        public EmbedBuilder Query(string gameServerIP, int gameServerPort)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Connect(IPAddress.Parse(gameServerIP), gameServerPort);

            byte[] packet_send = new byte[16];
            byte[] cmd = Encoding.ASCII.GetBytes("getstatus");

            Buffer.BlockCopy(PACKET_HEADER, 0, packet_send, 0, PACKET_HEADER.Length);
            Buffer.BlockCopy(cmd, 0, packet_send, PACKET_HEADER.Length, cmd.Length);

            StringBuilder response = new StringBuilder();
            byte[] bufferRecv = new byte[65565];

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(gameServerIP), 0);
            client.Send(packet_send, SocketFlags.None);

            do
            {
                int bytesReceived = client.Receive(bufferRecv);
                response.Append(Encoding.ASCII.GetString(bufferRecv, 0, bytesReceived));
            } while (client.Available > 0);

            string[] raw = response.ToString().Split("\n");
            string rawInfo = raw[1];
            string[] temp = rawInfo.Split("\\");

            for (int i = 1; i < temp.Length; i += 2)
            {
                info.Add(temp[i], Regex.Replace(temp[i + 1], "(\\^)([0-9])", ""));
            }

            for (int i = 2; i < raw.Length; i++)
            {
                string[] rawPlayer = raw[i].Split(" ");
                string name = "";

                if (rawPlayer.Length < 2)
                    continue;

                for (int z = 2; z < rawPlayer.Length; z++)
                {
                    if (z != rawPlayer.Length)
                        name += rawPlayer[z] + " ";
                    else
                        name += rawPlayer[z];
                }
               
                Player player = new Player(rawPlayer[1], rawPlayer[0], name.Replace("\"", ""));
                players.Add(player);
            }

            EmbedBuilder em = new EmbedBuilder();
            em.Title = info["sv_hostname"].ToString();
            em.AddField("Map", info["mapname"].ToString(), false);
            em.Color = new Color(164, 22, 248);

            string playerDetails = "No players Online";

            if (players.Count > 0)
            {
                playerDetails = "";

                foreach (Player p in players)
                {
                    playerDetails += p.nickname + "\n";
                }
            }

            em.AddField("Players", playerDetails, false);
            em.Build();

            return em;
        }
    }

    public class Player
    {
        public int points;
        public int ping;
        public string nickname;

        public Player(string ping, string points, string nickname)
        {
            this.ping = int.Parse(ping);
            this.points = int.Parse(points);
            this.nickname = nickname;
        }
    }
}