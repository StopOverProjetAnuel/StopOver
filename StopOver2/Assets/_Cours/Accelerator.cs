using UnityEngine;

public class Accelerator : MonoBehaviour
{
    [SerializeField] bool debug = true;

    #region Scripts Used
    C_CharacterControler _CharacterControler;
    C_CharacterFX _CharacterFX;
    #endregion

    Rigidbody rb;

    [Header("Speed")]
    [SerializeField] float baseSpeed = 5f;
    [SerializeField] float boostSpeed = 15f;
    [SerializeField] float accelerationInitialImpulse = 10f;

    [Header("Surchauffe")]
    [SerializeField] Vector2 cooldownDurationMinMax = new Vector2(1f, 2.5f);
    [SerializeField] float overheatCooldown = 10f;

    [SerializeField] float accelerationDuration = 1.75f;

    [SerializeField] AnimationCurve accelerationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    float accelerationTimer = 0f;
    float cooldownTimer = 0f;
    float currentSpeed = 0f;

    float accelerationRatio = 0f;
    bool isAccelerating = false;

    public void IniatiateBoostValue(C_CharacterControler characterControler, C_CharacterFX characterFX, Rigidbody _rb) //Work with awake
    {
        _CharacterControler = characterControler;
        _CharacterFX = characterFX;
        rb = _rb;
    }

    private void TriggerBoost(bool boostBegan, bool boostHeld)
    {
        BoostHandler(boostBegan, boostHeld);
    }



    public void BoostHandler(bool boostBegan, bool boostHeld)
    {
        if (!CooldownHandler(Time.deltaTime))
        {
            if (debug) Debug.Log("Cooldown : " + cooldownTimer);
            return;
        }

        if (boostBegan)
        {
            if (debug) Debug.Log("Boost Began");
            BeginBoost();

            return;
        }

        if (boostHeld && isAccelerating)
        {
            if (debug) Debug.Log("Boost Held : " + accelerationTimer);
            HeldBoost();
            return;
        }

        if (isAccelerating)
        {
            if (debug) Debug.Log("Boost Released : " + accelerationTimer);
            ReleaseBoost();
        }
    }

    /// <summary>
    /// Initial boost, sets acceleration to true
    /// </summary>
    public void BeginBoost()
    {
        rb.AddRelativeForce(0, 0, accelerationInitialImpulse, ForceMode.Impulse);
        _CharacterFX.ActiveBoost();

        isAccelerating = true;
    }

    /// <summary>
    /// Boost applied over time
    /// </summary>
    public void HeldBoost()
    {
        accelerationTimer = Mathf.Clamp(accelerationTimer + Time.fixedDeltaTime / 2, 0f, accelerationDuration);
        accelerationRatio = accelerationTimer / accelerationDuration;

        float curveValue = accelerationCurve.Evaluate(accelerationRatio);
        currentSpeed = Mathf.Lerp(baseSpeed, boostSpeed, curveValue);

        // Si on dépasse la durée d'acceleration
        if (accelerationTimer >= accelerationDuration)
        {
            SetCooldown(true, accelerationRatio);
            accelerationTimer = 0f;
            isAccelerating = false;

        }
    }

    /// <summary>
    /// Handles end of boost
    /// </summary>
    public void ReleaseBoost()
    {
        SetCooldown(false, accelerationRatio);

        isAccelerating = false;
        accelerationRatio = 0f;
        accelerationTimer = 0f;
        currentSpeed = baseSpeed;

    }

    /// <summary>
    /// Optional - Handles acceleration and deceleration over time
    /// </summary>
    public void AccelerationHandler(float delta)
    {
        if (isAccelerating)
        {
            accelerationTimer = Mathf.Clamp(accelerationTimer + delta / 2, 0f, accelerationDuration);
        }
        else
        {
            accelerationTimer = Mathf.Clamp(accelerationTimer - delta / 2, 0f, accelerationDuration);
        }

        accelerationRatio = accelerationTimer / accelerationDuration;

        float curveValue = accelerationCurve.Evaluate(accelerationRatio);
        currentSpeed = Mathf.Lerp(baseSpeed, boostSpeed, curveValue);
    }

    /// <summary>
    /// Handles cooldown management
    /// </summary>
    public bool CooldownHandler(float delta)
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer = Mathf.Clamp(cooldownTimer - delta, 0f, overheatCooldown);
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Set cooldown after a while, handling overheat
    /// </summary>
    void SetCooldown(bool overheat, float ratio)
    {
        if (overheat)
        {
            cooldownTimer = overheatCooldown;
        }
        else
        {
            cooldownTimer = Mathf.Lerp(cooldownDurationMinMax.x, cooldownDurationMinMax.y, ratio);
        }
    }
}