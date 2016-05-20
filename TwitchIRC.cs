using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace TwitchSpammer
{
    class TwitchIRC
    {
        private TcpClient client;

        private StreamReader outputStream;
        private StreamWriter inputStream;

        private String username, channel, message;

        public TwitchIRC(String username, String channel, String token)
        {
            client = new TcpClient("irc.twitch.tv", 6667);
            outputStream = new StreamReader(client.GetStream());
            inputStream = new StreamWriter(client.GetStream());
            this.username = username;
            this.channel = channel;
            inputStream.WriteLine("PASS " + token);
            inputStream.WriteLine("NICK " + username);
            inputStream.WriteLine("USER " + username + " 8 * : " + username);
            inputStream.WriteLine("JOIN #" + channel);
            inputStream.Flush();
        }

        private Boolean send(String message)
        {
            try {
                inputStream.WriteLine(message);
                inputStream.Flush();
                return true;
            } catch
            {
                return false;
            }
        }

        public void setMessage(String message)
        {
            this.message = message;
        }

        public Boolean sendMessage()
        {
            return send(":" + this.username + "!" + this.username + "@" + this.username + ".tmi.twitch.tv PRIVMSG #" + this.channel + " :" + this.message);
        }

        public String readMessage()
        {
            return outputStream.ReadLine();
        }
    }
}
