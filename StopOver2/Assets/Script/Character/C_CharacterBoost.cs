using System.Collections;
using UnityEngine;

public class C_CharacterBoost : MonoBehaviour
{
    public float boostForce;
    public float speedBoost;
    public float timeAccelerationBoost;
    public float durasionBoost;
    public float minCooldownBoost;
    public float maxCooldownBoost;
    public float surchaufeCooldownBoost;

    public float currentColdownBoost;
    private float timeUsBoost;
    [HideInInspector] public bool boostActiv;
    private bool firstImpulsBoostDone;
    private bool accelerationBoostDone;
    private bool canFirstImpuls;


    public  float t2;
    // Start is called before the first frame update
    public void IniatiateBoostValue()
    {
        canFirstImpuls = true;
        boostActiv = true;
    }

    // Update is called once per frame
    public void TriggerBoost(float inputDirectionNorm, bool isGrounded, Rigidbody rb, float currentSpeedPlayer)
    {
        if (isGrounded && boostActiv)
        {
            if (inputDirectionNorm > 0 && canFirstImpuls)
            {
                FirstImpulsBoost(true, rb);
            }


            if (inputDirectionNorm > 0 && firstImpulsBoostDone)
            {
                Boost(true, currentSpeedPlayer);
            }
            else
            {
                StartCoroutine(CooldownBoost(currentSpeedPlayer));
            }
        }
    }

    private void Boost(bool caBombarde, float currentSpeedPlayer)
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
                currentSpeedPlayer = Mathf.SmoothStep(currentSpeedPlayer, speedBoost, t2);

                currentColdownBoost = Mathf.Lerp(minCooldownBoost, maxCooldownBoost, t2);
            }
            else if (accelerationBoostDone)
            {
                currentColdownBoost = surchaufeCooldownBoost;

                StartCoroutine(CooldownBoost(currentSpeedPlayer));
                caBombarde = false;
            }
        }
    }

    private void FirstImpulsBoost(bool firstImpulsBoost, Rigidbody rb)
    {
        rb.AddForce(transform.TransformDirection(Vector3.forward) * boostForce, ForceMode.Impulse);
        accelerationBoostDone = false;
        firstImpulsBoostDone = true;
        canFirstImpuls = false;
        firstImpulsBoost = false;
    }

    IEnumerator CooldownBoost(float currentSpeedPlayer)
    {
        boostActiv = false;
        currentSpeedPlayer = speedBoost;

        yield return new WaitForSeconds(currentColdownBoost);
        t2 = 0;
        firstImpulsBoostDone = false;
        canFirstImpuls = true;
        Debug.Log("Cooldown Done");
        boostActiv = true;
    }
}
