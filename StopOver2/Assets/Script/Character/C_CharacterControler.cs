using UnityEngine;

public class C_CharacterControler : MonoBehaviour
{
    #region Varibles
    private C_CharacterBoost _CharacterBoost;
    private Rigidbody rb;

    [Header("Speed Parameters")]
    public float speedPlayer;
    [Tooltip("Multiple speed for backward movement")]
    [SerializeField] private float speedBackwardMultiplier = 0.5f;
    [Tooltip("Multiple speed for strafe movement")]
    [SerializeField] private float strafeSpeedMultiplier = 0.5f;
    [Tooltip("Multiple speed in all direction when the player is flying")]
    [SerializeField] private float speedAirMultiplier = 0.25f;
    [Tooltip("Force impulse when the player accelerate at low speed")]
    [SerializeField] private float firstImpulseForce = 50f;
    [Tooltip("Player have to be lower than this value to use the first Impulse")]
    [SerializeField] private float maxSpeedRequireFirstImpulse = 25f;
    [HideInInspector] public float currentSpeed;
    private float currentAirMultiplier = 1f;

    [Header("Rotation Parameters")]
    [SerializeField] private float maxRotateSpeed = 1440f;
    [SerializeField] private float minRotateSpeed = 720f;
    [SerializeField] private float maxSpeedForMinRspeed = 60f;
    [SerializeField] private float rSAirMultiplier; //rotate speed
    [Tooltip("Offset of the center of mass when the player move forward")]
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
        rb = rigidbody;
    }

    public void TriggerControl(float verticalInput, float horizontalInput, bool isGrounded, GameObject centerOfMass)
    {
        currentAirMultiplier = (isGrounded) ? 1f : speedAirMultiplier;

        if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0 && rb.velocity.magnitude <= 10f) rb.AddRelativeForce(0, 0, firstImpulseForce * currentAirMultiplier, ForceMode.Impulse);

        if (horizontalInput != 0f || verticalInput != 0f) //Move
        {
            float currentBackwardMultiplier = (verticalInput < 0) ? speedBackwardMultiplier : 1f;

            Vector3 normInputDir = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            currentSpeed = speedPlayer * currentBackwardMultiplier * currentAirMultiplier;
            Vector3 forceDir = normInputDir * currentSpeed;
            forceDir.x *= strafeSpeedMultiplier;
            rb.AddRelativeForce(forceDir, ForceMode.Acceleration);
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

    private float timerAngle;
    private void AutoDive(Quaternion airAngle, bool grounded)
    {
        if (fallTimer >= timeToDiveAngle)
        {
            float timer = Mathf.Clamp01(Time.time - timerAngle + 1);
            Quaternion angleDiveV3 = Quaternion.Euler(diveAngle, rb.transform.rotation.eulerAngles.y, 0f);
            Quaternion newAngleDive = Quaternion.Lerp(transform.rotation, angleDiveV3, timer);
            rb.MoveRotation(newAngleDive);
        }
        else if (fallTimer < timeToDiveAngle && !grounded) fallTimer += Time.fixedDeltaTime; timerAngle = Time.time + timeToDiveAngle;

        Debug.Log(rb.transform.rotation.eulerAngles.y);

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