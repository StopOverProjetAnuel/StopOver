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


    public float currentColdownBoost;
    private float timeUsBoost;
    private bool boostActiv;
    private bool firstImpulsBoostDone;
    private bool accelerationBoostDone;
    private bool canFirstImpuls;


    public  float t2;
    // Start is called before the first frame update
    void Start()
    {
        canFirstImpuls = true;
        boostActiv = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGrounded() == true && boostActiv)
        {
            if(_CharacterInput.boostInput > 0 && canFirstImpuls)
            {
                FirstImpulsBoost(true);
            }


            if (_CharacterInput.boostInput > 0 && firstImpulsBoostDone)
            {
                Boost(true);
            }
            else
            {
                StartCoroutine(CooldownBoost());
            }
        }

        if (!boostActiv)
        {
            _CharacterFX.SurchauffeBoostDecres();

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

                _CharacterFX.SurchauffeBoost();

                currentColdownBoost = Mathf.Lerp(minCooldownBoost, maxCooldownBoost, t2);
            }
            else if (accelerationBoostDone)
            {
                currentColdownBoost = surchaufeCooldownBoost;

                _CharacterFX.DesactiveBosstSurchauffe();

                StartCoroutine(CooldownBoost());
                caBombarde = false;
            }
        }


    }

    private void FirstImpulsBoost(bool firstImpulsBoost)
    {
        _CharacterFX.ActiveBoost();

        rb.AddForce(transform.TransformDirection(Vector3.forward) * boostForce, ForceMode.Impulse);
        accelerationBoostDone = false;
        firstImpulsBoostDone = true;
        canFirstImpuls = false;
        firstImpulsBoost = false;
    }

    IEnumerator CooldownBoost()
    {
        boostActiv = false;
        _CharacterControler.currentSpeedForward = speedBoost;
        
        _CharacterFX.DesactiveBoost();

        yield return new WaitForSeconds(currentColdownBoost);
        t2 = 0;
        firstImpulsBoostDone = false;
        canFirstImpuls = true;
        Debug.Log("Cooldown Done");
        boostActiv = true;
    }
}
