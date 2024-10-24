using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rebind : MonoBehaviour
{
    public GameObject rebindMenu;    
    private bool isMenuActive = false; 

    public Text accelerateKeyText;  
    public Button accelerateButton;  

    public Text brakeKeyText;       
    public Button brakeButton;      

    public Text turnLeftKeyText;     
    public Button turnLeftKeyButton; 

    public Text turnRightKeyText;    
    public Button turnRightKeyButton;

    public InputHandler _inputHandler;
    public Car _car;
    private KeyCode newKey;          
    private string actionToRebind;  

  

    void Start()
    {
        //_car = gameObject.GetComponent<Car>();
        //_inputHandler = gameObject.GetComponent<InputHandler>();

        // Default 
        _inputHandler.SetCommand(KeyCode.W, new ForwardCommand(_car));
        _inputHandler.SetCommand(KeyCode.S, new BackwardCommand(_car));
        _inputHandler.SetCommand(KeyCode.A, new TurnLeftCommand(_car));
        _inputHandler.SetCommand(KeyCode.D, new TurnRightCommand(_car));

        // Default text 
        accelerateKeyText.text = "W";
        brakeKeyText.text = "S";
        turnLeftKeyText.text = "A";
        turnRightKeyText.text = "D";

        accelerateButton.onClick.AddListener(() => StartRebind("Accelerate"));
        brakeButton.onClick.AddListener(() => StartRebind("Brake"));
        turnLeftKeyButton.onClick.AddListener(() => StartRebind("Left"));
        turnRightKeyButton.onClick.AddListener(() => StartRebind("Right"));

        rebindMenu.SetActive(false);
    }


    void StartRebind(string action)
    {
        actionToRebind = action;
        StartCoroutine(WaitForKeyPress());
    }
    private IEnumerator WaitForKeyPress()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                newKey = kcode;
                RebindKey(actionToRebind, newKey);
                break;
            }
        }
    }
    void RebindKey(string action, KeyCode key)
    {
        switch (action)
        {
            case "Accelerate":
                _inputHandler.SetCommand(key, new ForwardCommand(_car));
                accelerateKeyText.text = key.ToString();  
                Debug.Log("Accelerate bind: " + key.ToString());
                break;
            case "Brake":
                _inputHandler.SetCommand(key, new BackwardCommand(_car));
                brakeKeyText.text = key.ToString();  
                break;
            case "Left":
                _inputHandler.SetCommand(key, new TurnLeftCommand(_car));
                turnLeftKeyText.text = key.ToString();  
                break;
            case "Right":
                _inputHandler.SetCommand(key, new TurnRightCommand(_car));
                turnRightKeyText.text = key.ToString();  
                break;
        }
    }

    void Update()
    {
        if (!isMenuActive)  
        {
            _inputHandler.HandleInput();
        }

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = !isMenuActive;
            rebindMenu.SetActive(isMenuActive);
            
            if (isMenuActive)
            {
                Time.timeScale = 0f;  
            }
            else
            {
                Time.timeScale = 1f;  
            }
        }
    }
}
