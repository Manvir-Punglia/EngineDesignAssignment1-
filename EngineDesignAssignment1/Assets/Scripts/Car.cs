using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : Subject
{
    public Rigidbody rb;               
    public float moveForce = 500f;  
    public float turnSpeed = 100f;
    public float maxSpeed = 50f;

    public TrailRenderer[] _tireTracks;

    private float moveInput;          
    private float turnInput;           
    private Quaternion targetRotation;

    private ProgressBar _progressBar;
    private CheckpointText _checkpointText;

    [HideInInspector] public int _currentCheckpoint;
    [HideInInspector] public int _lastCheckpoint;

    private bool isDrift;
    public float driftPotential = 45f;

    public float floatForce = 10f;
    public float maxFloatHeight = 2f;
    public float minFloatHeight = 1f;

    private void Awake()
    {
        _progressBar = (ProgressBar)FindObjectOfType(typeof(ProgressBar));
        _checkpointText = (CheckpointText)FindObjectOfType(typeof(CheckpointText));
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isDrift = false;
    }

    private void OnEnable()
    {
        Attach(_progressBar);
        Attach(_checkpointText);
    }

    private void OnDisable()
    {
        Detach(_progressBar);
        Detach(_checkpointText);
    }

    void Update()
    {
        //Debug.Log(rb.velocity.magnitude);
        NotifyObservers();
        _currentCheckpoint = CheckpointManager.Instance.nextCheckpointIndex;
        _lastCheckpoint = CheckpointManager.Instance.lastCheckpointIndex;

        CheckDrift();
    }

    void FixedUpdate()
    {

        //Debug.Log("Move Val: " + moveInput);
        //Debug.Log("Turn Val: " + turnInput);


        moveInput = 0f;
        turnInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            MoveForward();  
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveBackward();
        }

        
        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }

       
        if (rb.velocity.magnitude < maxSpeed)// will add foerce while speed is below max 
        {
            rb.AddForce(transform.forward * moveInput * moveForce * Time.fixedDeltaTime);
        }

       
        if (rb.velocity.magnitude > 0)// you can only turn as long as you are moving
        {
            
            float rotationAngle = turnInput * turnSpeed * Time.fixedDeltaTime;
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + rotationAngle, 0f);
            //turns smoothly remove if turning is bad 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float distToGround = hit.distance;

            

            if (distToGround < minFloatHeight)
            {
                float upwardForce = floatForce * (minFloatHeight - distToGround);

                rb.AddForce(Vector3.up * upwardForce, ForceMode.Acceleration);
            }
        }

        

    }
    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }

    private void CheckDrift()
    {
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 carForward = transform.forward;
        carForward.y = 0f;
        carForward.Normalize();

        float carAngle = Vector3.Angle(camForward, carForward);

        isDrift = carAngle > driftPotential;

        if (isDrift) StartEmitting();
        else StopEmitting();
    }

    private void StartEmitting()
    {
        foreach (TrailRenderer T in _tireTracks)
        {
            T.emitting = true;
        }
    }

    private void StopEmitting()
    {
        foreach (TrailRenderer T in _tireTracks)
        {
            T.emitting = false;
        }
    }
    public void TurnLeft()
    {
        turnInput = -1f;        
    }
    public void TurnRight()
    {
        turnInput = 1f;
    }
    public void MoveForward()
    {
        moveInput = 1f;
    }
    public void MoveBackward()
    {
        moveInput = -1f;
    }



}
