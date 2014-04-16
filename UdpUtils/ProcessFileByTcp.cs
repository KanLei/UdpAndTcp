/*
 * 使用 TCP 传输文件
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UdpUtils
{
    internal class ProcessFileByTcp
    {
        private const int MAXSIZE = 1024;


        /// <summary>
        /// 向客户端异步发送文件
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="filePath"></param>
        public async void SendToClientFileAsync(string ip, int port, string filePath, Message msg)
        {
            // 新建 Udp 用于通知服务端，客户端要发送文件了
            UdpClient sendClient = new UdpClient();
            TcpClient localClient = new TcpClient();

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                msg.Type = MessageEnum.FILE;  // 设置发送文件标识
                msg.FileLength = fileInfo.Length;

                msg.FileName = Regex.Match(filePath, @"\\([^\\]+\.[^\\]+)").Groups[1].Value;  // 获取文件名

                byte[] datagram = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(msg));
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                /*
                 * 向‘原’远程客户端发送请求传送文件的请求,
                 * 接收‘新’远程客户端的响应,获取传送文件的 Tcp 服务器 IP 和 Port
                 * 
                 * 注：Udp 客户端用于发送通知消息，Tcp 客户端用于发送文件
                 */
                sendClient.Send(datagram, datagram.Length, endPoint);

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] bytes = sendClient.Receive(ref remoteEndPoint);  // 阻塞直到接收到远程客户端的响应

                // 获取服务器的 IP 和 Port
                string serverIPAndPort = Encoding.Unicode.GetString(bytes);
                Message receiveIPAndPort = JsonConvert.DeserializeObject<Message>(serverIPAndPort);

                // 连接到服务器
                localClient.Connect(IPAddress.Parse(receiveIPAndPort.IpAddress), receiveIPAndPort.Port);

                /*
                 * 开始发送文件
                 */
                byte[] buffer = new byte[MAXSIZE];
                using (var localStream = localClient.GetStream())
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int percent = 0;
                    int count = 0;
                    do
                    {
                        count = fs.Read(buffer, 0, buffer.Length);  // 0 表示读入到 buffer 中的起始位置

                        if (Client.SendFileProgressNotify != null)
                        {
                            Client.SendFileProgressNotify(String.Format("{0:F2}%", (percent += count) / msg.FileLength * 100));
                        }
                        await Task.Delay(10);
                        await localStream.WriteAsync(buffer, 0, count);
                    } while (count > 0);
                }
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
            }
            finally  // 关闭相关资源
            {
                if (sendClient != null)
                {
                    sendClient.Close();
                }
                if (localClient != null && localClient.Connected)
                {
                    localClient.Close();
                }
            }
        }



        /// <summary>
        /// 接收客户端发送的文件
        /// </summary>
        public async void ReceiveClientFile(Message msg, IPEndPoint remtoe)
        {
            // 新建 Udp 协议用于接收文件
            UdpClient receiveFile = new UdpClient();

            const int PORT = 8555;
            var hostName = Dns.GetHostName();
            var localadd = Dns.GetHostEntry(hostName).AddressList.Where(i => i.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();

            TcpListener listener = null;
            TcpClient localClient = null;
            try
            {
                listener = new TcpListener(localadd, PORT);
                listener.Start();  // 服务器开启监听

                /*
                 *  响应远程客户端发送文件的请求
                 *  通知远程客户端将文件发送至 TcpListener 端口
                 */
                Message send = new Message { Type = MessageEnum.FILE, IpAddress = localadd.ToString(), Port = PORT };
                byte[] buffer = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(send));
                receiveFile.Send(buffer, buffer.Length, remtoe);  // 响应远程客户端

                localClient = listener.AcceptTcpClient();  // 等待远程客户端连接

                // 接收并保存到文件
                using (var remoteStream = localClient.GetStream())
                using (FileStream fs = new FileStream(msg.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    int percent = 0;
                    int count = 0;
                    byte[] datagram = new byte[MAXSIZE];
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                    do
                    {
                        count = remoteStream.Read(datagram, 0, datagram.Length);
                        await fs.WriteAsync(datagram, 0, count);  // 0 表示从 datagram 起始处写入到 fs 流中

                        if (Client.ReceiveFileProgressNotify != null)
                        {
                            Client.ReceiveFileProgressNotify(String.Format("{0:F2}%", (percent += count) / msg.FileLength * 100));
                        }

                    } while (count > 0);
                }

            }
            catch (Exception e)
            {
                Log.Write(e.Message);
            }
            finally  // 关闭相关资源
            {
                if (receiveFile != null)
                {
                    receiveFile.Close();
                }
                if (localClient != null && localClient.Connected)
                {
                    localClient.Close();
                }
                if (listener != null)
                {
                    listener.Stop();
                }
            }
        }
    }
}
