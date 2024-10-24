using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebind : MonoBehaviour
{
    private InputHandler _inputHandler;
    private Car _car;

    void Start()
    {
        _car = gameObject.GetComponent<Car>();
        _inputHandler = gameObject.GetComponent<InputHandler>();

        // Default bindings
        _inputHandler.SetCommand(KeyCode.W, new ForwardCommand(_car));
        _inputHandler.SetCommand(KeyCode.S, new BackwardCommand(_car));
        _inputHandler.SetCommand(KeyCode.A, new TurnLeftCommand(_car));
        _inputHandler.SetCommand(KeyCode.D, new TurnRightCommand(_car));
    }

    // Called when the user selects a new keybinding via the UI
    public void RebindKey(KeyCode newKey, string action)
    {
        switch (action)
        {
            case "Accelerate":
                _inputHandler.SetCommand(newKey, new ForwardCommand(_car));
                break;
            case "Brake":
                _inputHandler.SetCommand(newKey, new BackwardCommand(_car));
                break;
            case "TurnLeft":
                _inputHandler.SetCommand(newKey, new TurnLeftCommand(_car));
                break;
            case "TurnRight":
                _inputHandler.SetCommand(newKey, new TurnRightCommand(_car));
                break;

        }
    }

    void Update()
    {
        _inputHandler.HandleInput();
    }
}
