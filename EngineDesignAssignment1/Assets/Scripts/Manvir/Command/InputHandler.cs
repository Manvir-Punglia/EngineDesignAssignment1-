using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler
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
            if (Input.GetKeyDown(entry.Key))
            {
                entry.Value.Execute();
            }
        }
    }
}

