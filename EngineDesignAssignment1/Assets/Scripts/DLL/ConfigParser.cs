using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ConfigParser : MonoBehaviour
{
    private string filePath;

    void Start()
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
}