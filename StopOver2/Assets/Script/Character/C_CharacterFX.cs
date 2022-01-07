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
    public Material matBoost;
    public Material matSurchauffe;
    public VisualEffect flammeSurchauffe;
    private float currentTimeSurchauffe;
    private float t1;

    [Header("FuelTank Parameters")]
    public GameObject fuelTankFluide;
    public Renderer fluideShader;

    [Header("Camera Effects Parameters")]
    public float maxSpeedCamEffect = 60f;

    [Header("Player Animation Parameters")]
    public GameObject characterModel;
    public float leanAngleZ = 25f;
    public float leanAngleY = 15f;

    [Space(10)]

    public bool showDebug = false;


    public void InitiateFXValue(C_CharacterBoost cB)
    {
        _CharacterBoost = cB;
        ressourceManager = FindObjectOfType<RessourceManager>();
        fluideShader = fuelTankFluide.GetComponent<Renderer>();
        speedEffectCamera = FindObjectOfType<SpeedEffectCamera>();
    }

    public void TriggerContinuousFX(float currentSpeed, float mouseX)
    {
        FuelTankAmount();
        FovSpeed(currentSpeed);
        LeaningModel(mouseX);
    }

    #region Boost
    public void ActiveBoost()
    {
        /**matBoost.SetFloat("_Top_Flame_Gradient__Stop_flame_", 0);
       
        matBoost.SetFloat("_BoostFlame", 1);*/
    }

    public void DesactiveBoost()
    {
        //matBoost.SetFloat("_BoostFlame", 0.63f);
    }



    public void SurchauffeBoost()
    {
        /**currentTimeSurchauffe = Mathf.Lerp(0, 1, _CharacterBoost.t2);
        matSurchauffe.SetFloat("Color_Size", currentTimeSurchauffe);
        matSurchauffe.SetFloat("_ColorDensity", currentTimeSurchauffe);
        matBoost.SetFloat("_Flame_ColorGradient", currentTimeSurchauffe);*/
    }

    public void DesactiveBosstSurchauffe()
    {
        /**flammeSurchauffe.Play();
        matBoost.SetFloat("_BoostFlame", 0.63f);
        matBoost.SetFloat("_Top_Flame_Gradient__Stop_flame_", 1);*/
    }

    public void SurchauffeBoostDecres(bool isNotActif) 
    {
        /**if(t1 >= 1)
        {
            t1 = 0;
        }
        else
        {
            t1 -= Time.fixedDeltaTime / _CharacterBoost.currentColdownBoost;
        }
        currentTimeSurchauffe = Mathf.Lerp(1, 0, t1);
        matSurchauffe.SetFloat("Color_Size", currentTimeSurchauffe);
        matSurchauffe.SetFloat("_ColorDensity", currentTimeSurchauffe);
        matBoost.SetFloat("_Flame_ColorGradient", currentTimeSurchauffe);

        Debug.Log(t1);*/
    }
    #endregion

    private void FuelTankAmount()
    {
        float a = ressourceManager.currentRessource / ressourceManager.maxRessource;
        float b = Mathf.Lerp(-0.1f, 0.1f, a);
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

    private void LeaningModel(float mouseX)
    {
        float leaningAngleZ = -mouseX * leanAngleZ;
        float leaningAngleY = mouseX * leanAngleY;

        Vector3 leaningAngle = new Vector3(0, leaningAngleY, leaningAngleZ);

        characterModel.transform.localRotation = Quaternion.Euler(leaningAngle);
    }
}