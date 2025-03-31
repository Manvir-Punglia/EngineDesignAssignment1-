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
    private byte[] buffer = new byte[1024];
    private IPEndPoint serverEndPoint;
    private Dictionary<string, GameObject> otherPlayers = new Dictionary<string, GameObject>();
    
    [Header("Settings")]
    public string playerName = "Player1";
    public float updateRate = 0.01f;// very important will change how fluid the movement is
    public GameObject playerPrefab;
    
    [Header("References")]
    //can be used if we want to input somthing into the script, ask Manvir if you need to do this 
    //[SerializeField] private Inbetween _inbetween; 
    
    [SerializeField] private Transform localPlayer;
    [SerializeField] private TextMeshProUGUI debugText;


    float timeSinceLastUpdate = 0.0f;
    float maxTimeForUpdate = 0.01f;
    bool deadCheckoning = false;
    void Start()
    {
        try
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8889);
            
            Debug.Log($"UDP connecting to {serverEndPoint}");
            
            StartCoroutine(ReceiveLoop());// we use Coroutines because threads are cringe 
            StartCoroutine(SendLoop());
        }
        catch (Exception ex)
        {
            Debug.LogError("Client error: " + ex.Message);
        }
    }

    void Update()
    {
        Debug.Log($"connected players num: {otherPlayers.Count + 1}");

    }

    private IEnumerator SendLoop()
    {
        while (true)
        {
            try
            {
                // Get current position and rotation
                Vector3 position = localPlayer.position;
                Quaternion rotation = localPlayer.rotation;
                
                // The system only supports this format, 
                string message = $"{playerName}:" + $"{position.x}:{position.y}:{position.z}:" + $"{rotation.x}:{rotation.y}:{rotation.z}:{rotation.w}";
                
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
                    
                    // Parse message format: "Name:PosX:PosY:PosZ:RotX:RotY:RotZ:RotW"
                    string[] parts = message.Split(':');// how we split the values into different pieces 
                    if (parts.Length == 8)// make sure the size is right
                    {
                        string receivedName = parts[0];
                        float px = float.Parse(parts[1]);
                        float py = float.Parse(parts[2]);
                        float pz = float.Parse(parts[3]);
                        float rx = float.Parse(parts[4]);
                        float ry = float.Parse(parts[5]);
                        float rz = float.Parse(parts[6]);
                        float rw = float.Parse(parts[7]);

                        if (receivedName == playerName) continue;// this will skip what happens next if it recives itw own name in the data 

                        Quaternion rotation = new Quaternion(rx, ry, rz, rw);
                        Vector3 position = new Vector3(px, py, pz);

                        if (!otherPlayers.ContainsKey(receivedName))// if the player is not already in the scene it will add them 
                        {
                            GameObject newPlayer = Instantiate(
                                playerPrefab, 
                                position, 
                                rotation
                            );
                            newPlayer.name = receivedName;
                            otherPlayers.Add(receivedName, newPlayer);
                            Debug.Log($"New player connected: {receivedName}");
                        }
                        else // this is if they already have a car
                        {
                            DeadReckoning playerDeadReckon = otherPlayers[receivedName].GetComponent<DeadReckoning>();

                            playerDeadReckon.deadCheckoning = false;

                            Transform playerTransform = otherPlayers[receivedName].transform;
                            playerTransform.position = position;
                            playerTransform.rotation = rotation;

                            playerDeadReckon.updateData(position, rotation);

                            playerDeadReckon.deadCheckoning = true;
                            
                        }
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
        
        foreach (var player in otherPlayers.Values)
        {
            Destroy(player);
        }
    }
}