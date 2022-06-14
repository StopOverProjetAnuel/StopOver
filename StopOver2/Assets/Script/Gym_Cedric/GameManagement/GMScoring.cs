using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LeaderboardSaveNLoad))]
public class GMScoring : MonoBehaviour
{
    private DifficultySet point;
    private LeaderboardSaveNLoad leaderboardSaveNLoad;
    private GMTimer _GMTimer;
    private RessourceManager _RessourceManager;

    [Header("Parameters")]
    [SerializeField] private float pointPerTime = 2f;
    [SerializeField] private float pointPerResource = 1f;
    private float finalScore = 0f;



    public void InitiateGMScoring(GMTimer gMTimer)
    {
        _GMTimer = gMTimer;
        _RessourceManager = FindObjectOfType<RessourceManager>();
        leaderboardSaveNLoad = GetComponent<LeaderboardSaveNLoad>();
        leaderboardSaveNLoad.LeaderboardAwake();
        
        point = GetComponent<DifficultySet>();
        pointPerTime = point.ReturnPointPerTimer();
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