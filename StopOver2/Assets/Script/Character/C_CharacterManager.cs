using UnityEngine;

[RequireComponent(typeof(C_CharacterControler))]
[RequireComponent(typeof(C_CharacterBoost))]
[RequireComponent(typeof(C_CharacterPropulseur))]
[RequireComponent(typeof(C_CharacterCalculAngle))]
[RequireComponent(typeof(C_CharacterFX))]
[RequireComponent(typeof(C_CharacterSound))]
[RequireComponent(typeof(C_CharacterAnim))]
public class C_CharacterManager : MonoBehaviour
{
    #region Variables
    #region Script Used
    private C_CharacterControler _CharacterControler;
    private C_CharacterBoost _CharacterBoost;
    private C_CharacterPropulseur _CharacterPropulseur;
    private C_CharacterCalculAngle _CharacterCalculAngle;
    private C_CharacterFX _CharacterFX;
    private C_CharacterSound _CharacterSound;
    private C_CharacterAnim _CharacterAnime;
    private FMOD_FCManager musicManager;
    #endregion

    [Header("Object Require")]
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private GameObject centerOfMass;
    [SerializeField] private Transform centerOfMassBack;
    private Rigidbody rb;

    [Header("Input Parameters")]
    [SerializeField] private string boostInputName = "Boost";
    [SerializeField] private string boostInputName2 = "Boost2";
    [SerializeField] private float maxInputValue = 1f;
    private float horizontalValue = 0f;
    private float verticalValue = 0f;
    private float mouseXValue = 0f;
    private bool boostInputHold = false;
    private bool boostInputHold2 = false;
    private bool boostInputDown = false;
    private bool boostInputDown2 = false;
    private bool boostInputUp = false;
    private bool boostInputUp2 = false;


