using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RessourceManager : MonoBehaviour
{
    public float currentRessource;
    public float maxRessource = 100;
    public float minRessource = 0;
    public float currentResourceScored;
    public GameObject scoreDisplayObj;
    private TextMeshProUGUI scoreDisplayText;

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