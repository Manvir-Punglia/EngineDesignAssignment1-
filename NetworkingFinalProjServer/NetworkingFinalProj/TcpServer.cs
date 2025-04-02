using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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

        static string path = @"..\\leaderboard.txt";

        static string leaderboard;

        static bool addScore = true;
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



                string[] msgTxt = msg.Split(':');
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] thisline = line.Split('|');
                        

                        for(int i = 0; i < thisline.Length; i++)
                        {
                            string[] s = thisline[i].Split(':');
                            //Console.WriteLine(s[0] + " " + msgTxt[0]);
                            if (s[0] == msgTxt[0])
                            {
                                addScore = false;
                                int s1 = Int32.Parse(s[1]);
                                int m1 = Int32.Parse(msgTxt[1]);
                                if (m1 > s1)
                                {
                                    thisline[i] = msg;
                                    Console.WriteLine("Score Changed!");
                                    
                                }
                            }
                        }

                        leaderboard = "";

                        foreach(string s in thisline)
                        {
                            if (addScore)
                            {
                                leaderboard = leaderboard + s.ToString() + "|";
                            }
                            else
                            {
                                if (thisline[thisline.Length-1] == s)
                                leaderboard = leaderboard + s.ToString();
                            }
                        }
                        Console.WriteLine(leaderboard);


                        




                    }

                    
                    
                }


                //Send updates to all clients in the list
                
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    if (addScore)
                    {
                        leaderboard = leaderboard + msg;
                    }
                    sw.Write(leaderboard);
                }
                
                

                



                sendBuffer = Encoding.ASCII.GetBytes(leaderboard);
                foreach (var sockets in clientSockets)
                {

                    Console.WriteLine("Sent to: " + sockets.RemoteEndPoint.ToString());
                    Console.WriteLine("");

                    sockets.BeginSend(sendBuffer, 0, sendBuffer.Length, 0, new AsyncCallback(SendCallback), sockets);


                }


                sendMsg = "";





                socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);
                addScore = true;
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
