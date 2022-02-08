using UnityEngine;

public class C_CharacterBoost : MonoBehaviour
{
    #region Variables
    #region Scripts Used
    C_CharacterControler _CharacterControler;
    C_CharacterFX _CharacterFX;
    #endregion

    public float boostImpulseForce;
    public float speedBoost;
    public float timeAccBoost;
    private float currentTimeAccBoost = 0f;
    public float minCooldownBoost;
    public float maxCooldownBoost;
    public float surchaufeCooldownBoost;
    private float currentCooldownBoost = 0f;
    private float saveMaxCooldownBoost = 0f;
    private bool boostPressed = true;

    public bool showDebug = false;
    #region OLD
    /**private float timeUsBoost;
    [HideInInspector] public bool boostActiv;
    private bool firstImpulsBoostDone;
    private bool accelerationBoostDone;
    private bool canFirstImpuls;

    public bool isBoosted = false;

    public  float t2;*/
    #endregion
    #endregion


    public void IniatiateBoostValue() //Work with awake
    {
        _CharacterControler = GetComponent<C_CharacterControler>();
        _CharacterFX = GetComponent<C_CharacterFX>();

        boostPressed = true;
    }

    public void TriggerBoost(float getMoveForward, bool inputBoostTrigger, bool inputBoostTriggerContinue, Rigidbody rb)
    {
        if (getMoveForward > 0 && currentCooldownBoost == 0 && boostPressed) 
        { 
            if (inputBoostTrigger)
            {
                BoostImpulseForce(rb);
                Debug.Log("Trigger BoostImpulseForce");
            }
        
            if (inputBoostTriggerContinue && currentTimeAccBoost != timeAccBoost)
            {
                UseBoostSpeed();
                IncreesBoostTimer();
                Debug.Log("Trigger UseBoostSpeed & IncreesBoostTimer");
            }
            else if (!inputBoostTriggerContinue && _CharacterControler.currentSpeed != _CharacterControler.speedPlayer)
            {
                ResetBoostSpeed();
                Debug.Log("Trigger ResetBoostSpeed");
            }
        }
        else if (currentTimeAccBoost != 0)
        {
            ResetBoostSpeed();
        }
        else if (currentCooldownBoost != 0)
        {
            DecreesBoostCooldown();
            boostPressed = false;
            Debug.Log("Trigger DecreesBoostCooldown");
        }

        if (Input.GetButtonUp("Boost"))
        {
            boostPressed = true;
        }

        #region Debug
        if (showDebug)
        {
            Debug.Log("Boost button is active : " + inputBoostTriggerContinue);
            Debug.Log("Current Cooldown Boost : " + Mathf.Round(currentCooldownBoost));
            Debug.Log("Current Time Acceleration Boost : " + Mathf.Round(currentTimeAccBoost));
        }
        #endregion
    }

    private void BoostImpulseForce(Rigidbody rb)
    {
        rb.AddRelativeForce(0, 0, boostImpulseForce, ForceMode.Impulse);
        _CharacterFX.ActiveBoost();
    }

    private void UseBoostSpeed()
    {
        _CharacterControler.currentSpeed = speedBoost;
    }

    private void IncreesBoostTimer()
    {
        currentTimeAccBoost = Mathf.Clamp(currentTimeAccBoost + Time.fixedDeltaTime / 2, 0, timeAccBoost);

        _CharacterFX.SurchauffeBoost(currentTimeAccBoost, timeAccBoost);

        if (currentTimeAccBoost == timeAccBoost)
        {
            ResetBoostSpeed();
            _CharacterFX.DesactiveBoostSurchauffe();
            currentCooldownBoost = surchaufeCooldownBoost;
            saveMaxCooldownBoost = surchaufeCooldownBoost;
        }
    }

    private void ResetBoostSpeed()
    {
        _CharacterControler.currentSpeed = _CharacterControler.speedPlayer;

        float a = currentTimeAccBoost / timeAccBoost;
        float b = Mathf.Lerp(minCooldownBoost, maxCooldownBoost, a);
        currentCooldownBoost = b;
        saveMaxCooldownBoost = b;

        currentTimeAccBoost = 0f;

        _CharacterFX.DesactiveBoost();
    }

    private void DecreesBoostCooldown()
    {
        currentCooldownBoost = Mathf.Clamp(currentCooldownBoost - Time.fixedDeltaTime / 2, 0, surchaufeCooldownBoost);

        _CharacterFX.SurchauffeBoostDecres(currentCooldownBoost, saveMaxCooldownBoost);

        if (currentCooldownBoost == 0)
        {
            _CharacterFX.SignBoost();
            _CharacterFX.boostReadyVFX.SendEvent("BoostReady");
        }
    }

    #region OLD (don t erase it)
    /**public void IniatiateBoostValue() //Work with awake
    {
        canFirstImpuls = true;
        boostActiv = true;
    }
    public void TriggerBoost(float inputBoostTrigger, bool isGrounded, Rigidbody rb, float currentSpeedPlayer)
    {
        if (isGrounded && boostActiv)
        {
            if (inputBoostTrigger > 0 && canFirstImpuls)
            {
                FirstImpulsBoost(true, rb);
            }


            if (inputBoostTrigger > 0 && firstImpulsBoostDone)
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
        Debug.Log("Boost Call");
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

                isBoosted = true;
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
        isBoosted = false;

        yield return new WaitForSeconds(currentColdownBoost);
        t2 = 0;
        firstImpulsBoostDone = false;
        canFirstImpuls = true;
        Debug.Log("Cooldown Done");
        boostActiv = true;
    }*/
    #endregion
}
