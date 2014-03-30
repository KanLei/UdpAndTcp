using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpUtils
{
    /// <summary>
    /// 自定义 Udp Client
    /// </summary>
    public class Client
    {
        /*
         * 在这里初始化，是为了确保有唯一的 udpClient，即客户端唯一的 IP 和 Port,
         * 当第一次请求登陆的时候就已经确定了,不会因为登陆和退出两次操作而生成两个不同的 udpClient.
         */
        private static UdpClient udpClient = new UdpClient();

        public static Action StartClientNotify;
        public static Action<Message> ReceiveMessageNotify;
        public static Action<Message> SignInNotify;
        public static Action<Message> SignOutNotify;

        /// <summary>
        /// 向服务器异步发送信息，并返回 EndPoint 信息
        /// </summary>
        public static async Task SendToServerAsync(string ip, int port, Message msg)
        {
            byte[] datagram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(msg));
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            udpClient.SendAsync(datagram, datagram.Length, endPoint);

            if (StartClientNotify != null)
            {
                StartClientNotify();    // Notify the client
            }

            // 发送下线消息，则不再监听消息
            if (msg.Type != MessageEnum.SIGN_OUT)
            {
                new Task(RecieveClient).Start();
            }
        }

        /// <summary>
        /// 异步发送聊天信息
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <param name="msg">Message</param>
        public static async void SendToClientAsync(string ip, int port, Message msg)
        {
            byte[] datagram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(msg));
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            udpClient.SendAsync(datagram, datagram.Length, endPoint);
        }

        static void RecieveClient()
        {
            while (true)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] datagram = udpClient.Receive(ref endPoint);

                string message = Encoding.Unicode.GetString(datagram);
                Message receiveMessage = JsonConvert.DeserializeObject<Message>(message);
                ProcessMessage(receiveMessage);
            }
        }

        /// <summary>
        /// use different logic to deal with different message
        /// </summary>
        /// <param name="type">message type</param>
        static void ProcessMessage(Message msg)
        {
            ProcessMessage processMessage = new ProcessMessage(msg, udpClient);

            switch (msg.Type)
            {
                case MessageEnum.SIGN_IN:
                    if (SignInNotify != null)
                    {
                        SignInNotify(msg);
                    }
                    break;
                case MessageEnum.SIGN_OUT:
                    if (SignOutNotify != null)
                    {
                        SignOutNotify(msg);
                    }
                    break;
                case MessageEnum.CHAT:
                    if (ReceiveMessageNotify != null)
                    {
                        ReceiveMessageNotify(msg);  // 通知客户端收到聊天信息
                    }
                    break;
                default:
                    Log.Write(String.Format("MessageEnum doesn't have {0}", msg.Type));
                    break;
            }
        }
    }
}
