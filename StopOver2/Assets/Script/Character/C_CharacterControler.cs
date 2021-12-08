using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterControler : MonoBehaviour
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

    public void TriggerControl(bool isGrounded, float verticalInput, Rigidbody rb)
    {
        if (isGrounded == true)
        {
            
            if(verticalInput > 0)
            {
                if (!firstAccelerationDone)
                {
                    FirstAcceleration();
                }else if (firstAccelerationDone && !accelerationDone)
                {
                    Acceleration();
                }

                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * verticalInput * currentSpeedForward, transform.position);
            }

            if(verticalInput < 0)
            {
                firstAccelerationDone = false;
                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * verticalInput * speedBackWard, transform.position);
            }

            if(verticalInput == 0)
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

    public void TriggerFixedControl(bool isGrounded, float verticalInput, float mouseXInput, Rigidbody rb)
    {
        if (isGrounded == false)
        {
            rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.right) * -verticalInput * airControlSpeedForward);
            rb.AddTorque(Vector3.up * torqueAirControl * mouseXInput);

        }
        else
        {
            rb.AddTorque(Vector3.up * torque * mouseXInput);
        }
    }
}
