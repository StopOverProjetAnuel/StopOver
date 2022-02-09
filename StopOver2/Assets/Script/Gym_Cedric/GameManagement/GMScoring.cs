using UnityEngine;
using TMPro;

public class GMScoring : MonoBehaviour
{
    private GMTimer _GMTimer;
    private RessourceManager _RessourceManager;

    [Header("Parameters")]
    public float pointPerTime = 2f;
    public float pointPerResource = 1f;
    private float finalScore = 0f;

    [Header("Display Score")]
    public TextMeshProUGUI scoreText;
    public string prefixScore = "You earned ";
    public string suffixScore = " points !";

    public void InitiateGMScoring(GMTimer gMTimer)
    {
        _GMTimer = gMTimer;
        _RessourceManager = FindObjectOfType<RessourceManager>();
    }

    public void CalculateFinalScore()
    {
        float timerPoints = _GMTimer.currentTime * pointPerTime;
        float resourcePoints = _RessourceManager.currentRessource * pointPerResource;

        finalScore = timerPoints + resourcePoints;
        finalScore = Mathf.Round(finalScore);

        DisplayFinalScore();
    }

    private void DisplayFinalScore()
    {
        scoreText.text = prefixScore + finalScore + suffixScore;
    }
}
