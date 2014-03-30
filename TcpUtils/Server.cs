using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpUtils
{
    /// <summary>
    /// Use for Tcp Server
    /// </summary>
    public class Server
    {
        private const int BYTESSIZE = 1024;
        private static TcpListener listener;
        private static List<TcpClient> clients = new List<TcpClient>();

        /// <summary>
        /// Start listen occur when StartListen run.
        /// </summary>
        public static event Action StartListenNotify;

        public static event Action<string> ReceiveMessageNotify;

        // Notify subscribe user
        private static void StartListen()
        {
            if (StartListenNotify != null)
            {
                StartListenNotify();
            }
        }

        /// <summary>
        /// Server start listen the Ip and Port
        /// </summary>
        public static void StartListen(string ip, int port)
        {
            listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();

            StartListen();  // Notify

            new Task(() =>
            {
                while (true)
                {
                    TcpClient remoteClient = listener.AcceptTcpClient();
                    clients.Add(remoteClient);
                    new Thread(StartReceive).Start(remoteClient);
                }
            }).Start();

            // make sure at least has one client
            while (clients.Count == 0) ;
        }

        private static void StartReceive(Object remoteClient)
        {
            while (true)
            {
                new Thread(ReceiveMessage).Start(remoteClient);
            }
        }
        private static void ReceiveMessage(Object remoteClient)
        {
            byte[] buffer = new byte[BYTESSIZE];
            TcpClient client = remoteClient as TcpClient;
            client.GetStream().Read(buffer, 0, BYTESSIZE);
            string receiveMessage = Encoding.Unicode.GetString(buffer);

            if (ReceiveMessageNotify != null)
            {
                ReceiveMessageNotify(receiveMessage);
            }
        }

        /// <summary>
        /// Send Message To Remote Client
        /// </summary>
        /// <param name="text"></param>
        public static void SendMessage(string text)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(text);
            foreach (TcpClient client in clients)
            {
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
        }
    }
}
