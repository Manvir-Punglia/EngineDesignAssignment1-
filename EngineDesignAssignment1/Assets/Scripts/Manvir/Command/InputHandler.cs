using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler:MonoBehaviour
{
    private Dictionary<KeyCode, Command> keyLog;

    public InputHandler()
    {
        keyLog = new Dictionary<KeyCode, Command>();
    }

    public void SetCommand(KeyCode key, Command command)
    {
        keyLog[key] = command;
    }

    public void HandleInput()
    {
        foreach (var entry in keyLog)
        {
            if (Input.GetKey(entry.Key))
            {
                entry.Value.Execute();
                Debug.Log(entry.Value);
            }
        }
    }
}

