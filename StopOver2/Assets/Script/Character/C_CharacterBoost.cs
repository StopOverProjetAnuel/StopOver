using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterBoost : C_CharacterManager
{
    

    public float boostForce;
    public float speedBoost;
    public float timeAccelerationBoost;
    public float durasionBoost;
    public float maxCooldownBoost;
    public float minCooldownBoost;
    public float surchaufeCooldownBoost;


    private float currentColdownBoost;
    private float timeUsBoost;
    private bool boostActiv;
    private bool firstImpulsBoostDone;
    private bool accelerationBoostDone;
    private bool canFirstImpuls;


    private float t2;
    // Start is called before the first frame update
    void Start()
    {
        canFirstImpuls = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGrounded() == true)
        {
            if(_CharacterInput.boostInput > 0 && canFirstImpuls)
            {
                FirstImpulsBoost(true);
            }

            if (firstImpulsBoostDone)
            {
                Boost(true);
            }
        }

    }

    private void Boost(bool caBombarde)
    {
        if (caBombarde)
        {
            if (t2 >= 1f)
            {
                accelerationBoostDone = true;
                t2 = 0;
            }
            if (!accelerationBoostDone && firstImpulsBoostDone)
            {
                t2 += Time.deltaTime / timeAccelerationBoost;
                _CharacterControler.currentSpeedForward = Mathf.SmoothStep(_CharacterControler.speedForward, speedBoost, t2);
            }
            else if (accelerationBoostDone)
            {
                _CharacterControler.currentSpeedForward = speedBoost;
                firstImpulsBoostDone = false;
                canFirstImpuls = true;
                caBombarde = false;
            }
        }


    }

    private void FirstImpulsBoost(bool firstImpulsBoost)
    {
        rb.AddForce(transform.TransformDirection(Vector3.forward) * boostForce, ForceMode.Impulse);
        accelerationBoostDone = false;
        firstImpulsBoostDone = true;
        canFirstImpuls = false;
        firstImpulsBoost = false;
    }
}
