using UnityEngine;

public class C_CharacterBoost : MonoBehaviour
{
    [SerializeField] bool debug = true;

    #region Scripts Used
    C_CharacterControler _CharacterControler;
    C_CharacterFX _CharacterFX;
    #endregion

    Rigidbody rb;

    [Header("Speed Parameters")]
    [SerializeField] private float boostSpeed = 15f;
    [SerializeField] private float accelerationInitialImpulse = 10f;
    private float baseSpeed = 5f;

    [Header("Overheat Parameters")]
    [SerializeField] Vector2 cooldownDurationMinMax = new Vector2(1f, 2.5f);
    [SerializeField] float overheatCooldown = 10f;

    [SerializeField] float accelerationDuration = 1.75f;

    [SerializeField] AnimationCurve accelerationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    float accelerationTimer = 0f;
    float cooldownTimer = 0f;
    bool overheat = false;

    float accelerationRatio = 0f;
    bool isAccelerating = false;

    public void IniatiateBoostValue(C_CharacterControler characterControler, C_CharacterFX characterFX, Rigidbody _rb) //Work with awake
    {
        _CharacterControler = characterControler;
        _CharacterFX = characterFX;
        rb = _rb;

        baseSpeed = _CharacterControler.speedPlayer;
    }

    public void TriggerBoost(bool boostBegan, bool boostHeld, bool boostEnd)
    {
        BoostHandler(boostBegan, boostHeld, boostEnd);
    }



    public void BoostHandler(bool boostBegan, bool boostHeld, bool boostEnd)
    {
        _CharacterFX.FovSpeed(rb.velocity.magnitude, (boostHeld && isAccelerating));

        if (!CooldownHandler(Time.fixedDeltaTime)) //Check if the boost is in cooldown, return if true
        {
            if (debug) Debug.Log("Cooldown : " + cooldownTimer);
            return;
        }

        if (boostBegan && boostHeld) //Check if the player push down the boost button
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

        if (isAccelerating && boostEnd)
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
        _CharacterControler.currentSpeed = Mathf.Lerp(baseSpeed, boostSpeed, curveValue);

        _CharacterFX.ActiveBoost();
        _CharacterFX.OverheatBoost(accelerationTimer, accelerationDuration);

        // Si on dépasse la durée d'acceleration
        if (accelerationTimer >= accelerationDuration)
        {
            isAccelerating = false;
            overheat = true;
            SetCooldown(overheat, accelerationRatio);
            accelerationTimer = 0f;

            _CharacterControler.currentSpeed = baseSpeed;

            _CharacterFX.DesactiveBoost();
            _CharacterFX.DesactiveBoostOverheat();
        }
    }

    /// <summary>
    /// Handles end of boost
    /// </summary>
    public void ReleaseBoost()
    {
        overheat = false;
        SetCooldown(overheat, accelerationRatio);

        isAccelerating = false;
        accelerationRatio = 0f;
        accelerationTimer = 0f;

        _CharacterControler.currentSpeed = baseSpeed;

        _CharacterFX.DesactiveBoost();
    }

    /// <summary>
    /// Handles cooldown management
    /// </summary>
    public bool CooldownHandler(float delta)
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer = Mathf.Clamp(cooldownTimer - delta, 0f, overheatCooldown);
            _CharacterFX.OverheatBoostDecres(cooldownTimer, cooldownDurationMinMax.y);
            return false;
        }
        else
        {
            overheat = false;
            _CharacterFX.SignBoost();
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