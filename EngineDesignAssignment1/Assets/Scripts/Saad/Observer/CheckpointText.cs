using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointText : Observer
{
    private Car _car;
    private int _currentCheckpoint;
    private int _lastCheckpoint;
    public GameObject checkpointTextObject;

    [SerializeField] private bool isDirty = false;

    private int _previousCurrentCheckpoint;
    private int _previousLastCheckpoint;

    private void Awake()
    {
        if (checkpointTextObject == null)
        {
            checkpointTextObject.SetActive(true);
            checkpointTextObject = GameObject.Find("CheckpointText");
        }
        
    }

    private void Start()
    {
        checkpointTextObject = GameObject.Find("CheckpointText");
    }

    void Update()
    {
        if (isDirty && checkpointTextObject != null)
        {
            checkpointTextObject.GetComponent<Text>().text = (_currentCheckpoint.ToString()) + "/" + _lastCheckpoint;

            isDirty = false;
        }
        
    }

    public override void Notify(Subject subject)
    {
        _car = subject.GetComponent<Car>();
        _currentCheckpoint = _car._currentCheckpoint;
        _lastCheckpoint = _car._lastCheckpoint;

        if (_currentCheckpoint != _previousCurrentCheckpoint || _lastCheckpoint != _previousLastCheckpoint)
        {   
            isDirty = true;

            _previousCurrentCheckpoint = _currentCheckpoint;
            _previousLastCheckpoint = _lastCheckpoint;
        }
    }
}
