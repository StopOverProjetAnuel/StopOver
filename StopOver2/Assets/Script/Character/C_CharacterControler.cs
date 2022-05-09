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

    public float airControlSpeedForward;

    [Header("Rotation Parameters")]
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
    public float fallAngle = 15f;
    public float timeToFallPos = 1f;
    private float fallTimer;
    private Rigidbody rb;
    public float autoDiveSpeedMul = 1f;

    [Space(10)]
    public bool showDebug = false;
    #endregion

    public void InitiateControlValue(Rigidbody pRb)
    {
        _CharacterBoost = GetComponent<C_CharacterBoost>();
        currentSpeed = speedPlayer;
        rb = pRb;
    }

    public void TriggerControl(float verticalInput, bool isGrounded, GameObject centerOfMass)
    {
        if (isGrounded && currentAirMultiplier != 1f)
        {
            currentAirMultiplier = 1f;
        }
        else if (currentAirMultiplier != speedAirMultiplier)
        {
            currentAirMultiplier = speedAirMultiplier;
        }

        if (Input.GetButtonDown("Vertical")) 
        {
            rb.AddRelativeForce(0, 0, Input.GetAxisRaw("Vertical") * firstAccelerationForce * currentAirMultiplier, ForceMode.Impulse);
        }

        if(verticalInput > 0) //Move Forward
        {
            rb.AddRelativeForce(0, 0, verticalInput * currentSpeed * currentAirMultiplier, ForceMode.Acceleration);
            #region Debug
                if (showDebug)
                {
                    Debug.Log("Player Forward");
                }
                #endregion
        }

        if (verticalInput < 0) //Move Backward
        {
            rb.AddRelativeForce(0, 0, verticalInput * currentSpeed * speedBackwardMultiplier * currentAirMultiplier, ForceMode.Acceleration);
        }

        Vector3 newComPos = new Vector3(0, 1, comFowardPos * Input.GetAxisRaw("Vertical"));
        centerOfMass.transform.localPosition = newComPos;
    }

    public void TriggerRotation(bool isGrounded, float verticalInput, float mouseXInput, Quaternion airAngle)
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

            AutoDive(airAngle, isGrounded);
        }
    }

    private void AutoDive(Quaternion airAngle, bool grounded)
    {
        if (fallTimer >= timeToFallPos && transform.localRotation.x < fallAngle)
        {
            float angleGap = fallAngle + transform.localRotation.x;

            float angleVelocityX = angleGap * autoDiveSpeedMul;
            rb.AddRelativeTorque(angleVelocityX, 0, 0, ForceMode.Acceleration);
        }
        else if (fallTimer < timeToFallPos && !grounded) fallTimer += Time.fixedDeltaTime; 

        #region auto dive player with transform rotation (outdated & take priority)
        /*if (fallTimer <= timeToFallPos)
        {
            float timeAngle = Mathf.Clamp(fallTimer / timeToFallPos, 0, 1);
            Quaternion newAirAngle = Quaternion.Euler(new Vector3(fallAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            Quaternion currentAirAngle = Quaternion.Lerp(originAirAngle, newAirAngle, timeAngle);

            transform.rotation = currentAirAngle;

            fallTimer += Time.fixedDeltaTime;

            #region Debug
            if (showDebug)
            {
                Debug.Log("airAngle : " + airAngle);
                Debug.Log("new airAngle : " + b.eulerAngles);
            }
            #endregion
        }*/
        #endregion
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