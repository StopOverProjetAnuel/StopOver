/**using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GMScoring : MonoBehaviour
{
    private GMTimer _GMTimer;
    private RessourceManager _RessourceManager;

    [Header("Parameters")]
    [SerializeField] private float pointPerTime = 2f;
    [SerializeField] private float pointPerResource = 1f;
    private float finalScore = 0f;

    [Header("Display Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string prefixScore = "You earned ";
    [SerializeField] private string suffixScore = " points !";
    [SerializeField] private int numberScoreStore = 3;
    private LeaderBoardData _LeaderBoardData = new LeaderBoardData();

    public void InitiateGMScoring(GMTimer gMTimer)
    {
        _GMTimer = gMTimer;
        _RessourceManager = FindObjectOfType<RessourceManager>();


        if (!System.IO.File.Exists(Application.persistentDataPath + "/LeaderBoard.json"))
        {
            ScoreData newScore = new ScoreData();

            newScore.name = "none";
            newScore.scoreValue = 0f;

            for (int i = 0; i < numberScoreStore; i++)
            {
                _LeaderBoardData.scoreData.Add(newScore);
            }

            SaveScoreData();
        }
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

        string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/LeaderBoard.json");
        JsonUtility.FromJson(jsonString, typeof(List<ScoreData>));
        Debug.Log("json data : " + jsonString);
        Debug.Log("score data : " + _LeaderBoardData.scoreData.ToString());

        ScoreData[] oldScoreData = _LeaderBoardData.scoreData.ToArray();
        /**Debug.Log("Old score data : - " + oldScoreData[0].name);
        Debug.Log("                 - " + oldScoreData[0].scoreValue);
        Debug.Log("                 - " + oldScoreData[1].scoreValue);
        Debug.Log("                 - " + oldScoreData[1].scoreValue);
        Debug.Log("                 - " + oldScoreData[2].scoreValue);
        Debug.Log("                 - " + oldScoreData[2].scoreValue);*/

/**
        for (int i = 0; i < numberScoreStore; i++)
        {
            if (finalScore >= oldScoreData[i].scoreValue)
            {
                oldScoreData[i].name = "none";
                oldScoreData[i].scoreValue = finalScore;
                break;
            }
        }

        ScoreData newScore = new ScoreData();
        for (int i = 0; i < numberScoreStore; i++)
        {
            newScore.name = oldScoreData[i].name;
            newScore.scoreValue = oldScoreData[i].scoreValue;
            _LeaderBoardData.scoreData.Add(newScore);
        }

        SaveScoreData();
    }



    public void SaveScoreData()
    {
        string leaderBoard = JsonUtility.ToJson(_LeaderBoardData);
        System.IO.File.Delete(Application.persistentDataPath + "/LeaderBoard.json");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/LeaderBoard.json", leaderBoard);
    }
}

[System.Serializable]
public class LeaderBoardData
{
    public List<ScoreData> scoreData = new List<ScoreData>();
}

[System.Serializable]
public class ScoreData
{
    public string name;
    public float scoreValue;
}
*/