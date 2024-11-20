using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ConfigParser : MonoBehaviour
{
    private string filePath;
    public float carSpeed { get; private set; }
    public float timerLength { get; private set; }
    public int obstacleAmount { get; private set; }

    private void Awake()
    {
        // path 
        filePath = Path.Combine(Application.streamingAssetsPath, "Config.txt");
        // ChatGPT was used for this. It was given how the document would look like and asked to read the values and assign it to variables, i changed the code by then making the variables actually do something 
        if (File.Exists(filePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                
                // base Values
                int width = 0, height = 0;
                bool isFullScreen = false;

                foreach (string line in lines)
                {
                    if (line.StartsWith("Resolution:"))
                    {
                        // extract resolution values
                        string resolutionPart = line.Replace("Resolution:", "").Trim();
                        string[] resolutionValues = resolutionPart.Split(',');

                        if (resolutionValues.Length == 2)
                        {
                            width = int.Parse(resolutionValues[0].Trim());
                            height = int.Parse(resolutionValues[1].Trim());
                        }
                    }
                    else if (line.StartsWith("Fullscreen:"))
                    {
                        string fullscreenPart = line.Replace("Fullscreen:", "").Trim();
                        isFullScreen = bool.Parse(fullscreenPart);
                    }
                    else if (line.StartsWith("Car Speed:"))
                    {
                        // Extract car speed value
                        string carSpeedPart = line.Replace("Car Speed:", "").Trim();
                        carSpeed = float.Parse(carSpeedPart);
                    }
                    else if (line.StartsWith("Timer Length:"))
                    {
                        // Extract timer length value
                        string timerLengthPart = line.Replace("Timer Length:", "").Trim();
                        timerLength = float.Parse(timerLengthPart);
                    }
                    else if (line.StartsWith("Obstacle Amount:"))
                    {
                        // Extract obstacle amount value
                        string obstacleAmountPart = line.Replace("Obstacle Amount:", "").Trim();
                        obstacleAmount = int.Parse(obstacleAmountPart);
                    }
                }


                if (width > 0 && height > 0)
                {
                    Screen.SetResolution(width, height, isFullScreen);
                    Debug.Log($"Resolution set to {width}x{height}, Fullscreen: {isFullScreen}");
                }
                else
                {
                    Debug.LogError("Break 2");
                }
                
                // Log other game settings
                Debug.Log($"Car Speed: {carSpeed}");
                Debug.Log($"Timer Length: {timerLength}");
                Debug.Log($"Obstacle Amount: {obstacleAmount}");
            }
            catch (System.Exception)
            {
                Debug.LogError("Break 1");
            }
        }
        else
        {
            Debug.LogError("Break 3");
        }
    }

    void Start()
    {
        
    }
}