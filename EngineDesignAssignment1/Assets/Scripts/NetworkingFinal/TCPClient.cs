using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
    private static Socket _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private static byte[] buffer = new byte[1024];
    private static byte[] sendBuffer = new byte[1024];


    private static string receivedMessage;

    // Start is called before the first frame update
    void Start()
    {


       

        _client.Connect(IPAddress.Parse("127.0.0.1"), 8888);
        Debug.Log("Connected to the server");

        _client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), _client);
    }

    // Update is called once per frame
    void Update()
    {
        SendMsg("I HATE YOU!!!");
    }


    private void ReceiveCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int rec = socket.EndReceive(result);

        string msg = Encoding.ASCII.GetString(buffer, 0, rec);
        Debug.Log("Received: " + msg);

        //enterMsg.updateRecv(msg);
        receivedMessage = msg;

        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);
    }


    public void SendMsg(string sentMsg)
    {
        sendBuffer = Encoding.ASCII.GetBytes(sentMsg);

        //sendBuffer = Encoding.ASCII.GetBytes("Hoi!");


        _client.Send(sendBuffer);
    }

}
