using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : Subject
{
    [Header("Plugin")]
    public OurPlugin.ConfigParser _configParser;

    [Header("Physics Properties")]
    public Rigidbody rb;               
    public float moveForce = 500f;  
    public float turnSpeed = 100f;
    public float maxSpeed = 50f;
    public float floatForce = 10f;
    public float maxFloatHeight = 2f;
    public float minFloatHeight = 1f;
    public Transform respawnPos;

    [Header("Wheel Properties")]
    public TrailRenderer[] _tireTracks;
    public Transform[] _rearWheels;
    public Transform[] _frontWheels;
    public float wheelRadius;
    public float maxSteerAngle = 20f;
    public float driftPotential = 45f;

    private float moveInput;          
    private float turnInput;           
    private Quaternion targetRotation;

    private ProgressBar _progressBar;
    private CheckpointText _checkpointText;

    [HideInInspector] public int _currentCheckpoint;
    [HideInInspector] public int _lastCheckpoint;

    [HideInInspector] public bool isDrift;

    public Renderer carRenderer;

    private float distTravelled;
    private float rotInRad;
    private float rotInDeg;

    private Vector3 localAngle;
    private float steerAngle;

    public GameObject[] _bombParts;

    public ParticleSystem explosionVFX;
    

    private void Awake()
    {

        _progressBar = (ProgressBar)FindObjectOfType(typeof(ProgressBar));
        _checkpointText = (CheckpointText)FindObjectOfType(typeof(CheckpointText));
        if (_configParser == null)
        {
            _configParser = FindObjectOfType<OurPlugin.ConfigParser>();
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isDrift = false;
        carRenderer = carRenderer.gameObject.GetComponent<Renderer>();
        moveForce = _configParser.carSpeed;
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
        TireSpin();
        TireTurn();
    }

    void FixedUpdate()
    {

        //Debug.Log("Move Val: " + moveInput);
        //Debug.Log("Turn Val: " + turnInput);


        

        //if (Input.GetKey(KeyCode.W))
        //{
        //    MoveForward();  
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    MoveBackward();
        //}

        
        //if (Input.GetKey(KeyCode.A))
        //{
        //    TurnLeft();
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    TurnRight();
        //}

       
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

        moveInput = 0f;
        turnInput = 0f;

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

    private void TireSpin()
    {
        distTravelled = rb.velocity.magnitude * Time.deltaTime;
        rotInRad = distTravelled / wheelRadius;
        rotInDeg = rotInRad * Mathf.Rad2Deg;

        for (int i = 0; i < _rearWheels.Length; i++)
        {
            _rearWheels[i].transform.Rotate(0, 0, rotInDeg);
        }
    }

    private void TireTurn()
    {
        if (turnInput == -1f)
        {
            steerAngle = 90f - maxSteerAngle;
        }
        else if(turnInput == 1f)
        {
            steerAngle = 90f + maxSteerAngle;
        }
        else
        {
            steerAngle = 90f;
        }
        
        for (int i = 0; i < _frontWheels.Length; i++)
        {
            localAngle = _frontWheels[i].transform.localEulerAngles;
            localAngle.y = steerAngle;
            _frontWheels[i].transform.localEulerAngles = localAngle;
        }
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
        Debug.Log("moving Forward");
    }
    public void MoveBackward()
    {
        moveInput = -1f;
    }

    public Color GetCarColour()
    {
        return carRenderer.materials[0].color;
    }

    public void SetCarColour(Color colour)
    {
        carRenderer.materials[0].color = colour;
    }

    public void RespawnCar()
    {
        transform.position = respawnPos.position;
        transform.rotation = respawnPos.rotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        for (int i = 0; i < _bombParts.Length; i++)
        {
            _bombParts[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < _rearWheels.Length; i++)
        {
            _rearWheels[i].gameObject.SetActive(true);
        }
    }

    public void ReduceSpeed(float speedLoss)
    {
        Vector3 currentVel = rb.velocity;
        float newSpeed = Mathf.Max(currentVel.magnitude -  speedLoss, 0f);
        rb.velocity = currentVel.normalized * newSpeed;
    }

    public void Explode()
    {
        explosionVFX.Play();

        for (int i = 0; i < _bombParts.Length; i++)
        {
            _bombParts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _rearWheels.Length; i++)
        {
            _rearWheels[i].gameObject.SetActive(false);
        }
        
    }

}
