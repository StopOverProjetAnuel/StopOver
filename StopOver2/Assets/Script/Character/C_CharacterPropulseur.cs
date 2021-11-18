using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterPropulseur : C_CharacterManager
{
    [SerializeField] Vector2 zRotMinMax = new Vector2(-30f, 30f);
    [SerializeField] Vector2 xRotMinMax = new Vector2(-30f, 30f);

    public GameObject[] arrayPropulseurPointRight;
    public GameObject[] arrayPropulseurPointLeft;

    public float length;
    public float strengthRight;
    public float strengthLeft;

    public float multiStrength;
    public float divisionStrength;
    public float multiLenght;

    public float timeTransitionLean;

    public float dampening;

    private float lastHitDistRight;
    private float currentStrengthRight;

    private float lastHitDistLeft;
    private float currentStrengthLeft;

    private float currentDampening;
    private float currentLenghtRight;
    private float currentLenghtLeft;


    private float t1;
    private float currentTimeTransitionLean;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (_CharacterInput.horizontalInput < 0)
        {


            if(currentTimeTransitionLean >= 1)
            {
                currentTimeTransitionLean = 1;
            }
            else
            {
                t1 += Time.deltaTime;
                currentTimeTransitionLean = t1 / timeTransitionLean;
            }

            currentLenghtRight = Mathf.Lerp(length ,(length * multiLenght), currentTimeTransitionLean);
            currentLenghtLeft = Mathf.Lerp(length,(length / divisionStrength), currentTimeTransitionLean);


            currentDampening = dampening;

            //Debug.Log("lenght Right" + currentLenghtRight);
        }
        else if (_CharacterInput.horizontalInput > 0)
        {


            if (currentTimeTransitionLean >= 1)
            {
                currentTimeTransitionLean = 1;
            }
            else
            {
                t1 += Time.deltaTime;
                currentTimeTransitionLean = t1 / timeTransitionLean;
            }

            currentLenghtLeft = Mathf.Lerp(length,(length * multiLenght), currentTimeTransitionLean);
            currentLenghtRight = Mathf.Lerp(length,(length / divisionStrength), currentTimeTransitionLean);

            currentDampening = dampening;

            //Debug.Log("lenght Left" + currentLenghtLeft);
        }
        else
        {
            currentTimeTransitionLean = 0;
            t1 = 0;
            currentLenghtRight = length;
            currentLenghtLeft = length;
            currentDampening = 0;
            currentStrengthRight = strengthRight;
            currentStrengthLeft = strengthLeft;
        }*/

        currentTimeTransitionLean = 0;
        t1 = 0;
        currentLenghtRight = length;
        currentLenghtLeft = length;
        currentDampening = 0;
        currentStrengthRight = strengthRight;
        currentStrengthLeft = strengthLeft;

        CheckPropulsors(arrayPropulseurPointLeft, currentLenghtLeft, currentStrengthLeft, ref lastHitDistLeft);
        CheckPropulsors(arrayPropulseurPointRight, currentLenghtRight, currentStrengthRight, ref lastHitDistRight);

        //LockRotation();
    }

    void CheckPropulsors(GameObject[] propulsors, float currentLength, float currentStrength, ref float lastHitDist)
    {
        foreach (GameObject propulsPoint in propulsors)
        {
            RaycastHit hit;
            if (Physics.Raycast(propulsPoint.transform.position, propulsPoint.transform.TransformDirection(-Vector3.up), out hit, currentLength))
            {

                lastHitDist = hit.distance;
                float forceAmount = 0;
                float lengthRatio = (currentLength - hit.distance) / currentLength;

                forceAmount = currentStrength * lengthRatio + (currentDampening * (lastHitDist * hit.distance));

                rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPoint.transform.position);
            }
            else
            {
                lastHitDist = currentLength;
            }

        }
    }

    void LockRotation()
    {
       float currentZRotation = transform.localEulerAngles.z;

        currentZRotation = Mathf.Clamp(currentZRotation, zRotMinMax.x, zRotMinMax.y);

       float currentXRotation = transform.localEulerAngles.x;
       currentXRotation = Mathf.Clamp(currentXRotation, xRotMinMax.x, xRotMinMax.y);

        Vector3 newRotation = new Vector3
            (
                currentXRotation,
                transform.localEulerAngles.y,
                currentZRotation
            );
        transform.localEulerAngles = newRotation;
    }
}
