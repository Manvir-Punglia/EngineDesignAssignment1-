using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;

public class ClientMovement : MonoBehaviour
{
    private Socket client;
    private byte[] buffer = new byte[1024]; // Increased buffer size for string messages
    private IPEndPoint serverEndPoint;
    private Dictionary<string, GameObject> otherPlayers = new Dictionary<string, GameObject>();
    
    [Header("Settings")]
    public string playerName = "Player1";
    public float updateRate = 0.1f;
    public GameObject playerPrefab;
    
    [Header("References")]
   // [SerializeField] private Inbetween _inbetween;
    [SerializeField] private GameObject localPlayer;
    [SerializeField] private TextMeshProUGUI debugText;

    void Start()
    {
        try
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8889);
            
            Debug.Log($"UDP client started connecting to {serverEndPoint}");
            
            StartCoroutine(ReceiveLoop());
            StartCoroutine(SendLoop());
        }
        catch (Exception ex)
        {
            Debug.LogError("Client error: " + ex.Message);
        }
    }

    void Update()
    {
        // Update debug display
        Debug.Log($"Connected players: {otherPlayers.Count + 1}"); 
    }

    private IEnumerator SendLoop()
    {
        while (true)
        {
            try
            {
                // Get current position
                Vector3 position = localPlayer.transform.position;
                
                // Create message: "Name:X:Y:Z"
                string message = $"{playerName}:{position.x}:{position.y}:{position.z}";
                byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
                
                client.SendTo(sendBuffer, serverEndPoint);
            }
            catch (Exception ex)
            {
                Debug.LogError("Send error: " + ex.Message);
            }
            yield return new WaitForSeconds(updateRate);
        }
    }

    private IEnumerator ReceiveLoop()
    {
        EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        
        while (true)
        {
            try
            {
                if (client.Available > 0)
                {
                    int received = client.ReceiveFrom(buffer, ref remoteEP);
                    string message = Encoding.ASCII.GetString(buffer, 0, received);
                    
                    // Parse message format: "Name:X:Y:Z"
                    string[] parts = message.Split(':');
                    if (parts.Length == 4)
                    {
                        string receivedName = parts[0];
                        float x = float.Parse(parts[1]);
                        float y = float.Parse(parts[2]);
                        float z = float.Parse(parts[3]);

                        // Skip our own messages
                        if (receivedName == playerName) continue;

                        // Create new player if not exists
                        if (!otherPlayers.ContainsKey(receivedName))
                        {
                            GameObject newPlayer = Instantiate(playerPrefab, new Vector3(x, y, z), Quaternion.identity);
                            newPlayer.name = receivedName;
                            otherPlayers.Add(receivedName, newPlayer);
                            Debug.Log($"New player connected: {receivedName}");
                        }

                        // Update position
                        otherPlayers[receivedName].transform.position = new Vector3(x, y, z);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Receive error: " + ex.Message);
            }
            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        if (client != null)
        {
            client.Close();
            Debug.Log("UDP client shutdown");
        }
        
        // Clean up other players
        foreach (var player in otherPlayers.Values)
        {
            Destroy(player);
        }
    }
}