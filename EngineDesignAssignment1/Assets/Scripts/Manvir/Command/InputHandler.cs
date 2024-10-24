using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler:MonoBehaviour
{
    private Dictionary<KeyCode, ICommand> keyLog;

    public InputHandler()
    {
        keyLog = new Dictionary<KeyCode, ICommand>();
    }

    public void SetCommand(KeyCode key, ICommand command)
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

