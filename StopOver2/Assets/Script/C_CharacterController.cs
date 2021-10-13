using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterController : MonoBehaviour
{
    //public Transform transformBase;
    public Rigidbody rb;
    public GameObject centerGravity;
    public GameObject groundCheck;

    [Space]
    public GameObject[] arrayPropulsPoint;
    public GameObject[] arrayStabilisateurPoint;
    [Space]
    public float distanceNoControl;
    private float distanceGroundChara;
    [Space]
    public float speedForward;
    public float speedBackward;
    public float speedSize;
    [Space]
    public float normalDistanceCharacterGround;
    public float powerUpPropulseurBase;
    public float powerUpPropulseurNoControl;
    private float powerUpPropulseur;
    public float powerFloting;
    [SerializeField]private float currentPowerFloting;
    [SerializeField]private float timeTransitionPowerFloting;
    private float t = 0;
    private float currentPowerUpPropulseur;
    private float valueCurrentPowerUpPropulseur;
    [Space]
    public float distanceStabilisateur;
    public float powerUpStabilisateur;
    public int powerStabilisateur;
    [Space]
    public float rotationMaxX;
    public float rotationMaxx;
    private float rotationX;
    private float clampRotationX;
    private float angleCharacter;

    private float propulsAngleX;
    private bool noControl;
    private int beginNoControl;

    void Start()
    {
        rb.centerOfMass = centerGravity.transform.localPosition;
        currentPowerFloting = powerFloting;
    }


    /*void Update()
    {


        RaycastHit groundHit;
        if(Physics.Raycast(groundCheck.transform.position , transform.TransformDirection(Vector3.down), out groundHit, 10f))
        {
            //Debug.Log(groundHit.distance);
            if(groundHit.distance <= distanceNoControl)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * speedForward, transform.position);
                    //Debug.Log((Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * speedForward, transform.position));
                }
                if(Input.GetAxis("Vertical") < 0)
                {
                    rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * speedBackward, transform.position);
                }

            }
        }

        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * speedSize);



        foreach(GameObject propulsPoint in arrayPropulsPoint)
        {
            RaycastHit propulsPointHit;
            if(Physics.Raycast(propulsPoint.transform.position, transform.TransformDirection(Vector3.down), out propulsPointHit, normalDistanceCharacterGround))
            {
                float x = propulsPointHit.distance / normalDistanceCharacterGround;

                Debug.Log(x);

                valueCurrentPowerUpPropulseur = Mathf.Lerp(1f,0f, x);
                currentPowerUpPropulseur = Mathf.Lerp(0f, powerUpPropulseur, valueCurrentPowerUpPropulseur);

                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(normalDistanceCharacterGround - propulsPointHit.distance, powerFloting) / normalDistanceCharacterGround * currentPowerUpPropulseur, propulsPoint.transform.position);

            }
            //Debug.Log(hit.distance);
        }

        foreach(GameObject stabilisateurPoint in arrayStabilisateurPoint)
        {
            RaycastHit stabilisateurPointHit;
            if(Physics.Raycast(stabilisateurPoint.transform.position, transform.TransformDirection(Vector3.down),out stabilisateurPointHit, distanceStabilisateur))
            {
                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(distanceStabilisateur - stabilisateurPointHit.distance, powerStabilisateur) / distanceStabilisateur * powerUpStabilisateur, stabilisateurPoint.transform.position);
            }
        }


        //rb.AddForce(-Time.deltaTime * transform.TransformVector(Vector3.right) * transform.InverseTransformVector(rb.velocity).x * 5f);




    }*/


    private void FixedUpdate()
    {

        #region//CanMoveForwardBackward si il est a proximiter du sol 
        RaycastHit groundHit;
        if (Physics.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), out groundHit, 10f))
        {
            distanceGroundChara = groundHit.distance;

            if (distanceGroundChara <= distanceNoControl)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * speedForward, transform.position);

                }
                if (Input.GetAxis("Vertical") < 0)
                {
                    rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * speedBackward, transform.position);
                }

            }
        }
        #endregion


        // Move Size 
        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * speedSize);


        if (distanceGroundChara > distanceNoControl)
        {
            noControl = true;
            beginNoControl = 1;
            powerUpPropulseur = powerUpPropulseurNoControl;
            currentPowerFloting = 1;
        }
        else if(distanceGroundChara <= distanceNoControl)
        {
            noControl = false;
            
            powerUpPropulseur = powerUpPropulseurBase;
            currentPowerFloting = powerFloting;
        }




        if(!noControl && beginNoControl == 1)
        {
            if (t >= 1f)
            {
                t = 0;
            }

            t += Time.deltaTime / timeTransitionPowerFloting;

            currentPowerFloting = Mathf.SmoothStep(2, powerFloting, t);

            if(currentPowerFloting == powerFloting)
            {
                beginNoControl = 0;
            }
        }

        #region//Propulseur 
        foreach (GameObject propulsPoint in arrayPropulsPoint)
        {
            RaycastHit propulsPointHit;
            if (Physics.Raycast(propulsPoint.transform.position, transform.TransformDirection(Vector3.down), out propulsPointHit, normalDistanceCharacterGround))
            {
                float x = propulsPointHit.distance / normalDistanceCharacterGround;

                valueCurrentPowerUpPropulseur = Mathf.Lerp(1f, 0f, x);
                currentPowerUpPropulseur = Mathf.Lerp(0f, powerUpPropulseur, valueCurrentPowerUpPropulseur);

                rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(normalDistanceCharacterGround - propulsPointHit.distance, currentPowerFloting) / normalDistanceCharacterGround * currentPowerUpPropulseur, propulsPoint.transform.position);

            }
        }
        #endregion

        //Test Calmp angle X
        /*angleCharacter = this.transform.eulerAngles.x;
        Debug.Log(angleCharacter);
        angleCharacter = Mathf.Clamp(angleCharacter, 0f, 25f);
        this.transform.rotation = Quaternion.Euler(angleCharacter, this.transform.eulerAngles.y, this.transform.eulerAngles.z);*/
    }


  


}
