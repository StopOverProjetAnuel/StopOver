using UnityEngine;
using UnityEngine.VFX;

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
    public VisualEffect smokeShip;

    [Header("FuelTank Parameters")]
    public GameObject fuelTankFluide;
    private Renderer fluideShader;

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
        fluideShader = fuelTankFluide.GetComponent<Renderer>();
        speedEffectCamera = FindObjectOfType<SpeedEffectCamera>();


        #region BoostVFX
        sparkEngineVFX = overheatingObjects[0].GetComponent<VisualEffect>();
        smokeEngineVFX = overheatingObjects[1].GetComponent<VisualEffect>();
        overheatingFlameVFX = overheatingObjects[2].GetComponent<VisualEffect>();
        overheatingDistortionVFX = overheatingObjects[3].GetComponent<VisualEffect>();
        #endregion
    }

    public void TriggerContinuousFX(float currentSpeed, float mouseX, bool isGrounded)
    {
        FuelTankAmount();
        FovSpeed(currentSpeed);
        LeaningModel(mouseX);
        TriggerSmokeShip(isGrounded);
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

    public void SurchauffeBoost(float currentTimeAcc, float maxTimeAcc)
    {
        float a = maxTimeAcc / 2;
        currentColorSize = currentTimeAcc / a;
        reactorMat.sharedMaterial.SetFloat("Color_Size", currentColorSize);

        float c = maxTimeAcc - a;
        if (currentTimeAcc >= a)
        {
            currentColorDensity = currentTimeAcc / c;
            reactorMat.sharedMaterial.SetFloat("_ColorDensity", currentColorDensity);
        }
    }

    public void DesactiveBoostSurchauffe()
    {
        distorsionVFX.SendEvent("EngineOFF");

        sparkEngineVFX.SendEvent("BoostON");
        smokeEngineVFX.SendEvent("SmokeEngineON");
        overheatingFlameVFX.SendEvent("OverheatingFlameON");
        overheatingDistortionVFX.SendEvent("OverheatingON");
    }

    public void SurchauffeBoostDecres(float currentCooldownBoost, float maxCooldownBoost) 
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
        float a = ressourceManager.currentRessource / ressourceManager.maxRessource;
        float b = Mathf.Lerp(-0.16f, 0.16f, a);
        fluideShader.sharedMaterial.SetFloat("_Remplissage", b);
    }

    private void FovSpeed(float currentSpeed)
    {
        float a = Mathf.Clamp(currentSpeed / maxSpeedCamEffect, 0, 1);
        speedEffectCamera.speed = a;

        #region Debug
        if (showDebug)
        {
            Debug.Log("Speed percent cam effects" + a);
        }
        #endregion
    }

    float smoothMouseMovement = 0f;

    private void LeaningModel(float mouseX)
    {
        smoothMouseMovement = Mathf.SmoothStep(smoothMouseMovement, mouseX, leaningSpeed);

        float leaningAngleY = smoothMouseMovement * leanAngleY;
        float leaningAngleZ = -smoothMouseMovement * leanAngleZ;

        Vector3 leaningAngle = new Vector3(0, leaningAngleY, leaningAngleZ);

        characterModel.transform.localRotation = Quaternion.Euler(leaningAngle);
    }

    private bool isSmokePlay = false;

    public void TriggerSmokeShip(bool isGrounded)
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
}