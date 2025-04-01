using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkingFinalProj
{
    class TcpServer
    {
        private static byte[] buffer = new byte[1024];
        private static byte[] sendBuffer = new byte[1024];
        private static Socket server;
        private static Socket udpServer;
        private static string sendMsg = "";

        private static List<Socket> clientSockets = new List<Socket>();
        private static List<EndPoint> udpClientSockets = new List<EndPoint>();

        static string _ip;

        public void Start()
        {


            Console.WriteLine("Starting Server...");
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));

            server.Listen(32);

            server.BeginAccept(new AsyncCallback(AcceptCallback), null);


            Thread sendThread = new Thread(new ThreadStart(SendLoop));
            //sendThread.Start();



        }

        private static void AcceptCallback(IAsyncResult result)
        {
            Socket socket = server.EndAccept(result);
            Console.WriteLine("Client connected!!");

            clientSockets.Add(socket);

            socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);

            server.BeginAccept(new AsyncCallback(AcceptCallback), null);

        }


        private static void ReceiveCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;

            int rec = socket.EndReceive(result);
            //byte[] data = new byte[rec];
            //Array.Copy(buffer, data, rec)

            string msg = Encoding.ASCII.GetString(buffer, 0, rec);


            if (msg == "quit")
            {
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                return;
            }

            else
            {
                //this is where you should use a mutex
                sendMsg += (" " + msg);

                Console.WriteLine("Received Message: " + msg);
                Console.WriteLine("From: " + socket.RemoteEndPoint.ToString());

                sendBuffer = Encoding.ASCII.GetBytes(sendMsg);




                //Send updates to all clients in the list

                foreach (var sockets in clientSockets)
                {

                    if (sockets.RemoteEndPoint.ToString() != socket.RemoteEndPoint.ToString())
                    {
                        Console.WriteLine("Sent to: " + sockets.RemoteEndPoint.ToString());
                        Console.WriteLine("");

                        sockets.BeginSend(sendBuffer, 0, sendBuffer.Length, 0, new AsyncCallback(SendCallback), sockets);
                    }


                }

                sendMsg = "";





                socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);

            }




        }

        private static void SendCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.EndSend(result);



        }

        private static void SendLoop()
        {
            while (true)
            {
                //Protect this with a mutex
                sendBuffer = Encoding.ASCII.GetBytes(sendMsg);

                //Send updates to all clients in the list

                foreach (var socket in clientSockets)
                {
                    Console.WriteLine("Sent to: " + socket.RemoteEndPoint.ToString());

                    socket.BeginSend(sendBuffer, 0, sendBuffer.Length, 0, new AsyncCallback(SendCallback), socket);

                }

                sendMsg = "";
                Thread.Sleep(250);

            }
        }

        


    }
}
