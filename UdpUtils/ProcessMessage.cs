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
        private static List<Message> clients = new List<Message>();  // 用于保存在线客户端的集合
        private UdpClient udpClient;
        byte[] datagram;
        string text;
        public Message msg { get; set; }

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


            // send current user to other online users
            clients.AsParallel().ForAll(x =>
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(x.IpAddress), x.Port);
                udpClient.BeginSend(datagram, datagram.Length, endPoint, null, null);
            });

            // send other online users to current user
            clients.AsParallel().ForAll(x =>
            {
                byte[] dgram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(x));
                udpClient.Send(dgram, dgram.Length, remoteEndPoint);
            });

            clients.Add(this.msg);
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

            // remove current user
            clients.Remove(clients.Find(x => x.IpAddress == msg.IpAddress && x.Port == msg.Port));

            // notify other online users
            clients.AsParallel().ForAll(x =>
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(x.IpAddress), x.Port);
                udpClient.BeginSend(datagram, datagram.Length, endPoint, null, null);
            });
        }

        internal void Chat()
        {

        }

    }
}
