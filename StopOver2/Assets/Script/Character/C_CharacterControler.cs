using UnityEngine;

public class C_CharacterControler : MonoBehaviour
{
    #region Varibles
    C_CharacterBoost _CharacterBoost;

    [Header("Speed Parameters")]
    public float timeFirstAcceleration;
    public float timeAcceleration;
    public float firstAccelerationForward;
    public float speedForward;
    public float speedBackWard;
    [HideInInspector]
    public float currentSpeedForward;

    [SerializeField] float rotateSpeed;
    [SerializeField] float rSAirMultiplier; //rotate speed

    public float airControlSpeedForward;

    bool firstAccelerationDone;
    bool accelerationDone;

    float t1;
    float t2;

    [Header("Fall Parameters")]
    public float fallAcceleration;
    public float dragAirForce;
    public float dragGroundForce;
    [Space]
    public float fallAngle = 30f;
    public AnimationCurve smoothFallForward;
    float fallTimer;

    [Space(10)]
    public bool showDebug = false;
    #endregion

    public void InitiateControlValue()
    {
        _CharacterBoost = GetComponent<C_CharacterBoost>();
        currentSpeedForward = speedForward;
    }

    public void TriggerControl(float verticalInput, Rigidbody rb)
    {
        if(verticalInput > 0) //Move Forward
        {
            rb.AddRelativeForce(0, 0, verticalInput * currentSpeedForward);
            #region Debug
            if (showDebug)
            {
                Debug.Log("Player Forward");
            }
            #endregion
            #region OLD
            /**if (!firstAccelerationDone)
            {
                FirstAcceleration();
            }
            else if (firstAccelerationDone && !accelerationDone)
            {
                Acceleration();
            }*/
            #endregion
        }

        if (verticalInput < 0) //Move Backward
        {
            rb.AddRelativeForce(0, 0, verticalInput * currentSpeedForward);
            #region OLD
            //firstAccelerationDone = false;
            #endregion
        }
        #region OLD
        /**if(verticalInput == 0)
        {
            firstAccelerationDone = false;
            accelerationDone = false;
        }*/
        #endregion
    }
    #region OLD
    /**private void FirstAcceleration()
    {
        if (!_CharacterBoost.isBoosted)
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
        else
        {
            currentSpeedForward = 30000;
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
    }*/
    #endregion

    public void TriggerRotation(bool isGrounded, float verticalInput, float mouseXInput, Rigidbody rb)
    {
        Vector3 rotateValue = Vector3.up * rotateSpeed * mouseXInput * Time.fixedDeltaTime;

        if (isGrounded)
        {
            rb.AddTorque(rotateValue, ForceMode.Acceleration);

            if (fallTimer != 0)
            {
                fallTimer = 0f;
            }
        }
        else
        {
            rb.AddTorque(rotateValue * rSAirMultiplier);

            if (fallTimer <= 1)
            {
                Vector3 keepCurrentRotation = transform.localRotation.eulerAngles; //Keep the current rotation in y and z
                keepCurrentRotation.x = 0f;
                Vector3 fallRotation = fallAngle * Vector3.right; //Make the new rotation in x
                Vector3 changeRotation = fallRotation + keepCurrentRotation; //Combine both
                //transform.localRotation = Quaternion.Euler(smoothFallForward.Evaluate(fallTimer));

                fallTimer += Time.fixedDeltaTime;

                Debug.Log("keepCurrentRotation" + keepCurrentRotation);
                Debug.Log("fallRotation" + fallRotation);
                Debug.Log("changeRotation" + changeRotation);
            }
        }
    }

    public void GravityFall(bool isGrounded, Rigidbody rb)
    {
        if (isGrounded)
        {
            rb.drag = dragGroundForce;
        }
        else
        {
            rb.drag = dragAirForce;
            rb.AddForce(Vector3.down * fallAcceleration, ForceMode.Acceleration);
        }
    }
}