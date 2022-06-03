using UnityEngine;
using UnityEngine.VFX;
using System.Collections.Generic;

public class C_CharacterFX : MonoBehaviour
{
    #region Scripts Used
    C_CharacterBoost _CharacterBoost;
    RessourceManager ressourceManager;
    SpeedEffectCamera speedEffectCamera;
    #endregion

    [Header("Boost Parameters")]
    public Renderer flameIntShader;
    public Renderer flameExtShader;
    public GameObject boostReadyVFX;
    public VisualEffect distorsionVFX;
    [SerializeField] private VisualEffect SupersonicBoomVFX;
    public GameObject[] overheatingObjects = new GameObject[4];
    #region OverheatingVFX
    private VisualEffect sparkEngineVFX;
    private VisualEffect smokeEngineVFX;
    private VisualEffect overheatingFlameVFX;
    private VisualEffect overheatingDistortionVFX;
    #endregion
    public Renderer reactorMat;

    [Header("Vehicle Parameters")]
    [SerializeField] private VisualEffect smokeShip;
    [SerializeField] private VisualEffect waterShip;
    [SerializeField] private VisualEffect splashWaterShip;
    [SerializeField] private TrailRenderer[] trails;
    public float newTimeTrail = 0f;

    [Header("FuelTank Parameters")]
    [SerializeField] Renderer fluideShader;

    [Header("Camera Effects Parameters")]
    public float maxSpeedCamEffect = 60f;

    [Header("Collision Parameters")]
    [SerializeField] private GameObject hitCollisionVFX;
    [SerializeField] private VisualEffect leftCollisionVFX, rightCollisionVFX;

    [Space(10)]

    public bool showDebug = false;


    public void InitiateFXValue(C_CharacterBoost cB)
    {
        _CharacterBoost = cB;
        ressourceManager = FindObjectOfType<RessourceManager>();
        speedEffectCamera = FindObjectOfType<SpeedEffectCamera>();


        #region BoostVFX
        sparkEngineVFX = overheatingObjects[0].GetComponent<VisualEffect>();
        smokeEngineVFX = overheatingObjects[1].GetComponent<VisualEffect>();
        overheatingFlameVFX = overheatingObjects[2].GetComponent<VisualEffect>();
        overheatingDistortionVFX = overheatingObjects[3].GetComponent<VisualEffect>();
        #endregion
    }

    public void TriggerContinuousFX(bool isGrounded, string groundTag)
    {
        FuelTankAmount();

        TriggerSmokeShip(isGrounded, groundTag);
    }

    public void TriggerEnterCollision()
    {
        TriggerJoystickShake();
    }

    #region Boost
    public void SignBoost()
    {
        flameIntShader.sharedMaterial.SetFloat("_Top_Flame_Gradient__Stop_flame_", 0f);
        flameIntShader.sharedMaterial.SetFloat("_BoostFlame", 0.675f);
        distorsionVFX.SendEvent("BoostOFF");
        boostReadyVFX.SetActive(true);

        sparkEngineVFX.SendEvent("BoostOFF");
        smokeEngineVFX.SendEvent("SmokeEngineOFF");
        overheatingDistortionVFX.SendEvent("OverheatingOFF");
    }

    public void EnableBoost()
    {
        flameIntShader.sharedMaterial.SetFloat("_BoostFlame", 1f);
        flameExtShader.sharedMaterial.SetFloat("_BoostFlame", 1f);
        distorsionVFX.SendEvent("BoostON");
        boostReadyVFX.SetActive(true);
    }

    public void DisableBoost()
    {
        flameIntShader.sharedMaterial.SetFloat("_Top_Flame_Gradient__Stop_flame_", 1f);
        flameExtShader.sharedMaterial.SetFloat("_BoostFlame", 0f);
        distorsionVFX.SendEvent("BoostOFF");
        boostReadyVFX.SetActive(false);
    }

    public void SupersonicBoom()
    {
        SupersonicBoomVFX.SendEvent("SuperSonicStart");
    }

