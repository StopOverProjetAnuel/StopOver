using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LeaderboardSaveNLoad))]
public class GMScoring : MonoBehaviour
{
    private LeaderboardSaveNLoad leaderboardSaveNLoad;
    private GMTimer _GMTimer;
    private RessourceManager _RessourceManager;
    private GMMenu _GMMenu;

    [Header("Parameters")]
    [SerializeField] private float pointPerTime = 2f;
    [SerializeField] private float pointPerResource = 1f;
    private float finalScore = 0f;

    [Header("Display Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private string prefixScore = "You earned ";
    [SerializeField] private string suffixScore = " points !";
    [SerializeField] private List<ScoreData> _ScoreData = new List<ScoreData>(3);
    [SerializeField] private TextMeshProUGUI[] EndMenuLeaderboardNames;
    [SerializeField] private TextMeshProUGUI[] EndMenuLeaderboardScores;
    private int scorePosSaveName;

    public void InitiateGMScoring(GMTimer gMTimer, GMMenu gMMenu)
    {
        _GMTimer = gMTimer;
        _RessourceManager = FindObjectOfType<RessourceManager>();
        leaderboardSaveNLoad = GetComponent<LeaderboardSaveNLoad>();
        _GMMenu = gMMenu;


        leaderboardSaveNLoad.LeaderboardAwake();
    }

    public void CalculateFinalScore()
    {
        float timerPoints = _GMTimer.currentTime * pointPerTime;
        float resourcePoints = _RessourceManager.currentRessource * pointPerResource;

        finalScore = timerPoints + resourcePoints;
        finalScore = Mathf.Round(finalScore);
        Debug.Log("score : " + finalScore);
        leaderboardSaveNLoad.DisplayFinalScore(finalScore);
    }
}