    [Header("Speed Parameters")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float lowSpeed;
    [SerializeField] private float midSpeed;
    [SerializeField] private float highSpeed;

    [Header("GroundChecker Parameters")]
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private float distanceNoControl;
    private float distanceGroundChara;
    private string groundTag;
    private bool isOnAir;
    private Quaternion airAngle = Quaternion.identity;

    [Header("Music Parameters")]
    [Tooltip("When the player hit that value with his speed (velocity) will trigger the max music intensity")]
    [SerializeField] private float maxMusicSpeed = 110f;
    private bool notRepeted = false;
    private bool notRepetedStrafe = false;
    #endregion



    private void Awake()
    {
        #region Get Component
        _CharacterControler = GetComponent<C_CharacterControler>();
        _CharacterBoost = GetComponent<C_CharacterBoost>();
        _CharacterPropulseur = GetComponent<C_CharacterPropulseur>();
        _CharacterCalculAngle = GetComponent<C_CharacterCalculAngle>();
        _CharacterFX = GetComponent<C_CharacterFX>();
        _CharacterSound = GetComponent<C_CharacterSound>();
        _CharacterAnime = GetComponent<C_CharacterAnim>();
        musicManager = FindObjectOfType<FMOD_FCManager>();
        #endregion

        #region Get Object
        rb = GetComponent<Rigidbody>();

        groundCheck = GameObject.Find("GroundCheck");
        centerOfMass = GameObject.Find("CenterOfMass");
        centerOfMassBack = transform.Find("CenterOfMassBack");
        #endregion

        #region Initiate Module Script Value
        _CharacterBoost.IniatiateBoostValue(_CharacterControler, _CharacterFX, _CharacterPropulseur, _CharacterSound, rb, musicManager);
        _CharacterFX.InitiateFXValue(_CharacterBoost);
        _CharacterSound.InitiateSoundValue();
        _CharacterPropulseur.InitiatePropulsorValue(rb);
        _CharacterControler.InitiateControlValue(rb);
        _CharacterCalculAngle.calculAngleGetProperties(rb);
        #endregion
    }

    private void Start()
    {
        _CharacterFX.SignBoost();
    }

    private void Update()
    {
        #region Get Input
        horizontalValue = Input.GetAxis("Horizontal"); //Get right & left Input
        verticalValue = Input.GetAxis("Vertical"); //Get forward & backward Input
        this.mouseXValue = Mathf.Clamp(Input.GetAxis("Mouse X"), -maxInputValue, maxInputValue); //Get mouse horizontal movement
        boostInputHold = Input.GetButton(boostInputName); //Get boost button hold
        boostInputHold2 = Input.GetButton(boostInputName2); //Get boost button hold
        boostInputDown = Input.GetButtonDown(boostInputName); //Get boost button when press
        boostInputDown2 = Input.GetButtonDown(boostInputName2); //Get boost button when press
        boostInputUp = Input.GetButtonUp(boostInputName); //Get boost button when press
        boostInputUp2 = Input.GetButtonUp(boostInputName2); //Get boost button when press
        #endregion

        currentSpeed = rb.velocity.magnitude;

        _CharacterBoost.TriggerBoost(boostInputDown || boostInputDown2, boostInputHold && boostInputHold2, boostInputUp || boostInputUp2, CheckGrounded(distanceNoControl));

        _CharacterFX.TriggerContinuousFX(CheckGrounded(distanceNoControl), groundTag);

        float mouseXValue = Mathf.Clamp(this.mouseXValue, -1, 1);
        _CharacterAnime.charaAnimCallEvents(mouseXValue);

        MusicCharacterUpdate();
    }

    private void MusicCharacterUpdate()
    {
        _CharacterSound.TriggerSound();

        float currentMusicIntensity = Mathf.Clamp01(currentSpeed / maxMusicSpeed) * 40;
        musicManager.intensity = currentMusicIntensity;

        float currentEngineIntensity = Mathf.Clamp01(currentSpeed / maxMusicSpeed) * 135;
        _CharacterSound.engineIntensity = currentEngineIntensity;

        float ratioMaxSpeed = Mathf.Clamp01(currentSpeed / maxMusicSpeed);
        if (ratioMaxSpeed >= 1 && notRepeted)
        {
            _CharacterSound.maxSpeedTrigger();
            notRepeted = false;
        }
        else if (ratioMaxSpeed != 1 && !notRepeted) notRepeted = true;

        if (horizontalValue != 0 && notRepetedStrafe)
        {
            _CharacterSound.StrafeSoundStart();
            notRepetedStrafe = false;
        }
        else if (!notRepetedStrafe)
        {
            _CharacterSound.StrafeSoundStop();
            notRepetedStrafe = true;
        }
    }

    private void FixedUpdate()
    {
        rb.centerOfMass = centerOfMass.transform.localPosition; //Place center of mass (need cause all the "mass" are at the back of the pivot)

        CheckGrounded(distanceNoControl);

        _CharacterControler.TriggerControl(verticalValue, horizontalValue, distanceGroundChara, CheckGrounded(distanceNoControl), centerOfMass);
        _CharacterPropulseur.Propulsing();
        _CharacterControler.TriggerRotation(CheckGrounded(distanceNoControl), CheckGrounded(_CharacterPropulseur.floatingHeight), verticalValue, mouseXValue, airAngle);
        _CharacterControler.GravityFall(CheckGrounded(distanceNoControl), rb);
        _CharacterCalculAngle.calculAngleCallEvents();
    }


    public bool CheckGrounded(float rangeCheck)
    {
        RaycastHit groundHit;
        if (Physics.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), out groundHit, rangeCheck, layerGround.value))
        {
            distanceGroundChara = groundHit.distance;
            groundTag = groundHit.transform.tag;
            LastAirAngleChara();
            return true;
        }
        else
        {
            Physics.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), out groundHit, Mathf.Infinity, layerGround.value);
            distanceGroundChara = groundHit.distance;
            return false;
        }
    }


    private void LastAirAngleChara()
    {
        airAngle = transform.localRotation;
    }
}
