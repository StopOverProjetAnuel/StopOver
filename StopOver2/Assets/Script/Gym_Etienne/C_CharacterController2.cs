using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterController2 : MonoBehaviour
{
    public float length;
    public float strengthRight;
    public float strengthLeft;
    public float dampening;
    public GameObject groundCheck;
    public GameObject centerOfMass;
    public float distanceNoControl;

    public float firstAccelerationForward;
    public float speedForward;
    public float speedBoost;
    public float speedBackward;
    public float speedSize;
    public float timeFirstAcceleration;
    public float timeAccelerationBoost;
    public float durasionBoost;
    public float cooldownBoost;

    public float boostForce;

    public float divisionStrength;

    public GameObject[] arrayPropulseurPointRight;
    public GameObject[] arrayPropulseurPointLeft;

    public float radiuSpherecast;
    public float maxDistanceSphereCast;
    public LayerMask layerGround;

    public ParticleSystem boost;

    private Rigidbody rb;
    private float lastHitDistRight;
    private float lastHitDistLeft;
    private float distanceGroundChara;

    private float currentStrengthRight;
    private float currentStrengthLeft;

    private float t1;
    private float currentSpeedForward;
    private bool firstAccelerationDone;

    private bool boostActiv;
    private bool firstImpulsBoostDone;
    private float t2;
    private bool accelerationBoostDone;
    private bool canBoost = true;
    private float t3;

    private GameObject currentFaceHit;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        currentStrengthLeft = strengthLeft;
        currentStrengthRight = strengthRight;

        rb.centerOfMass = centerOfMass.transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("speed =" + currentSpeedForward);
        //Debug.Log("firstAccel = " + firstAccelerationDone);
        //Debug.Log("t = " + t);

        RaycastHit faceHit;
        if(Physics.SphereCast(centerOfMass.transform.position, radiuSpherecast , transform.TransformDirection(-Vector3.up),out faceHit, maxDistanceSphereCast, layerGround))
        {
            currentFaceHit = faceHit.transform.gameObject;
            rb.constraints = RigidbodyConstraints.None;
            Debug.Log(currentFaceHit.name);
            Debug.Log("C'est ok");
        }
        else
        {
            Debug.Log("c'est pas ok");
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && canBoost)
        {
            boostActiv = true;
            boost.Play();
            Boost();
        }
        else if(!canBoost || Input.GetKeyUp(KeyCode.Space))
        {
            boost.Stop();
            //Debug.Log("Boost over");
            canBoost = false;
            boostActiv = false;
            firstImpulsBoostDone = false;
            accelerationBoostDone = false;
        }

        if (boostActiv)
        {
            if(t3 >= 1f)
            {
                t3 = 0;
                canBoost = false;
            }
            t3 += Time.deltaTime / durasionBoost;
        }else if (!boostActiv)
        {
            if (t3 >= 1f)
            {
                //Debug.Log("Boost Regen");
                t3 = 0;
                canBoost = true;
            }
            t3 += Time.deltaTime / cooldownBoost;
        }

        RaycastHit groundHit;
        if (Physics.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), out groundHit, 10f))
        {
            distanceGroundChara = groundHit.distance;

            if (distanceGroundChara <= distanceNoControl)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    if (!boostActiv)
                    {
                        if(t1 >= 1f)
                        {
                            firstAccelerationDone = true;
                            t1 = 0;
                        }

                        if (!firstAccelerationDone)
                        {
                            t1 += Time.deltaTime / timeFirstAcceleration;
                            currentSpeedForward = Mathf.SmoothStep(firstAccelerationForward, speedForward, t1);
                        }else if (firstAccelerationDone)
                        {
                            currentSpeedForward = speedForward;
                        }

                    }

                    rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * currentSpeedForward, transform.position);
                    
                }
                if (Input.GetAxis("Vertical") < 0 && !boostActiv)
                {
                    firstAccelerationDone = false;
                    rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * speedBackward, transform.position);
                }
                if (Input.GetAxis("Vertical") == 0)
                {
                    firstAccelerationDone = false;
                }

            }
        }


        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * speedSize);

        if(Input.GetAxis("Horizontal") < 0)
        {
            currentStrengthLeft = strengthLeft / divisionStrength;
            currentStrengthRight = strengthRight;
        }else if(Input.GetAxis("Horizontal") > 0)
        {
            currentStrengthRight = strengthRight /divisionStrength;
            currentStrengthLeft = strengthLeft;
        }
        else
        {
            currentStrengthRight = strengthRight;
            currentStrengthLeft = strengthLeft;
        }

        foreach (GameObject propulsPointRight in arrayPropulseurPointRight)
        {
            RaycastHit hit;
            if (Physics.Raycast(propulsPointRight.transform.position, transform.TransformDirection(-Vector3.up), out hit, length))
            {             

                float forceAmount = 0;
                forceAmount = currentStrengthRight * (length - hit.distance) / length + (dampening *(lastHitDistRight * hit.distance));
                rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPointRight.transform.position);

                lastHitDistRight = hit.distance;
            }
            else
            {
                lastHitDistRight = length;
            }

        }

        foreach (GameObject propulsPointLeft in arrayPropulseurPointLeft)
        {
            RaycastHit hit;
            if (Physics.Raycast(propulsPointLeft.transform.position, transform.TransformDirection(-Vector3.up), out hit, length))
            {

                float forceAmount = 0;
                forceAmount = currentStrengthLeft * (length - hit.distance) / length + (dampening * (lastHitDistLeft * hit.distance));
                rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPointLeft.transform.position);

                lastHitDistLeft = hit.distance;
            }
            else
            {
                lastHitDistLeft = length;
            }

        }
    }



    private void Boost()
    {
        //Debug.Log("Boooooost!!!");
        if (!firstImpulsBoostDone)
        {
            rb.AddForce(transform.TransformDirection(Vector3.forward) * boostForce, ForceMode.Impulse);
            firstImpulsBoostDone = true;
        }

        if(t2 >= 1f)
        {
            accelerationBoostDone = true;
            t2 = 0;
        }
        if (!accelerationBoostDone)
        {
            t2 += Time.deltaTime / timeAccelerationBoost;
            currentSpeedForward = Mathf.SmoothStep(speedForward, speedBoost, t2);
        }else if (accelerationBoostDone)
        {
            currentSpeedForward = speedBoost;
        }

        
    }
}
