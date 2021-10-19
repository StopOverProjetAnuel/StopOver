using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CheckAngle : MonoBehaviour
{
    [Header("Parametre Calculate Angle Character / Ground")]
    [Space]
    public LayerMask layerGround;
    public float distanceCalculAngleGround;
    public float angleGround;
    public GameObject pointCalculeAngleCharacter;
    public float angleCharacter;
    public float maxAngleCharacter;
    public bool toMushAngleOnCharacter;
    [Space]
    [Header("")]
    [Space]
    public GameObject pointCheckUp;
    public GameObject pointCheckDown;
    public float maxDistanceToCheck;
    public bool isUp;
    public bool isDown;

    private RaycastHit checkUpHit;
    private RaycastHit checkDownHit;


    private RaycastHit angleGroundHit;
    private RaycastHit angleCharacterHit;
    // Start is called before the first frame update
    void Start()
    {
        isDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateAngle();
            CheckDown();
        

    }

    private void CalculateAngle()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out angleGroundHit, distanceCalculAngleGround))
        {
            angleGround = Vector3.Angle(angleGroundHit.normal, Vector3.up);
        }

        pointCalculeAngleCharacter.transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        if (Physics.Raycast(pointCalculeAngleCharacter.transform.position, Vector3.up, out angleCharacterHit, 2))
        {
            float currentAngleCharacter = Vector3.Angle(angleCharacterHit.normal, Vector3.down);
            if (currentAngleCharacter > 7.5f)
            {
                angleCharacter = currentAngleCharacter - angleGround;
            }
            else
            {
                angleCharacter = currentAngleCharacter;
            }

            if (angleCharacter >= maxAngleCharacter)
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
            Debug.Log("Is Check Up");
            isUp = true;
            isDown = false;
            return true;
        }
        else
        {
            isDown = true;
            isUp = false;
            CheckDown();
            return false;
        }
    }

    private void CheckDown()
    {
        if (isDown)
        {

            if (Physics.Raycast(pointCheckDown.transform.position, transform.TransformDirection(-Vector3.up), out checkDownHit, maxDistanceToCheck, layerGround))
            {
                Debug.Log("Is Check Down");
                isDown = true;
                isUp = false;
                pointCheckUp.SetActive(false);
                Debug.Log(checkDownHit.distance);
            }
            else if(checkDownHit.distance >= maxDistanceToCheck -0.25f)
            {
                Debug.Log("Swith Down To Up");
                isUp = true;
                isDown = false;
                pointCheckUp.SetActive(true);
            }
        }

        if (isUp)
        {
            if(Physics.Raycast(pointCheckUp.transform.position, transform.TransformDirection(-Vector3.up), out checkUpHit, maxDistanceToCheck, layerGround))
            {
                Debug.Log("Is Check Up");
                isUp = true;
                isDown = false;
                pointCheckDown.SetActive(false);
                //Debug.Log(checkUpHit.distance);
            }
            else if(checkUpHit.distance >= maxDistanceToCheck - 0.25f)
            {
                Debug.Log("Swith Up to Down");
                isDown = true;
                isUp = false;
                pointCheckDown.SetActive(true);
            }

        }
 
    }
}
