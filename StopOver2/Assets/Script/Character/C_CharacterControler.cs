using UnityEngine;

public class C_CharacterControler : MonoBehaviour
{
    #region Varibles
    C_CharacterBoost _CharacterBoost;

    [Header("Speed Parameters")]
    public float speedPlayer;
    [HideInInspector]
    public float currentSpeed;
    public float speedBackwardMultiplier = 0.5f;
    public float speedAirMultiplier = 0.25f;
    private float currentAirMultiplier = 1f;
    public float firstAccelerationForce = 50f;
    private bool firstAccelerationDone = false;

    public float airControlSpeedForward;

    public float maxRotateSpeed = 1440f;
    public float minRotateSpeed = 720f;
    public float maxSpeedForMinRspeed = 60f;
    public float rSAirMultiplier; //rotate speed

    public float comFowardPos = 0.95f;

    [Header("Fall Parameters")]
    public float maxFallAcceleration = 110f;
    public float minFallAcceleration = 50f;
    [SerializeField]
    private float currentFallAcceleration = 50f;
    public AnimationCurve curveFallAcceleration;
    public float dragAirForce;
    public float dragGroundForce;
    [Space]
    public float fallAngle = 30f;
    public float timeToFallPos = 1f;
    private float fallTimer;

    [Space(10)]
    public bool showDebug = false;
    #endregion

    public void InitiateControlValue()
    {
        _CharacterBoost = GetComponent<C_CharacterBoost>();
        currentSpeed = speedPlayer;
    }

    public void TriggerControl(float verticalInput, Rigidbody rb, bool isGrounded, GameObject centerOfMass)
    {
        if (isGrounded && currentAirMultiplier != 1f)
        {
            currentAirMultiplier = 1f;
        }
        else if (currentAirMultiplier != speedAirMultiplier)
        {
            currentAirMultiplier = speedAirMultiplier;
        }


        if(verticalInput > 0) //Move Forward
        {
            if (firstAccelerationDone)
            {
                rb.AddRelativeForce(0, 0, verticalInput * currentSpeed * currentAirMultiplier, ForceMode.Acceleration);
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
            else
            {
                rb.AddRelativeForce(0, 0, firstAccelerationForce, ForceMode.Impulse);
                firstAccelerationDone = true;
            }
        }
        else if (verticalInput == 0 && firstAccelerationDone == true)
        {
            firstAccelerationDone = false;
        }

        if (verticalInput < 0) //Move Backward
        {
            rb.AddRelativeForce(0, 0, verticalInput * currentSpeed * speedBackwardMultiplier * currentAirMultiplier, ForceMode.Acceleration);
        }

        Vector3 newComPos = new Vector3(0, 1, comFowardPos * Input.GetAxisRaw("Vertical"));
        centerOfMass.transform.localPosition = newComPos;
    }

    public void TriggerRotation(bool isGrounded, float verticalInput, float mouseXInput, Rigidbody rb, Quaternion airAngle)
    {
        float normSpeed = Mathf.Clamp(rb.velocity.magnitude / maxSpeedForMinRspeed, 0, 1);
        float currentRotateSpeed = Mathf.Lerp(maxRotateSpeed, minRotateSpeed, normSpeed);
        Vector3 rotateValue = Vector3.up * currentRotateSpeed * mouseXInput * Time.fixedDeltaTime;

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
            rb.AddTorque(rotateValue * rSAirMultiplier, ForceMode.Acceleration);

            if (fallTimer <= timeToFallPos)
            {
                float a = Mathf.Clamp(fallTimer / timeToFallPos, 0, 1);
                Quaternion b = Quaternion.Euler(new Vector3(fallAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
                Quaternion c = Quaternion.Lerp(airAngle, b, a);

                transform.rotation = c;

                fallTimer += Time.fixedDeltaTime;

                #region Debug
                if (showDebug)
                {
                    Debug.Log("airAngle : " + airAngle);
                    Debug.Log("new airAngle : " + b.eulerAngles);
                }
                #endregion
            }
        }
    }

    public void GravityFall(bool isGrounded, Rigidbody rb)
    {
        if (isGrounded)
        {
            rb.drag = dragGroundForce;

            if (accelerationFallTimer != 0f) //reset timer fall acceleration
            {
                accelerationFallTimer = 0f;
                currentFallAcceleration = minFallAcceleration;
            }
        }
        else
        {
            rb.drag = dragAirForce;
            rb.AddForce(Vector3.down * currentFallAcceleration, ForceMode.Force);

            if (currentFallAcceleration != maxFallAcceleration)
            {
                IncreesFallAcceleration();
            }
        }
    }

    float accelerationFallTimer = 0f;
    private void IncreesFallAcceleration()
    {
        accelerationFallTimer = Mathf.Clamp(accelerationFallTimer + Time.fixedDeltaTime / 2, 0, 1);
        float a = curveFallAcceleration.Evaluate(accelerationFallTimer);
        currentFallAcceleration = Mathf.Lerp(minFallAcceleration, maxFallAcceleration, a);
    }
}