using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointText : Observer
{
    private Car _car;
    private int _currentCheckpoint;
    private int _lastCheckpoint;
    public Text checkpointText;

    // Update is called once per frame
    void Update()
    {
        checkpointText.text = (_currentCheckpoint.ToString()) + "/" + _lastCheckpoint;
    }

    public override void Notify(Subject subject)
    {
        _car = subject.GetComponent<Car>();
        _currentCheckpoint = _car._currentCheckpoint;
        _lastCheckpoint = _car._lastCheckpoint;
    }
}
