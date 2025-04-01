using System;
using System.Threading;

namespace NetworkingFinalProj
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            UdpServer udpServer = new UdpServer();
            Thread udpThread = new Thread(new ThreadStart(udpServer.Start)) { IsBackground = true };
            udpThread.Start();

            TcpServer tcpServer = new TcpServer();
            tcpServer.Start();
            
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}