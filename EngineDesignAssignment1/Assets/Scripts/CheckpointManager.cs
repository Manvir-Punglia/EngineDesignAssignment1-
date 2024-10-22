using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : Singleton<CheckpointManager>
{
    private List<SingleCheckpoint> singleCheckpointList;
    private int nextCheckpointIndex;

    public Text checkpointText;

    void Start()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");
        if (checkpointsTransform == null)
        {
            Debug.Log("didnt work");
        }

        singleCheckpointList = new List<SingleCheckpoint> ();

        foreach (Transform Checkpoint in checkpointsTransform)
        {
            SingleCheckpoint singleCheckpoint = Checkpoint.GetComponent<SingleCheckpoint>();
            singleCheckpoint.setCheckpoints(this);
            singleCheckpointList.Add(singleCheckpoint);
        }

        nextCheckpointIndex = 0;
    }

    private void Update()
    {
        checkpointText.text = (nextCheckpointIndex.ToString()) + "/" + singleCheckpointList.Count;
    }

    public void PassCheckpoint(SingleCheckpoint singleCheckpoint)
    {
        if (singleCheckpointList.IndexOf(singleCheckpoint) == nextCheckpointIndex)
        {
            Debug.Log("Correct Checkpoint");
            nextCheckpointIndex++;
        }
        else
        {
            Debug.Log("Wrong Checkpoint");
        }
    }
}