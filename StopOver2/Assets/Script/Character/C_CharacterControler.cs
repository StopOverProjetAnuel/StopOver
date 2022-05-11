using UnityEngine;

public class C_CharacterControler : MonoBehaviour
{
    #region Varibles
    private C_CharacterBoost _CharacterBoost;
    private Rigidbody rb;

    [Header("Speed Parameters")]
    public float speedPlayer;
    [SerializeField] private float speedBackwardMultiplier = 0.5f;
    [Tooltip("multiple the speed to ")]
    [SerializeField] private float strafeSpeedMultiplier = 0.5f;
    [SerializeField] private float speedAirMultiplier = 0.25f;
    [SerializeField] private float firstAccelerationForce = 50f;
    [HideInInspector] public float currentSpeed;
    private float currentAirMultiplier = 1f;

    [Header("Rotation Parameters")]
    [SerializeField] private float maxRotateSpeed = 1440f;
    [SerializeField] private float minRotateSpeed = 720f;
    [SerializeField] private float maxSpeedForMinRspeed = 60f;
    [SerializeField] private float rSAirMultiplier; //rotate speed
    [SerializeField] private float comFowardPos = 0.95f;

    [Header("Fall Parameters")]
    [SerializeField] private float maxFallAcceleration = 110f;
    [SerializeField] private float minFallAcceleration = 50f;
    [SerializeField] private float currentFallAcceleration = 50f;
    [SerializeField] private AnimationCurve curveFallAcceleration;
    [SerializeField] private float dragAirForce;
    [SerializeField] private float dragGroundForce;

    [Header("Dive Parameters")]
    [SerializeField] private float diveAngle = 15f;
    [SerializeField] private float timeToDiveAngle = 1f;
    [SerializeField] private float autoDiveSpeedMul = 1f;
    private float fallTimer;

    [Header("Debug Parameters")]
    public bool showDebug = false;
    #endregion

    public void InitiateControlValue(Rigidbody rigidbody)
    {
        _CharacterBoost = GetComponent<C_CharacterBoost>();
        currentSpeed = speedPlayer;
        rb = rigidbody;
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

        if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0) 
        {
            rb.AddRelativeForce(0, 0, firstAccelerationForce * currentAirMultiplier, ForceMode.Impulse);
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
        if (fallTimer >= timeToDiveAngle && transform.localRotation.x < diveAngle)
        {
            float angleGap = diveAngle + transform.localRotation.x;

            float angleVelocityX = angleGap * autoDiveSpeedMul;
            rb.AddRelativeTorque(angleVelocityX, 0, 0, ForceMode.Acceleration);
        }
        else if (fallTimer < timeToDiveAngle && !grounded) fallTimer += Time.fixedDeltaTime; 

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