using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace UdpUtils
{
    /// <summary>
    /// 自定义 Udp Server
    /// </summary>
    public class Server
    {
        private static UdpClient udpClient;

        public static Action StartServerNotify;
        public static Action<Message> ReceiveMessageNotify;
        public static Action<Message> SignInNotify;
        public static Action<Message> SignOutNotify;


        /// <summary>
        /// 启动服务器
        /// </summary>
        public static void StartServer(string ip, int port)
        {
            IPEndPoint localPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            udpClient = new UdpClient(localPoint);

            if (StartServerNotify != null)
            {
                StartServerNotify();    // Notify the server
            }

            /*
             * 注意这里使用的是线程池线程，默认是后台线程，
             * 如果使用前台线程，则会阻塞服务器端正常关闭。
             */
            Task.Factory.StartNew(RecieveClient);  // 开始接收消息,
        }

        /// <summary>
        /// 接收客户端的请求
        /// </summary>
        static void RecieveClient()
        {
            try
            {
                while (true)
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] datagram = udpClient.Receive(ref endPoint);

                    string message = Encoding.Unicode.GetString(datagram);
                    Message receiveMessage = JsonConvert.DeserializeObject<Message>(message);
                    receiveMessage.IpAddress = endPoint.Address.ToString();
                    receiveMessage.Port = endPoint.Port;
                    ProcessMessage(receiveMessage, endPoint);
                }
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
            }
            finally
            {
                udpClient.Close();
            }
        }

        /// <summary>
        /// use different logic to deal with different message
        /// </summary>
        /// <param name="type">message type</param>
        static void ProcessMessage(Message msg, IPEndPoint remoteEndPoint)
        {
            ProcessMessage processMessage = new ProcessMessage(msg, udpClient);

            switch (msg.Type)
            {
                case MessageEnum.SIGN_IN:
                    processMessage.SignIn_Server(remoteEndPoint);
                    break;
                case MessageEnum.SIGN_OUT:
                    processMessage.SignOut_Server(remoteEndPoint);
                    break;
                case MessageEnum.CHAT:
                    //processMessage.Chat();
                    break;
                default:
                    Log.Write(String.Format("MessageEnum doesn't have {0}", msg.Type));
                    break;
            }
        }
    }
}
