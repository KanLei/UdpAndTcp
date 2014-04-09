using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;

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
        public static Action<string> ReceiveFileProgressNotify;  // 接收文件进度提示
        public static Action<string> SendFileProgressNotify;     // 发送文件进度提示


        /// <summary>
        /// 向服务器异步发送信息
        /// </summary>
        public static async Task SendToServerAsync(string ip, int port, Message msg)
        {
            byte[] datagram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(msg));
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            await udpClient.SendAsync(datagram, datagram.Length, endPoint);

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
        /// 向客户端异步发送信息
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <param name="msg">Message</param>
        public static async void SendToClientAsync(string ip, int port, Message msg)
        {
            byte[] datagram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(msg));
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            await udpClient.SendAsync(datagram, datagram.Length, endPoint);
        }


        /// <summary>
        /// 接收消息
        /// </summary>
        private static void RecieveClient()
        {
            try
            {
                while (true)
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] datagram = udpClient.Receive(ref endPoint);

                    string message = Encoding.Unicode.GetString(datagram);
                    Message receiveMessage = JsonConvert.DeserializeObject<Message>(message);

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
        /// 发送文件到远程客户端
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="msg">信息内容</param>
        public static void SendFileToClient(string ip, int port, string filePath, Message msg)
        {
            ProcessFile process = new ProcessFile();
            process.SendToClientFileAsync(ip, port, filePath, msg);
        }


        /// <summary>
        /// 从远程客户端接收文件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="remtoe"></param>
        private static void ReceiveClientFile(Message msg, IPEndPoint remtoe)
        {
            ProcessFile process = new ProcessFile();
            process.ReceiveClientFile(msg, remtoe);
        }



        /// <summary>
        /// use different logic to deal with different message
        /// </summary>
        /// <param name="type">message type</param>
        private static void ProcessMessage(Message msg, IPEndPoint remote)
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
                case MessageEnum.FILE:
                    ReceiveClientFile(msg, remote);
                    break;
                default:
                    Log.Write(String.Format("MessageEnum doesn't have {0}", msg.Type));
                    break;
            }
        }
    }
}
