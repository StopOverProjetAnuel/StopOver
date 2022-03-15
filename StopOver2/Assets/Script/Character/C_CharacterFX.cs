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
    public VisualEffect boostReadyVFX;
    public VisualEffect distorsionVFX;
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
    [SerializeField] private TrailRenderer[] trails = new TrailRenderer[4];
    [SerializeField] private float trailSpeedThreshold = 45f;

    [Header("FuelTank Parameters")]
    [SerializeField] Renderer fluideShader;

    [Header("Camera Effects Parameters")]
    public float maxSpeedCamEffect = 60f;

    [Header("Player Animation Parameters")]
    public GameObject characterModel;
    public float leanAngleZ = 25f;
    public float leanAngleY = 15f;
    public float leaningSpeed = 0.1f;

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

    public void TriggerContinuousFX(float mouseX, bool isGrounded)
    {
        FuelTankAmount();
        LeaningModel(mouseX);
        TriggerSmokeShip(isGrounded);
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

        sparkEngineVFX.SendEvent("BoostOFF");
        smokeEngineVFX.SendEvent("SmokeEngineOFF");
        overheatingDistortionVFX.SendEvent("OverheatingOFF");
    }

    public void ActiveBoost()
    {
        flameIntShader.sharedMaterial.SetFloat("_BoostFlame", 1f);
        flameExtShader.sharedMaterial.SetFloat("_BoostFlame", 1f);
        distorsionVFX.SendEvent("BoostON");
    }

    public void DesactiveBoost()
    {
        flameIntShader.sharedMaterial.SetFloat("_Top_Flame_Gradient__Stop_flame_", 1f);
        flameExtShader.sharedMaterial.SetFloat("_BoostFlame", 0f);
        distorsionVFX.SendEvent("BoostOFF");
    }

    /////////////////////////////

    private float currentColorSize;
    private float currentColorDensity;

    public void OverheatBoost(float currentTimeAcc, float maxTimeAcc)
    {
        float halfTimeAcc = maxTimeAcc / 2;
        currentColorSize = currentTimeAcc / halfTimeAcc;
        reactorMat.sharedMaterial.SetFloat("Color_Size", currentColorSize);

        float lateHalftTimeAcc = maxTimeAcc - halfTimeAcc;
        if (currentTimeAcc >= halfTimeAcc)
        {
            currentColorDensity = currentTimeAcc / lateHalftTimeAcc;
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
        float a = currentCooldownBoost / maxCooldownBoost;
        float b = Mathf.Lerp(0, currentColorSize, a);
        reactorMat.sharedMaterial.SetFloat("Color_Size", b);

        float c = Mathf.Lerp(0, currentColorDensity, a);
        reactorMat.sharedMaterial.SetFloat("_ColorDensity", c);
    }
    #endregion

    private void FuelTankAmount()
    {
        if (!fluideShader.sharedMaterial) return;

        float a = ressourceManager.currentRessource / ressourceManager.maxRessource;
        float b = Mathf.Lerp(-0.16f, 0.16f, a);
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

    float smoothMouseMovement = 0f;

    private void LeaningModel(float mouseX)
    {
        smoothMouseMovement = Mathf.SmoothStep(smoothMouseMovement, mouseX, leaningSpeed);

        float leaningAngleY = smoothMouseMovement * leanAngleY * Time.timeScale;
        float leaningAngleZ = -smoothMouseMovement * leanAngleZ * Time.timeScale;

        Vector3 leaningAngle = new Vector3(0, leaningAngleY, leaningAngleZ);

        characterModel.transform.localRotation = Quaternion.Euler(leaningAngle);
    }

    private bool isSmokePlay = false;

    private void TriggerSmokeShip(bool isGrounded)
    {
        if (isGrounded && !isSmokePlay)
        {
            smokeShip.SendEvent("SmokeOn");
            isSmokePlay = true;
        }
        else
        {
            smokeShip.SendEvent("SmokeOff");
            isSmokePlay = false;
        }
    }

    private void TriggerJoystickShake()
    {

    }

    public void HandleTrailPlayer(float currentSpeed)
    {
        Debug.LogWarning("current Speed : " + currentSpeed);
        if (trailSpeedThreshold > currentSpeed)
        {
            for (int i = 0; i < trails.Length; i++)
            {
                if(trails[i].emitting)
                {
                    trails[i].emitting = false;
                    Debug.LogWarning("Stopped Emitting");

                }
            }
        }
        else
        {
            for (int i = 0; i < trails.Length; i++)
            {
                if (!trails[i].emitting)
                {
                    Debug.LogWarning("Started Emitting");
                    trails[i].emitting = true;
                }
            }
        }
    }
}