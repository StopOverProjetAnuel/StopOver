using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterControler : C_CharacterManager
{
    public float timeFirstAcceleration;
    public float firstAccelerationForward;
    public float speedForward;
    public float speedBackWard;

    public float torque;

    private bool firstAccelerationDone;

    private float t1;
    private float currentSpeedForward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnAir)
        {
            if(_CharacterInput.verticalInput > 0)
            {
                if (t1 >= 1f)
                {
                    firstAccelerationDone = true;
                    t1 = 0;
                }

                if (!firstAccelerationDone)
                {
                    t1 += Time.deltaTime / timeFirstAcceleration;
                    currentSpeedForward = Mathf.SmoothStep(firstAccelerationForward, speedForward, t1);
                }
                else if (firstAccelerationDone)
                {
                    currentSpeedForward = speedForward;
                }

                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * _CharacterInput.verticalInput * currentSpeedForward, transform.position);
            }

            if(_CharacterInput.verticalInput < 0)
            {
                firstAccelerationDone = false;
                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * _CharacterInput.verticalInput * currentSpeedForward, transform.position);
            }

            if(_CharacterInput.verticalInput == 0)
            {
                firstAccelerationDone = false;
            }

        }
        
    }

    private void FixedUpdate()
    {
        //transform.rotation = _CharacterInput.cameraDirection;

        rb.AddTorque(transform.up * torque * _CharacterInput.horizontalInput);
    }
}
