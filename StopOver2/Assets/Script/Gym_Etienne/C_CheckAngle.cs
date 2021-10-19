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

    public C_CharacterController3 _CharacterController3;

    private RaycastHit checkUpHit;
    private RaycastHit checkDownHit;


    private RaycastHit angleGroundHit;
    private RaycastHit angleCharacterHit;

    private int stayOnAir;
    // Start is called before the first frame update
    void Start()
    {
        isDown = true;
        stayOnAir = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateAngle();

        if (isUp)
        {
            StartCoroutine(CheckUp());
            StopCoroutine(CheckDown());
        }else if (isDown)
        {
            StartCoroutine(CheckDown());
            StopCoroutine(CheckUp());
        }
       


        if (!_CharacterController3.onAir)
        {
            if (isUp)
            {
                stayOnAir = 1;
            }else if (isDown)
            {
                stayOnAir = 2;
            }

        }

        if (_CharacterController3.onAir)
        {
            Debug.Log(stayOnAir);
            if(stayOnAir == 1)
            {
                isUp = true;
            }else if(stayOnAir == 2)
            {
                isDown = true;
            }

        }




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


    IEnumerator CheckUp()
    {
        if (isUp)
        {
            if (Physics.Raycast(pointCheckUp.transform.position, pointCheckUp.transform.TransformDirection(-Vector3.up), out checkUpHit, maxDistanceToCheck, layerGround))
            {

                isUp = true;
                isDown = false;
                pointCheckDown.SetActive(false);
                //Debug.Log(checkUpHit);
            }
            else
            {

                isDown = true;
                isUp = false;
                pointCheckDown.SetActive(true);
            }

        }

        yield return false;
    }

    IEnumerator CheckDown()
    {
        if (isDown)
        {

            if (Physics.Raycast(pointCheckDown.transform.position, pointCheckDown.transform.TransformDirection(-Vector3.up), out checkDownHit, maxDistanceToCheck, layerGround))
            {

                isDown = true;
                isUp = false;
                pointCheckUp.SetActive(false);

            }
            else
            {

                isUp = true;
                isDown = false;
                pointCheckUp.SetActive(true);
            }
        }

        yield return false;
    }

}
