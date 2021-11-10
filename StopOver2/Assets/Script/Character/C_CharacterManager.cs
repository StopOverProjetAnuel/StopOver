using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterManager : MonoBehaviour
{
    [Header("Character Script")]
    public C_CharacterControler _CharacterControler;
    public C_CharacterBoost _CharacterBoost;
    public C_CharacterInput _CharacterInput;
    public C_CharacterFX _CharacterFX;
    public C_CharacterPropulseur _CharacterPropulseur;
    public C_CharacterCalculAngle _CharacterCalculAngle;
    public C_CharacterAnime _CharacterAnime;

    public GameObject groundCheck;
    public GameObject centerOfMass;

    public Rigidbody rb;

    public LayerMask layerGround;
    public float distanceGroundChara;
    public float distanceNoControl;
    public bool isOnAir;


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        groundCheck = GameObject.Find("GroundCheck");
        centerOfMass = GameObject.Find("CenterOfMass");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.centerOfMass = centerOfMass.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit groundHit;
        if (Physics.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), out groundHit, 10f, layerGround))
        {
            distanceGroundChara = groundHit.distance;

            if (distanceGroundChara <= distanceNoControl)
            {
                isOnAir = false;
            }else if (distanceGroundChara > distanceNoControl)
            {
                isOnAir = true;
            }
        }
    }

    private void FixedUpdate()
    {
        transform.rotation = _CharacterInput.cameraDirection;
    }
}
