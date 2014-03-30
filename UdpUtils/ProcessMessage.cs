using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpUtils
{
    /// <summary>
    /// process message
    /// </summary>
    internal class ProcessMessage
    {
        public Message msg { get; set; }
        private static List<Message> messages = new List<Message>();
        private static readonly Object obj1 = new Object();
        private static readonly Object obj2 = new Object();
        private UdpClient udpClient;
        byte[] datagram;
        string text;

        public ProcessMessage() { }

        public ProcessMessage(Message msg, UdpClient udpClient)
        {
            this.msg = msg;
            this.udpClient = udpClient;

            datagram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(msg));
            text = String.Format("{0}-{1}:{2}-{3}", msg.FromUserName, msg.IpAddress, msg.Port, msg.Type);
        }

        /// <summary>
        /// 通知服务器有新客户端登陆
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        internal void SignIn_Server(IPEndPoint remoteEndPoint)
        {
            if (Server.SignInNotify != null)
            {
                Server.SignInNotify(msg);
            }

            lock (obj1)  // prevent more than one client run coincide
            {
                // send current user to other online users
                messages.AsParallel().ForAll(x =>
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(x.IpAddress), x.Port);
                    udpClient.BeginSend(datagram, datagram.Length, endPoint, null, null);
                });

                // send other online users to current user
                messages.AsParallel().ForAll(x =>
                {
                    byte[] dgram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(x));
                    udpClient.Send(dgram, dgram.Length, remoteEndPoint);
                });

                messages.Add(this.msg);
            }
        }


        /// <summary>
        /// 通知服务器有新客户端退出
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        internal void SignOut_Server(IPEndPoint remoteEndPoint)
        {
            if (Server.SignOutNotify != null)
            {
                Server.SignOutNotify(msg);
            }

            lock (obj2)  // prevent more than one client run coincide
            {
                // remove current user
                messages.Remove(messages.Find(x => x.IpAddress == msg.IpAddress && x.Port == msg.Port));

                // notify other online users
                messages.AsParallel().ForAll(x =>
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(x.IpAddress), x.Port);
                    udpClient.BeginSend(datagram, datagram.Length, endPoint, null, null);
                });
            }
        }

        internal void Chat()
        {

        }

    }
}
