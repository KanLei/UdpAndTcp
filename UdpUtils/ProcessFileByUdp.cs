/*
 * 使用 UDP 传输文件
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
    internal class ProcessFileByUdp
    {
        private const int MAXSIZE = 1024;


        /// <summary>
        /// 向客户端异步发送文件
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="filePath"></param>
        public async Task SendToClientFileAsync(string ip, int port, string filePath, Message msg)
        {
            // 新建 Udp 用于发送文件
            var sendClient = new UdpClient();

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
                 * 接收‘新’远程客户端的响应,获取传送文件的端口
                 * 
                 * 注：原远程客户端用于发送消息，新远程客户端用于发送文件
                 */
                await sendClient.SendAsync(datagram, datagram.Length, endPoint);

                //IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                UdpReceiveResult result = await sendClient.ReceiveAsync().ConfigureAwait(false);   // 阻塞直到接收到远程客户端的响应

                /*
                 * 开始发送文件
                 */
                byte[] buffer = new byte[MAXSIZE];
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1, true))
                {
                    int percent = 0;
                    int count = 0;
                    while ((count = await fs.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                    {
                        //await Task.Delay(10);
                        await sendClient.SendAsync(buffer, count, result.RemoteEndPoint);

                        if (Client.SendFileProgressNotify != null)
                        {
                            Client.SendFileProgressNotify(String.Format("{0:F2}%", (percent += count) / msg.FileLength * 100));
                        }
                    }
                    sendClient.Close();
                }
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
            }
        }



        /// <summary>
        /// 接收客户端发送的文件
        /// </summary>
        public async Task ReceiveClientFile(Message msg, IPEndPoint remtoe)
        {
            // 新建 Udp 协议用于接收文件
            var receiveFile = new UdpClient();

            try
            {

                /*
                 *  响应远程客户端发送文件的请求
                 *  通知远程客户端将文件发送至此端口
                 */
                Message send = new Message { Type = MessageEnum.FILE };
                byte[] buffer = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(send));
                await receiveFile.SendAsync(buffer, buffer.Length, remtoe);  // 响应远程客户端

                // 接收并保存到文件
                using (FileStream fs = new FileStream(msg.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 1, true))
                {
                    int percent = 0;
                    byte[] datagram = new byte[MAXSIZE];
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

                    while ((datagram = (await receiveFile.ReceiveAsync().ConfigureAwait(false)).Buffer).Length > 0)
                    {
                        await fs.WriteAsync(datagram, 0, datagram.Length);  // 0 表示从 datagram 起始处写入到 fs 流中

                        if (Client.ReceiveFileProgressNotify != null)
                        {
                            Client.ReceiveFileProgressNotify(String.Format("{0:F2}%", (percent += datagram.Length) / msg.FileLength * 100));
                        }

                    }
                    receiveFile.Close();
                }
            }
            catch (Exception e)
            {
                Log.Write(e.Message);
            }
        }
    }
}
