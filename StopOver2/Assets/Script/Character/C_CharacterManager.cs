using UnityEngine;

[RequireComponent(typeof(C_CharacterControler))]
[RequireComponent(typeof(C_CharacterBoost))]
[RequireComponent(typeof(C_CharacterPropulseur))]
[RequireComponent(typeof(C_CharacterCalculAngle))]
[RequireComponent(typeof(C_CharacterFX))]
[RequireComponent(typeof(C_CharacterAnim))]
public class C_CharacterManager : MonoBehaviour
{
    #region Script Used
    C_CharacterControler _CharacterControler;
    C_CharacterBoost _CharacterBoost;
    C_CharacterPropulseur _CharacterPropulseur;
    C_CharacterCalculAngle _CharacterCalculAngle;
    C_CharacterFX _CharacterFX;
    C_CharacterAnim _CharacterAnime;
    #endregion

    [Header("Object Require")]
    public GameObject groundCheck;
    public GameObject centerOfMass;
    public Transform centerOfMassBack;


    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public float horizontalInput = 0f;
    [HideInInspector] public float verticalInput = 0f;
    [HideInInspector] public float mouseXInput = 0f;
    [HideInInspector] public bool boostInputHold = false;
    [HideInInspector] public bool boostInputHold2 = false;
    [HideInInspector] public bool boostInputDown = false;
    [HideInInspector] public bool boostInputDown2 = false;
    [HideInInspector] public bool boostInputUp = false;
    [HideInInspector] public bool boostInputUp2 = false;

    [Header("Input Parameters")]
    public string boostInputName = "Boost";
    public string boostInputName2 = "Boost2";
    public float maxInputValue = 1f;


    [Header("Parameters")]
    public LayerMask layerGround;
    public float distanceGroundChara;
    public float distanceNoControl;
    public bool isOnAir;
    [Space]
    public float currentSpeed;
    public float basseSpeed;
    public float moyenSpeed;
    public float hautSpeed;

    private Quaternion airAngle = Quaternion.identity;


    private void Awake()
    {
        #region Get Component
        _CharacterControler = GetComponent<C_CharacterControler>();
        _CharacterBoost = GetComponent<C_CharacterBoost>();
        _CharacterPropulseur = GetComponent<C_CharacterPropulseur>();
        _CharacterCalculAngle = GetComponent<C_CharacterCalculAngle>();
        _CharacterFX = GetComponent<C_CharacterFX>();
        _CharacterAnime = GetComponent<C_CharacterAnim>();
        #endregion

        #region Get Object
        rb = GetComponent<Rigidbody>();

        groundCheck = GameObject.Find("GroundCheck");
        centerOfMass = GameObject.Find("CenterOfMass");
        centerOfMassBack = transform.Find("CenterOfMassBack");
        #endregion

        #region Initiate Module Script Value
        _CharacterBoost.IniatiateBoostValue(_CharacterControler, _CharacterFX, rb);
        _CharacterFX.InitiateFXValue(_CharacterBoost);
        _CharacterPropulseur.InitiatePropulsorValue(rb);
        _CharacterControler.InitiateControlValue(rb);
        #endregion
    }

    private void Start()
    {
        _CharacterFX.SignBoost();
    }

    void Update()
    {
        #region Get Input
        horizontalInput = Input.GetAxis("Horizontal"); //Get right & left Input
        verticalInput = Input.GetAxis("Vertical"); //Get forward & backward Input
        mouseXInput = Mathf.Clamp(Input.GetAxis("Mouse X"), -maxInputValue, maxInputValue); //Get mouse horizontal movement
        boostInputHold = Input.GetButton(boostInputName); //Get boost button hold
        boostInputHold2 = Input.GetButton(boostInputName2); //Get boost button hold
        boostInputDown = Input.GetButtonDown(boostInputName); //Get boost button when press
        boostInputDown2 = Input.GetButtonDown(boostInputName2); //Get boost button when press
        boostInputUp = Input.GetButtonUp(boostInputName); //Get boost button when press
        boostInputUp2 = Input.GetButtonUp(boostInputName2); //Get boost button when press
        #endregion

        _CharacterBoost.TriggerBoost(boostInputDown || boostInputDown2, boostInputHold && boostInputHold2, boostInputUp || boostInputUp2);

        _CharacterFX.TriggerContinuousFX(Mathf.Clamp(mouseXInput, -1, 1), CheckGrounded());
        _CharacterFX.HandleTrailPlayer(currentSpeed);
    }

    private void FixedUpdate()
    {
        currentSpeed = rb.velocity.magnitude;
        rb.centerOfMass = centerOfMass.transform.localPosition; //Place center of mass (need cause all the "mass" are at the back of the pivot)

        CheckGrounded();

        _CharacterControler.TriggerControl(verticalInput, CheckGrounded(), centerOfMass);
        _CharacterPropulseur.Propulsing(layerGround, rb.velocity.magnitude);
        _CharacterControler.TriggerRotation(CheckGrounded(), verticalInput, mouseXInput, airAngle);
        _CharacterControler.GravityFall(CheckGrounded(), rb);
    }


    public bool CheckGrounded()
    {
        RaycastHit groundHit;
        if (Physics.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), out groundHit, distanceNoControl, layerGround.value))
        {
            distanceGroundChara = groundHit.distance;
            LastAirAngleChara();
            return true;
        }
        else
        {
            return false;
        }
    }


    private void LastAirAngleChara()
    {
        airAngle = transform.localRotation;
    }
}
