using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Linq;

class UdpServer
{
    private Socket udpSocket;
    private List<IPEndPoint> clients = new List<IPEndPoint>();
    private readonly object clientLock = new object();
    private byte[] buffer = new byte[1024]; // Increased buffer size for messages

    public void Start()
    {
        try
        {
            Console.WriteLine("Starting UDP server...");
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8889));

            Console.WriteLine("UDP Server listening on 127.0.0.1:8889");
            BeginReceive();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UDP Server error: {ex.Message}");
        }
    }

    private void BeginReceive()
    {
        try
        {
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            udpSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None,
                ref remoteEP, ReceiveCallback, remoteEP);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UDP Receive error: {ex.Message}");
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        EndPoint senderEP = new IPEndPoint(IPAddress.Any, 0);
        try
        {
            int bytesRead = udpSocket.EndReceiveFrom(ar, ref senderEP);
            var senderIPEP = (IPEndPoint)senderEP;
            
            // Add new client if not already in list
            lock (clientLock)
            {
                if (!clients.Any(c => c.Equals(senderIPEP)))
                {
                    clients.Add(senderIPEP);
                    Console.WriteLine($"New client connected: {senderIPEP}");
                }
            }

            // Broadcast to all other clients
            lock (clientLock)
            {
                foreach (var client in clients.ToList()) // Use copy for thread safety
                {
                    if (!client.Equals(senderIPEP))
                    {
                        udpSocket.BeginSendTo(buffer, 0, bytesRead, SocketFlags.None, client, SendCallback, null);
                    }
                }
            }

            // Decode and display the received message
            string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received from {senderIPEP}: {receivedMessage}");

            BeginReceive();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UDP Receive callback error: {ex.Message}");
            BeginReceive();
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        try { udpSocket.EndSendTo(ar); }
        catch (Exception ex) { Console.WriteLine($"UDP Send error: {ex.Message}"); }
    }
}