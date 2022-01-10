using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class RessourceManager : MonoBehaviour
{
    [Header("Objects Need")]
    public GameObject fluidScored;
    private Renderer fluidMat;
    [Header("RealTime Resources")]
    public float currentRessource;
    public float resourceGoal = 25000f;
    public float currentResourceScored;
    [Header("Parameters")]
    public float maxRessource = 100;
    public float minRessource = 0;

    public bool showDebug = false;


    private void Awake()
    {
        currentResourceScored = 0f;
        currentRessource = 0f;
        fluidMat = fluidScored.GetComponent<Renderer>();

        TriggerScore(0);
    }

    public void TriggerRessourceCount(float varR)
    {
        currentRessource = Mathf.Clamp(currentRessource + varR, minRessource, maxRessource);
    }

    public void TriggerScore(float currentRTaken)
    {
        currentResourceScored += currentRTaken;
        currentRessource -= currentRTaken;
        float a = Mathf.Clamp(currentResourceScored / resourceGoal, 0, 1);
        fluidMat.sharedMaterial.SetFloat("_Remplissage", a);

        #region Debug
        if (showDebug)
        {
            Debug.Log("Current Score : " + currentResourceScored);
        }
        #endregion
    }
}