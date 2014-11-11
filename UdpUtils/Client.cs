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
            await udpClient.SendAsync(datagram, datagram.Length, endPoint).ConfigureAwait(false);

            if (StartClientNotify != null)
            {
                StartClientNotify();    // Notify the client
            }

            // 发送下线消息，则不再监听消息
            if (msg.Type != MessageEnum.SIGN_OUT)
            {
                await RecieveClient();
            }
        }

        /// <summary>
        /// 向客户端异步发送信息
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <param name="msg">Message</param>
        public static async Task SendToClientAsync(string ip, int port, Message msg)
        {
            byte[] datagram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(msg));
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            await udpClient.SendAsync(datagram, datagram.Length, endPoint).ConfigureAwait(false);
        }


        /// <summary>
        /// 接收消息
        /// </summary>
        private static async Task RecieveClient()
        {
            try
            {
                while (true)
                {
                    //IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                    //byte[] datagram =  udpClient.Receive(ref endPoint);
                    UdpReceiveResult result = await udpClient.ReceiveAsync();

                    string message = Encoding.Unicode.GetString(result.Buffer);
                    Message receiveMessage = JsonConvert.DeserializeObject<Message>(message);

                    await ProcessMessage(receiveMessage, result.RemoteEndPoint);

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
        public static async Task SendFileToClient(string ip, int port, string filePath, Message msg, bool udp = true)
        {
            if (udp)
            {
                msg.Udp = true;  // Udp 传输文件
                ProcessFileByUdp process = new ProcessFileByUdp();
                await process.SendToClientFileAsync(ip, port, filePath, msg).ConfigureAwait(false);
            }
            else
            {
                msg.Udp = false;  // Tcp 传输文件
                ProcessFileByTcp process = new ProcessFileByTcp();
                await process.SendToClientFileAsync(ip, port, filePath, msg).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// 从远程客户端接收文件
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="remtoe">IPEndPoint</param>
        private static async Task ReceiveClientFile(Message msg, IPEndPoint remtoe)
        {
            if (msg.Udp)
            {
                ProcessFileByUdp process = new ProcessFileByUdp();
                await process.ReceiveClientFile(msg, remtoe);
            }
            else
            {
                ProcessFileByTcp process = new ProcessFileByTcp();
                await process.ReceiveClientFile(msg, remtoe);
            }
        }



        /// <summary>
        /// use different logic to deal with different message
        /// </summary>
        /// <param name="type">message type</param>
        private static async Task ProcessMessage(Message msg, IPEndPoint remote)
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
                    await ReceiveClientFile(msg, remote);
                    break;
                default:
                    Log.Write(String.Format("MessageEnum doesn't have {0}", msg.Type));
                    break;
            }
        }
    }
}
