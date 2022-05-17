using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    [Header("Requirement")]
    [SerializeField] private GameObject fluidScored;
    private Renderer fluidMat;

    [Header("RealTime Resources")]
    public float currentRessource;
    [SerializeField] private float resourceGoal = 25000f;
    [SerializeField] private float currentResourceScored;

    [Header("Parameters")]
    public float maxRessource = 100;
    [SerializeField] private float minRessource = 0;

    [Header("Debug Parameters")]
    [SerializeField] private bool showDebug = false;


    private void Awake()
    {
        currentResourceScored = 0f;
        currentRessource = 0f;
        fluidMat = (fluidScored.TryGetComponent<Renderer>(out Renderer render)) ? render : null;

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
        if(fluidMat) fluidMat.sharedMaterial.SetFloat("_Remplissage", a);

        #region Debug
        if (showDebug)
        {
            Debug.Log("Current Score : " + currentResourceScored);
        }
        #endregion
    }
}