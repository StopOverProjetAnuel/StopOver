using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterPropulseur : C_CharacterManager
{
    

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

    private bool rightLean;
    private bool leftLean;

    private float t1;
    private float currentTimeTransitionLean;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_CharacterInput.horizontalInput < 0)
        {
            //rightLean = true;
            //leftLean = false;

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
            //currentLenghtRight = length * (multiLenght * - _CharacterInput.horizontalInput);
            //currentStrengthRight = strengthRight * multiStrength;
            //currentStrengthLeft = strengthLeft / divisionStrength;

            currentDampening = dampening;

            //Debug.Log("lenght Right" + currentLenghtRight);
        }
        else if (_CharacterInput.horizontalInput > 0)
        {
            //leftLean = true;
            //rightLean = false;

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
            //currentLenghtLeft = length * (multiLenght * _CharacterInput.horizontalInput);
            //currentStrengthLeft = strengthLeft * multiStrength;
            //currentStrengthRight = strengthRight / divisionStrength;

            currentDampening = dampening;

            //Debug.Log("lenght Left" + currentLenghtLeft);
        }
        else
        {
            //leftLean = false;
            //rightLean = false;
            currentTimeTransitionLean = 0;
            t1 = 0;
            currentLenghtRight = length;
            currentLenghtLeft = length;
            currentDampening = 0;
            currentStrengthRight = strengthRight;
            currentStrengthLeft = strengthLeft;
        }


        foreach (GameObject propulsPointRight in arrayPropulseurPointRight)
        {
            RaycastHit hit;
            if (Physics.Raycast(propulsPointRight.transform.position, transform.TransformDirection(-Vector3.up), out hit, currentLenghtRight))
            {

                lastHitDistRight = hit.distance;
                float forceAmount = 0;

                forceAmount = currentStrengthRight * (currentLenghtRight - lastHitDistRight) / currentLenghtRight + (currentDampening * (lastHitDistRight * hit.distance));

                //Debug.Log("lastHitDistanceRight" + lastHitDistRight);

                /*if (!rightLean)
                {
                    forceAmount = currentStrengthRight * (currentLenghtRight - lastHitDistRight) / currentLenghtRight + (currentDampening * (lastHitDistRight * hit.distance));
                }else if (rightLean)
                {
                    if (currentTimeTransitionLean >= 1)
                    {
                        forceAmount = currentStrengthRight;
                    }
                    else
                    {
                        t1 += Time.deltaTime;
                        currentTimeTransitionLean = t1 / timeTransitionLean;

                        forceAmount = Mathf.Lerp(forceAmount, currentStrengthRight, currentTimeTransitionLean);
                    }

                }*/
                rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPointRight.transform.position);
                //Debug.Log("Force Right = " + forceAmount);
            }
            else
            {
                lastHitDistRight = currentLenghtRight;
            }

        }

        foreach (GameObject propulsPointLeft in arrayPropulseurPointLeft)
        {
            RaycastHit hit;
            if (Physics.Raycast(propulsPointLeft.transform.position, transform.TransformDirection(-Vector3.up), out hit, currentLenghtLeft))
            {

                lastHitDistLeft = hit.distance;
                float forceAmount = 0;


                forceAmount = currentStrengthLeft * (currentLenghtLeft - hit.distance) / currentLenghtLeft + (currentDampening * (lastHitDistLeft * hit.distance));

                //Debug.Log("lastHitDistanceLeft" + lastHitDistLeft);

                /*if (!leftLean)
                {
                    forceAmount = currentStrengthLeft * (currentLenghtLeft - hit.distance) / currentLenghtLeft + (currentDampening * (lastHitDistLeft * hit.distance));
                }else if (leftLean)
                {

                    if(currentTimeTransitionLean >= 1)
                    {
                        forceAmount = currentStrengthLeft;
                    }
                    else
                    {
                        t1 += Time.deltaTime;
                        currentTimeTransitionLean = t1 / timeTransitionLean;
                        forceAmount = Mathf.Lerp(forceAmount, currentStrengthLeft, currentTimeTransitionLean);
                    }

                }*/
                rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPointLeft.transform.position);
                //Debug.Log("Force Left = " + forceAmount);
            }
            else
            {
                lastHitDistLeft = currentLenghtLeft;
            }

        }

        
    }
}
