using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointManager : Singleton<CheckpointManager>
{
    private List<SingleCheckpoint> singleCheckpointList;
    [HideInInspector] public int nextCheckpointIndex;
    [HideInInspector] public int lastCheckpointIndex;
    
    void Start()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");
        if (checkpointsTransform == null)
        {
            Debug.Log("Couldn't find the checkpoints");
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
        lastCheckpointIndex = singleCheckpointList.Count;
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

    public void ResetCheckpoints()
    {
        nextCheckpointIndex = 0;
    }
}