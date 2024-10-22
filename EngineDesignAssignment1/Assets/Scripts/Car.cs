using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : Subject
{
    public Rigidbody rb;               
    public float moveForce = 500f;  
    public float turnSpeed = 25f;
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
            moveInput = 1f;  
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveInput = -1f;  
        }

        
        if (Input.GetKey(KeyCode.A))
        {
            turnInput = -1f; 
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnInput = 1f; 
        }

       
        if (rb.velocity.magnitude < maxSpeed)// will add foerce while speed is below max 
        {
            rb.AddForce(transform.forward * moveInput * moveForce * Time.fixedDeltaTime);
        }

       
        if (rb.velocity.magnitude > 0)// you can only turn as long as you are moving
        {
            isDrift = true;
            float rotationAngle = turnInput * turnSpeed * Time.fixedDeltaTime;
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + rotationAngle, 0f);
            //turns smoothly remove if turning is bad 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);
        }

    }
    public float GetSpeed()
    {
        return rb.velocity.magnitude;
    }

    private void CheckDrift()
    {
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
}
