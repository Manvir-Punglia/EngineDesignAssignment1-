using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Subject
{
    public Rigidbody rb;               
    public float moveForce = 500f;  
    public float turnSpeed = 25f;     
    public float maxSpeed = 50f;       

    private float moveInput;          
    private float turnInput;           
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Debug.Log(rb.velocity.magnitude);
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
            
            float rotationAngle = turnInput * turnSpeed * Time.fixedDeltaTime;
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + rotationAngle, 0f);
            //turns smoothly remove if turning is bad 
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);
        }
    }
}
