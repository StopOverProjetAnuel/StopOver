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

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float mouseXInput;
    [HideInInspector] public float boostInput;

    [Header("Input Parameters")]
    public string boostInputName = "Boost";


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

    public bool isLowSpeed;
    public bool isAvarageSpeed;
    public bool isHighSpeed ;


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

        #region Get Input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseXInput = Input.GetAxis("Mouse X");
        boostInput = Input.GetAxis(boostInputName);
        #endregion

        #region Get Object
        rb = GetComponent<Rigidbody>();

        groundCheck = GameObject.Find("GroundCheck");
        centerOfMass = GameObject.Find("CenterOfMass");
        centerOfMassBack = transform.Find("CenterOfMassBack");
        #endregion

        #region Initiate Module Script Value
        _CharacterBoost.IniatiateBoostValue();
        _CharacterFX.InitiateFXValue(_CharacterBoost);
        #endregion
    }

    void Update()
    {
        rb.centerOfMass = centerOfMass.transform.localPosition;

        CheckGrounded();
        CalculSpeedCharcter();

        _CharacterControler.TriggerControl(CheckGrounded(), verticalInput, rb);
        _CharacterPropulseur.Propulsing();
    }

    private void FixedUpdate()
    {
        _CharacterControler.TriggerFixedControl(CheckGrounded(), verticalInput, mouseXInput, rb);
    }

    private void CalculSpeedCharcter()
    {
        currentSpeed = Mathf.Clamp(rb.velocity.magnitude, 0, Mathf.Infinity);
        int currentSpeedInt = Mathf.RoundToInt(currentSpeed);

        if(currentSpeed >= basseSpeed)
        {
            isLowSpeed = true;
        }
        else
        {
            isLowSpeed = false;
        }

        if(currentSpeed >= moyenSpeed && currentSpeed > basseSpeed)
        {
            isAvarageSpeed = true;
        }
        else
        {
            isAvarageSpeed = false;
        }

        if(currentSpeed >= hautSpeed && currentSpeed > moyenSpeed)
        {
            isHighSpeed = true;
        }
        else
        {
            isHighSpeed = false;

        }
    }

    public bool CheckGrounded()
    {
        RaycastHit groundHit;
        if (Physics.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), out groundHit, distanceNoControl, layerGround))
        {
            distanceGroundChara = groundHit.distance;

            return true;

        }
        else
        {
            return false;
        }
    }
}
