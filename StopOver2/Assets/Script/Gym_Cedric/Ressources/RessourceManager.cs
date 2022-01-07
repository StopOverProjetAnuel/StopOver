using UnityEngine;
using TMPro;

public class RessourceManager : MonoBehaviour
{
    [Header("Objects Need")]
    public GameObject scoreDisplayObj;
    private TextMeshProUGUI scoreDisplayText;
    [Header("RealTime Resources")]
    public float currentRessource;
    public float currentResourceScored;
    [Header("Parameters")]
    public float maxRessource = 100;
    public float minRessource = 0;

    private void Awake()
    {
        currentResourceScored = 0f;
        currentRessource = 0f;
        scoreDisplayText = scoreDisplayObj.GetComponent<TextMeshProUGUI>();
    }

    public void TriggerRessourceCount(float varR)
    {
        currentRessource = Mathf.Clamp(currentRessource + varR, minRessource, maxRessource);
    }

    public void TriggerScore(float currentRTaken)
    {
        currentResourceScored += currentRTaken;
        currentRessource -= currentRTaken;
        scoreDisplayText.SetText("Score : " + currentResourceScored);
        //Debug.Log("Current Score : " + currentResourceScored);
    }
}