    float timeTrail = 0f;
    float refTrail;
    public void TrailsBoosted()
    {
        timeTrail = Mathf.SmoothDamp(timeTrail, newTimeTrail, ref refTrail, 1f);

        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].time = timeTrail;
        }
    }

    /////////////////////////////

    private float currentColorSize;
    private float currentColorDensity;

    public void OverheatBoost(float currentTimeAcc, float maxTimeAcc)
    {
        float halfTimeAcc = maxTimeAcc / 2;
        currentColorSize = currentTimeAcc / halfTimeAcc;
        reactorMat.sharedMaterial.SetFloat("Color_Size", currentColorSize);

        if (currentTimeAcc >= halfTimeAcc)
        {
            currentColorDensity = currentTimeAcc / maxTimeAcc;
            reactorMat.sharedMaterial.SetFloat("_ColorDensity", currentColorDensity);
        }
    }

    public void DesactiveBoostOverheat()
    {
        distorsionVFX.SendEvent("EngineOFF");

        sparkEngineVFX.SendEvent("BoostON");
        smokeEngineVFX.SendEvent("SmokeEngineON");
        overheatingFlameVFX.SendEvent("OverheatingFlameON");
        overheatingDistortionVFX.SendEvent("OverheatingON");
    }

    public void OverheatBoostDecres(float currentCooldownBoost, float maxCooldownBoost) 
    {
        currentCooldownBoost = Mathf.Clamp(currentCooldownBoost, 0, maxCooldownBoost);
        float cooldownRatio = currentCooldownBoost / maxCooldownBoost;
        float colorSizeTime = Mathf.Lerp(0, 1.5f, cooldownRatio);
        reactorMat.sharedMaterial.SetFloat("Color_Size", colorSizeTime);

        float colorDensityTime = Mathf.Lerp(0, 1f, cooldownRatio);
        reactorMat.sharedMaterial.SetFloat("_ColorDensity", colorDensityTime);
    }
    #endregion

    private void FuelTankAmount()
    {
        if (!fluideShader.sharedMaterial) return;

        float a = ressourceManager.currentRessource / ressourceManager.maxRessource;
        float b = Mathf.Lerp(-0.13f, 0.13f, a);
        fluideShader.sharedMaterial.SetFloat("_Remplissage", b);
    }

    private float c = 0f;
    private float fovTimer = 0f;
    public void FovSpeed(float currentSpeed, bool isBoosted)
    {
        float a = Mathf.Clamp(currentSpeed / 60f, 0f, 1f);
        float b = Mathf.Lerp(0, 0.75f, a);

        if (isBoosted && b == 0.75f)
        {
            b = 1f;
        }

        c = Mathf.Lerp(c, b, fovTimer);
        fovTimer = (fovTimer < 0.1f) ? Mathf.Clamp(fovTimer + Time.deltaTime, 0f, 0.1f) : fovTimer = 0f; // IF(?) fovTimer < 0.1f DO Mathf.Clamp(fovTimer + Time.deltaTime, 0f, 0.1f) ELSE(:) fovTimer = 0f

        speedEffectCamera.speed = c;
        #region Debug
        if (showDebug)
        {
            Debug.Log("Speed percent cam effects" + b * 100);
        }
        #endregion
    }

    private void TriggerSmokeShip(bool isGrounded, string groundTag)
    {
        if (isGrounded)
        {
            if (groundTag == "Water")
            {
                smokeShip.enabled = false;
                waterShip.enabled = true;
                splashWaterShip.enabled = true;
            }
            else
            {
                smokeShip.enabled = true;
                waterShip.enabled = false;
                splashWaterShip.enabled = false;
            }
        }
        else
        {
            smokeShip.enabled = false;
            waterShip.enabled = false;
            splashWaterShip.enabled = false;
        }
    }

    private void TriggerJoystickShake()
    {

    }


    public void TriggerCollisionFX(float dot)
    {
        hitCollisionVFX.SetActive(true);

        if (dot > 0) rightCollisionVFX.Play(); else leftCollisionVFX.Play();
    }
}