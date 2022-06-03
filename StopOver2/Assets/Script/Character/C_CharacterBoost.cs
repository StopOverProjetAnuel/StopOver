using UnityEngine;

public class C_CharacterBoost : MonoBehaviour
{
    [SerializeField] private bool debug = true;

    #region Scripts Used
    private C_CharacterControler _CharacterControler;
    private C_CharacterFX _CharacterFX;
    private C_CharacterPropulseur _CharacterPropulseur;
    private Fmod_MusicManager musicManager;
    #endregion

    private Rigidbody rb;

    [Header("Speed Parameters")]
    [SerializeField] private float boostSpeed = 15f;
    [SerializeField] private float accelerationInitialImpulse = 10f;
    private float baseSpeed = 5f;

    [Header("Overheat Parameters")]
    [SerializeField] private Vector2 cooldownDurationMinMax = new Vector2(1f, 2.5f);
    [SerializeField] private float overheatCooldown = 10f;

    [SerializeField] private float accelerationDuration = 1.75f;

    [SerializeField] private AnimationCurve accelerationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float accelerationTimer = 0f;
    private float cooldownTimer = 0f;
    private bool overheat = false;

    private float accelerationRatio = 0f;
    private bool isAccelerating = false;

    [Header("Boost Prevention Parameters")]
    [SerializeField] private float timePreventionUngrounded = 2f;
    private float timeBufferPrevention = 0f;
    private bool currentBoostPrevention = true;



    public void IniatiateBoostValue(C_CharacterControler characterControler, C_CharacterFX characterFX, C_CharacterPropulseur characterPropulseur, Rigidbody _rb, Fmod_MusicManager mManager) //Work with awake
    {
        _CharacterControler = characterControler;
        _CharacterFX = characterFX;
        _CharacterPropulseur = characterPropulseur;
        rb = _rb;

        baseSpeed = _CharacterControler.speedPlayer;

        musicManager = mManager;
    }

    public void TriggerBoost(bool boostBegan, bool boostHeld, bool boostEnd, bool isGrounded)
    {
        CheckBoostGrounded(isGrounded);
        BoostHandler(boostBegan, boostHeld, boostEnd);
    }

    /// <summary>
    /// Will check if the player is in the air and disable all boost effects and activation after x sec
    /// </summary>
    private void CheckBoostGrounded(bool isGrounded)
    {
        if (!isGrounded)
        {
            if (Time.time < timeBufferPrevention) return;

            currentBoostPrevention = false;
            _CharacterFX.DisableBoost();
            Debug.Log("call disable boost");
        }
        else
        {
            currentBoostPrevention = true;
            timeBufferPrevention = Time.time + timePreventionUngrounded;
        }
    }

    public void BoostHandler(bool boostBegan, bool boostHeld, bool boostEnd)
    {
        _CharacterFX.FovSpeed(rb.velocity.magnitude, (boostHeld && isAccelerating));

        if (!CooldownHandler(Time.fixedDeltaTime)) //Check if the boost is in cooldown, return if true
        {
            if (debug) Debug.Log("Cooldown : " + cooldownTimer);

            return;
        }

        if (currentBoostPrevention) _CharacterFX.SignBoost();

        if (boostBegan && boostHeld && currentBoostPrevention) //Check if the player push down the boost button
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
        _CharacterFX.EnableBoost();
        _CharacterFX.SupersonicBoom();

        isAccelerating = true;
    }

    /// <summary>
    /// Boost applied over time
    /// </summary>
    public void HeldBoost()
    {
        accelerationTimer = Mathf.Clamp(accelerationTimer + Time.fixedDeltaTime, 0f, accelerationDuration);
        accelerationRatio = accelerationTimer / accelerationDuration;

        float curveValue = accelerationCurve.Evaluate(accelerationRatio);
        _CharacterControler.speedPlayer = Mathf.Lerp(baseSpeed, boostSpeed, curveValue);

        _CharacterFX.EnableBoost();
        _CharacterFX.OverheatBoost(accelerationTimer, accelerationDuration);

        musicManager.boost = 1;
        _CharacterControler.boostMultiplier = 0f;
        _CharacterPropulseur.thrustersForceMultiplier = 1f;

        // Si on dépasse la durée d'acceleration
        if (accelerationTimer >= accelerationDuration)
        {
            isAccelerating = false;
            overheat = true;
            SetCooldown(overheat, accelerationRatio);
            accelerationTimer = 0f;

            _CharacterControler.speedPlayer = baseSpeed;

            _CharacterFX.DisableBoost();
            _CharacterFX.DesactiveBoostOverheat();

            musicManager.boost = 0;
            _CharacterControler.boostMultiplier = 1f;
            _CharacterPropulseur.thrustersForceMultiplier = 1f;
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

        _CharacterControler.speedPlayer = baseSpeed;

        _CharacterFX.DisableBoost();

        musicManager.boost = 0;
        _CharacterControler.boostMultiplier = 1f;
        _CharacterPropulseur.thrustersForceMultiplier = 1f;
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