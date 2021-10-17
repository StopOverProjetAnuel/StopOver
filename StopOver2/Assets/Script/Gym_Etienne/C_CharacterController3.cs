using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_CharacterController3 : MonoBehaviour
{
    [Header("Gameobject")]
    [Space]
    public GameObject groundCheck;
    public GameObject centerOfMass;
    public GameObject capsule;
    public float distanceCapsule;
    [Space]
    [Header("Parametre Propulseur")]
    [Space]
    public GameObject[] arrayPropulseurPointRightDown;
    public GameObject[] arrayPropulseurPointLeftDown;
    public GameObject[] arrayPropulseurPointRightUp;
    public GameObject[] arrayPropulseurPointLeftUp;
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
    [Header("Check Ground")]
    [Space]
    public float radiuSpherecast;
    public float maxDistanceSphereCast;
    public LayerMask layerGround;
    [Space]
    [Header("Parametre Calculate Angle Character / Ground")]
    [Space]
    public float distanceCalculAngleGround;
    public float angleGround;
    public GameObject pointCalculeAngleCharacter;
    public float angleCharacter;
    public float maxAngleCharacter;
    [Space]
    [Header("")]
    [Space]
    public GameObject pointCheckUp;
    public GameObject pointCheckDown;
    public float maxDistanceToCheck;




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
    [SerializeField] private bool toMushAngleOnCharacter;

    private RaycastHit checkUpHit;
    private RaycastHit checkDownHit;
    [SerializeField] private bool isUp;
    [SerializeField] private bool isDown;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        currentStrengthLeft = strengthLeft;
        currentStrengthRight = strengthRight;

        rb.centerOfMass = centerOfMass.transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        matSurchaufeMoteur.color = Color.white;

        CheckUp();
        CheckDown();
    }

    // Update is called once per frame
    void Update()
    {
        #region Check Ground + Constraint RB
        RaycastHit faceHit;
        if(Physics.SphereCast(centerOfMass.transform.position, radiuSpherecast , transform.TransformDirection(-Vector3.up),out faceHit, maxDistanceSphereCast, layerGround))
        {
            currentFaceHit = faceHit.transform.gameObject;
            rb.constraints = RigidbodyConstraints.None;            
        }
        else
        {
            //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        #endregion

        #region Reset Scene
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("S_Gym_Etienne");
        }
        #endregion

        CalculateAngle();
        CheckDown();
        CheckUp();

        //capsule.transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z + distanceCapsule);
        //capsule.transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
        //capsule.transform.rotation = Quaternion.EulerRotation(new Vector3(0f, transform.rotation.y - 90f, 0f));
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
        #endregion

        #region Size
        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * speedSize);
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

        #region Gestion Strength Propulseur en fonction de l'angle du véhicule
        if (toMushAngleOnCharacter)
        {
            currentStrengthLeft = strengthLeft / divisionStrengthAngle;
            currentStrengthRight = strengthRight / divisionStrengthAngle;
        }
        #endregion

        #region Gestion Propulseur Left / Right
        if (isDown)
        {
            Debug.Log("Propulseur Down");
            foreach (GameObject propulsPointRightDown in arrayPropulseurPointRightDown)
            {
                RaycastHit hit;
                if (Physics.Raycast(propulsPointRightDown.transform.position, transform.TransformDirection(-Vector3.up), out hit, length))
                {             

                    float forceAmount = 0;
                    forceAmount = currentStrengthRight * (length - hit.distance) / length + (currentDampening *(lastHitDistRight * hit.distance));
                    rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPointRightDown.transform.position);

                    lastHitDistRight = hit.distance;
                }
                else
                {
                    lastHitDistRight = length;
                }

            }

            foreach (GameObject propulsPointLeftDown in arrayPropulseurPointLeftDown)
            {
                RaycastHit hit;
                if (Physics.Raycast(propulsPointLeftDown.transform.position, transform.TransformDirection(-Vector3.up), out hit, length))
                {

                    float forceAmount = 0;
                    forceAmount = currentStrengthLeft * (length - hit.distance) / length + (currentDampening * (lastHitDistLeft * hit.distance));
                    rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPointLeftDown.transform.position);

                    lastHitDistLeft = hit.distance;
                }
                else
                {
                    lastHitDistLeft = length;
                }

            }
        }else if (isUp)
        {
            
            foreach (GameObject propulsPointLeftUp in arrayPropulseurPointLeftUp)
            {
                RaycastHit hit;
                if (Physics.Raycast(propulsPointLeftUp.transform.position, transform.TransformDirection(Vector3.up), out hit, length))
                {
                    Debug.Log("hit1");
                    float forceAmount = 0;
                    forceAmount = currentStrengthLeft * (length - hit.distance) / length + (currentDampening * (lastHitDistLeft * hit.distance));
                    rb.AddForceAtPosition(-transform.up * forceAmount * rb.mass, propulsPointLeftUp.transform.position);

                    lastHitDistLeft = hit.distance;
                }
                else
                {
                    lastHitDistLeft = length;
                }

            }

            foreach (GameObject propulsPointRighttUp in arrayPropulseurPointRightUp)
            {
                RaycastHit hit;
                if (Physics.Raycast(propulsPointRighttUp.transform.position, transform.TransformDirection(Vector3.up), out hit, length))
                {
                    Debug.Log("hit2");

                    float forceAmount = 0;
                    forceAmount = currentStrengthRight * (length - hit.distance) / length + (currentDampening * (lastHitDistRight * hit.distance));
                    rb.AddForceAtPosition(-transform.up * forceAmount * rb.mass, propulsPointRighttUp.transform.position);

                    lastHitDistRight = hit.distance;
                }
                else
                {
                    lastHitDistRight = length;
                }

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

    private bool CheckUp()
    {
        if (Physics.Raycast(pointCheckUp.transform.position, Vector3.up, out checkUpHit, maxDistanceToCheck, layerGround))
        {
            isUp = true;
            isDown = false;
            return true;
        }
        return false;
    }

    private bool CheckDown()
    {
        if (Physics.Raycast(pointCheckDown.transform.position, -Vector3.up, out checkDownHit, maxDistanceToCheck, layerGround))
        {
            isDown = true;
            isUp = false;
            return true;
        }
        else
        {
            isUp = true;
            isDown = false;
        }
        return false;
    }
}
