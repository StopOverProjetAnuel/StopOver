using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_CharacterController2 : MonoBehaviour
{
    [Header("Gameobject")]
    [Space]
    public GameObject groundCheck;
    public GameObject centerOfMass;
    [Space]
    [Header("Parametre Propulseur")]
    [Space]
    public GameObject[] arrayPropulseurPointRight;
    public GameObject[] arrayPropulseurPointLeft;
    [Space]
    public float length;
    public float strengthRight;
    public float strengthLeft;
    public float divisionStrength;
    public float divisionStrengthAngle;
    [Space]
    [Header("Parametre Dampening")]
    [Space]
    public float dampening;
    public float dampeningRotate;
    [Space]
    [Header("Parametre General")]
    [Space]
    public Transform cameraTranform;
    public float distanceNoControl;
    [Space]
    [Header("Parametre Forward")]
    [Space]
    public float firstAccelerationForward;
    public float timeFirstAcceleration;
    public float speedForward;
    [Space]
    [Header("Parametre Backward")]
    [Space]
    public float speedBackward;
    [Space]
    [Header("Parametre Size")]
    [Space]
    public float speedSize;
    [Space]
    [Header("Parametre Boost")]
    [Space]
    public float boostForce;
    public float speedBoost;
    public float timeAccelerationBoost;
    public float durasionBoost;
    public float maxCooldownBoost;
    public float minCooldownBoost;
    public float surchaufeCooldownBoost;
    public ParticleSystem fxBoost;
    public ParticleSystem fxBoostColdownDone;
    public ParticleSystem fxBoostSurchaufe;
    public Material matSurchaufeMoteur;
    [Space]
    [Header("Parametre Air Control")]
    [Space]
    public bool isOnAir;
    public float airControlSpeedSize;
    public float airControlSpeedForward;
    [Space]
    [Header("Check Ground")]
    [Space]
    public float radiuSpherecast;
    public float maxDistanceSphereCast;
    public LayerMask layerGround;
    [Space]
    [Header("Check Angle")]
    [Space]
    public GameObject checkBackPoint;
    public float distanceCalculAngleGround;
    public GameObject pointCalculeAngleCharacter;
    public float angleGround;
    public float angleCharacter;
    public float maxAngleCharacter;
    public bool toMushAngleOnCharacter;
    public bool isOnBack;
   



    private Rigidbody rb;
    private float lastHitDistRight;
    private float lastHitDistLeft;

    private float currentStrengthRight;
    private float currentStrengthLeft;

    private float distanceGroundChara;

    private float currentDampening;

    private float currentSpeedForward;
    private bool firstAccelerationDone;
    private float t1;

    private float currentColdownBoost;
    private float timeUsBoost;
    private bool boostActiv;
    private bool firstImpulsBoostDone;
    private bool accelerationBoostDone;
    private bool canBoost = true;
    private bool boostColdown;
    private bool surchaufe;
    private bool transitionBoostToCooldown;
    private bool surchaufeMax;
    private Vector4 matMoteurCurrentSurchaufe;

    private float t2;
    private float t3;
    private float t4;
    

    private GameObject currentFaceHit;
    private bool isRotate;

    private RaycastHit angleGroundHit;
    private RaycastHit angleCharacterHit;

    private Quaternion cameraDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        currentStrengthLeft = strengthLeft;
        currentStrengthRight = strengthRight;

        rb.centerOfMass = centerOfMass.transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        matSurchaufeMoteur.color = Color.white;
        
        
    }

    // Update is called once per frame
    void Update()
    {

        #region Reset Scene
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("S_Gym_Etienne");
        }
        #endregion

        if (isOnAir)
        {
            AirControlle();

            RaycastHit hitBack;
            if (Physics.Raycast(checkBackPoint.transform.position, transform.TransformDirection(Vector3.up), out hitBack, 2f))
            {
                isOnBack = true;
            }
            else
            {
                isOnBack = false;
            }
        }

        CalculateAngle();

        if (isOnBack && Input.GetKeyDown(KeyCode.E))
        {
            transform.rotation = Quaternion.identity;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
    }

    public void FixedUpdate()
    {
        #region Lunch / Stop Boost
        
        if (Input.GetKey(KeyCode.Space) && canBoost)
        {
            fxBoost.Play();
            boostActiv = true;
            transitionBoostToCooldown = true;
            Boost(true);
            timeUsBoost += Time.deltaTime / durasionBoost;
        }
        else //if(!canBoost)
        {
            Boost(false);
            fxBoost.Stop();
            firstImpulsBoostDone = false;
            accelerationBoostDone = false;
            boostActiv = false;
        }
        #endregion

        #region Direction Character Camera
        cameraDirection = Quaternion.AngleAxis(cameraTranform.rotation.eulerAngles.y, Vector3.up);
        cameraDirection.x = transform.localRotation.x;
        cameraDirection.z = transform.localRotation.z;
        transform.rotation = cameraDirection;
        //Debug.Log(cameraDirection);
        #endregion

        #region Boost duration Acceleration / Cooldown Boost / Surchaufe
        if (boostActiv)
        {
            if(t3 >= 1f)
            {
                t3 = 0;
                surchaufe = true;
                surchaufeMax = true;
                fxBoostSurchaufe.Play();
                boostColdown = true;
                matMoteurCurrentSurchaufe = matSurchaufeMoteur.color;
                canBoost = false;
                boostActiv = false;
            }
            t3 += Time.deltaTime / durasionBoost;
            if (!surchaufe)
            {
                matSurchaufeMoteur.color = Color.Lerp(Color.white, Color.red, timeUsBoost);
                matMoteurCurrentSurchaufe = matSurchaufeMoteur.color;
            }
        }
        else if(!boostActiv && transitionBoostToCooldown)
        {
            t3 = 0;
            surchaufe = true;
            boostColdown = true;
            matMoteurCurrentSurchaufe = matSurchaufeMoteur.color;
            canBoost = false;
            transitionBoostToCooldown = false;
        }         
        
        
        if (boostColdown)
        {
            
            if (t4 >= 1f)
            {
                t4 = 0;
                surchaufe = false;
                surchaufeMax = false;
                fxBoostColdownDone.Play();
                timeUsBoost = 0;
                canBoost = true;
                boostColdown = false;
                
            }
            if (!surchaufeMax)
            {
             
                currentColdownBoost = Mathf.Lerp(0.5f, maxCooldownBoost, timeUsBoost);
            }else if (surchaufeMax)
            {
                currentColdownBoost = surchaufeCooldownBoost;
            }

            
            t4 += Time.deltaTime / currentColdownBoost;

            if (surchaufe)
            {
                matSurchaufeMoteur.color = Color.Lerp(matMoteurCurrentSurchaufe, Color.white, t4);

            }
        }
        #endregion

        #region Acceleration Forward / Forward / Backward 
        RaycastHit groundHit;
        if (Physics.Raycast(groundCheck.transform.position, transform.TransformDirection(Vector3.down), out groundHit, 10f))
        {
            Debug.Log("Touch");
            distanceGroundChara = groundHit.distance;

            if (distanceGroundChara <= distanceNoControl)
            {
                isOnAir = false;
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
            else
            {
                isOnAir = true;
            }
        }
        else
        {
            isOnAir = true;
        }
        #endregion

        #region Size
        //rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * speedSize);
        #endregion

        #region Gestion Dampening Virage / Strength Division Virage
        if (Input.GetAxis("Horizontal") < 0 && !toMushAngleOnCharacter)
        {
            currentStrengthLeft = strengthLeft / divisionStrength;
            currentStrengthRight = strengthRight;
            if (isRotate)
            {
                currentDampening = dampeningRotate;
                isRotate = false;
            }
        }else if(Input.GetAxis("Horizontal") > 0 && !toMushAngleOnCharacter)
        {
            currentStrengthRight = strengthRight /divisionStrength;
            currentStrengthLeft = strengthLeft;
            if (isRotate)
            {
                currentDampening = dampeningRotate;
                isRotate = false;
            }
        }
        else
        {
            currentDampening = dampening;
            isRotate = true;
            currentStrengthRight = strengthRight;
            currentStrengthLeft = strengthLeft;
        }
        #endregion

        if (toMushAngleOnCharacter)
        {
            currentStrengthLeft = strengthLeft / divisionStrengthAngle;
            currentStrengthRight = strengthRight / divisionStrengthAngle;
        }

        #region Gestion Propulseur Left / Right
        foreach (GameObject propulsPointRight in arrayPropulseurPointRight)
        {
            RaycastHit hit;
            if (Physics.Raycast(propulsPointRight.transform.position, transform.TransformDirection(-Vector3.up), out hit, length))
            {             

                float forceAmount = 0;
                forceAmount = currentStrengthRight * (length - hit.distance) / length + (currentDampening *(lastHitDistRight * hit.distance));
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
                forceAmount = currentStrengthLeft * (length - hit.distance) / length + (currentDampening * (lastHitDistLeft * hit.distance));
                rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPointLeft.transform.position);

                lastHitDistLeft = hit.distance;
            }
            else
            {
                lastHitDistLeft = length;
            }

        }
        #endregion
    }



    private void Boost(bool caBombarde)
    {
        if (caBombarde)
        {
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

    private void CalculateAngle()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out angleGroundHit, distanceCalculAngleGround))
        {
            angleGround = Vector3.Angle(angleGroundHit.normal, Vector3.up);
        }

        pointCalculeAngleCharacter.transform.position = new Vector3(transform.position.x,transform.position.y -1.5f, transform.position.z);
        if (Physics.Raycast(pointCalculeAngleCharacter.transform.position, Vector3.up, out angleCharacterHit, 2))
        {
            float currentAngleCharacter = Vector3.Angle(angleCharacterHit.normal, Vector3.down);
            if(currentAngleCharacter > 7.5f)
            {
                angleCharacter = currentAngleCharacter - angleGround;
            }
            else
            {
                angleCharacter = currentAngleCharacter;
            }

            if(angleCharacter >= maxAngleCharacter)
            {
                toMushAngleOnCharacter = true;
            }
            else
            {
                toMushAngleOnCharacter = false;
            }
        }
    }

    public void AirControlle()
    {
        Debug.Log("YAAAAAAAAAa");

        rb.AddTorque(Time.deltaTime * transform.TransformDirection(-Vector3.forward) * Input.GetAxis("Horizontal") * airControlSpeedSize);

        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.right) * -Input.GetAxis("Vertical") * airControlSpeedForward);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
