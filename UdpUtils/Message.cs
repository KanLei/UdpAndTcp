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


        /// <summary>
        /// 发送方
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 接收方
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// IP 地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 信息类型
        /// </summary>
        public MessageEnum Type { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public decimal FileLength { get; set; }

        /// <summary>
        /// true 为Udp传送文件; false 为Tcp传送文件
        /// </summary>
        public bool Udp { get; set; }
    }
}
