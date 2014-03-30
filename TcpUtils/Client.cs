using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpUtils
{
    /// <summary>
    /// Use for Tcp Client
    /// </summary>
    public class Client
    {
        private const int BYTESSIZE = 1024;
        private static TcpClient localClient;

        /// <summary>
        /// Start Connection Event
        /// </summary>
        public static event Action StartConnectionNotify;

        /// <summary>
        /// Receive Message Event
        /// </summary>
        public static event Action<string> ReceiveMessageNotify;

        // Notify subscribe user
        private static void ConnectionNotify()
        {
            if (StartConnectionNotify != null)
            {
                StartConnectionNotify();
            }
        }

        /// <summary>
        /// Client start connect the Ip and Port
        /// </summary>
        public static void StartConnection(string ip, int port)
        {
            localClient = new TcpClient();
            localClient.Connect(ip, port);

            ConnectionNotify();  // Notify

            new Task(StartReceive).Start();
        }

        private static void StartReceive()
        {
            while (true)
            {
                new Task(ReceiveMessage).Start();
            }
        }

        private static void ReceiveMessage()
        {
            byte[] buffer = new byte[BYTESSIZE];
            localClient.GetStream().Read(buffer, 0, BYTESSIZE);
            string receiveMessage = Encoding.Unicode.GetString(buffer);

            if (ReceiveMessageNotify != null)
            {
                ReceiveMessageNotify(receiveMessage);
            }
        }

        /// <summary>
        /// Send Message To Remote Client
        /// </summary>
        public static void SendMessage(string text)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(text);
            localClient.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}
