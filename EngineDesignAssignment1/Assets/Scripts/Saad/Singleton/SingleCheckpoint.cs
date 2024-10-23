using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCheckpoint : MonoBehaviour
{
    CheckpointManager checkpointManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Car>(out Car car))
        {
            TimeManager.Instance.ResetTimer();
            checkpointManager.PassCheckpoint(this);
        }
    }

    public void setCheckpoints(CheckpointManager CM)
    {
        this.checkpointManager = CM;
    }
}
