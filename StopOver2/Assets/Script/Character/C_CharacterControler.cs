using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterControler : C_CharacterManager
{
    public float timeFirstAcceleration;
    public float timeAcceleration;
    public float firstAccelerationForward;
    public float speedForward;
    public float speedBackWard;

    public float torque;
    public float torqueAirControl;

    public float airControlSpeedForward;

    private bool firstAccelerationDone;
    private bool accelerationDone;

    private float t1;
    private float t2;
    [HideInInspector]public float currentSpeedForward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckGrounded();

        if (CheckGrounded() == true)
        {
            
            if(_CharacterInput.verticalInput > 0)
            {
                if (!firstAccelerationDone)
                {
                    FirstAcceleration();
                }else if (firstAccelerationDone && !accelerationDone)
                {
                    Acceleration();
                }

                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * _CharacterInput.verticalInput * currentSpeedForward, transform.position);
            }

            if(_CharacterInput.verticalInput < 0)
            {
                firstAccelerationDone = false;
                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * _CharacterInput.verticalInput * speedBackWard, transform.position);
            }

            if(_CharacterInput.verticalInput == 0)
            {
                firstAccelerationDone = false;
                accelerationDone = false;
            }

        }


        
        
    }

    private void FirstAcceleration()
    {
        if(t2 >= 1f)
        {
            firstAccelerationDone = true;
            accelerationDone = false; 
            t2 = 0;
        }

        if (!firstAccelerationDone)
        {
            t2 += Time.deltaTime / timeFirstAcceleration;
            currentSpeedForward = Mathf.SmoothStep(currentSpeedForward, firstAccelerationForward, t2);
        }
    }

    private void Acceleration()
    {
        if (t1 >= 1f)
        {
            accelerationDone = true;
            t1 = 0;
        }

        if (!accelerationDone)
        {
            t1 += Time.deltaTime / timeAcceleration;
            currentSpeedForward = Mathf.SmoothStep(firstAccelerationForward, speedForward, t1);
        }
        else if (accelerationDone)
        {
            currentSpeedForward = speedForward;
        }
    }

    private void FixedUpdate()
    {
        if (CheckGrounded() == false)
        {
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.right) * -_CharacterInput.verticalInput * airControlSpeedForward);
            rb.AddTorque(Vector3.up * torqueAirControl * _CharacterInput.mouseXInput);

        }
        else
        {
            rb.AddTorque(Vector3.up * torque * _CharacterInput.mouseXInput);

        }

    }
}
