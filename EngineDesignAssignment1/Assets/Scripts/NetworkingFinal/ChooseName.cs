using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChooseName : MonoBehaviour
{
    public string _name;
    [SerializeField] private TMP_InputField _inputField;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextLevel()
    {
        _name = _inputField.text;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }
}
