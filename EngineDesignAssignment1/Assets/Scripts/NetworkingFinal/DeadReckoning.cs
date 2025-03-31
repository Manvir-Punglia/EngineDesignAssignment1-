using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadReckoning : MonoBehaviour
{
    [Header("Does Object Dead Reckon?")]
    [SerializeField] bool deadReckoning = true;


    public bool deadCheckoning = false;
    float timeSinceLastUpdate = 0.0f;
    float timeUntilDeadReckon = 0.01f;

    public Vector3 lastPositionUpdate;
    public Quaternion lastRotationUpdate;


    Vector3 posDiff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (deadReckoning)
        {
            if (deadCheckoning)
            {
                timeSinceLastUpdate += Time.deltaTime;

                if (timeSinceLastUpdate >= timeUntilDeadReckon)
                {
                    transform.position = transform.position + posDiff * Time.deltaTime;
                    //Debug.LogError("Dead Reckoning");
                }
            }
            else
            {
                timeSinceLastUpdate = 0.0f;
            }
        }
        
    }

    public void updateData(Vector3 newPos, Quaternion newRot)
    {
        posDiff = newPos - lastPositionUpdate;

        lastPositionUpdate = newPos;
        lastRotationUpdate = newRot;
    }


   
}
