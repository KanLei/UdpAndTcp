using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UdpUtils
{
    /// <summary>
    /// construct a mesage
    /// </summary>
    public class Message
    {
        public Message() { }

        /// <summary>
        /// Connect or disconnect to server
        /// </summary>
        /// <param name="fromUserName">my user name</param>
        /// <param name="type">chat | signin | signout</param>
        public Message(string fromUserName, MessageEnum type)
        {
            this.FromUserName = fromUserName;
            this.Type = type;
        }

        /// <summary>
        /// send a chat message
        /// </summary>
        public Message(string fromUserName, string toUserName, string content, MessageEnum type)
            : this(fromUserName, type)
        {
            this.ToUserName = ToUserName;
            this.Content = content;
            this.Type = type;
            this.SendTime = DateTime.Now;
        }

        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Content { get; set; }
        public MessageEnum Type { get; set; }
        public DateTime SendTime { get; set; }
    }
}
