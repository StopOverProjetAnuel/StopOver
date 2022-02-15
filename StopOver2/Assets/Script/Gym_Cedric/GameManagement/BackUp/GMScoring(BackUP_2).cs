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
    private ScoreData _ScoreData = new ScoreData();

    public void InitiateGMScoring(GMTimer gMTimer)
    {
        _GMTimer = gMTimer;
        _RessourceManager = FindObjectOfType<RessourceManager>();


        if (!System.IO.File.Exists(Application.persistentDataPath + "/LeaderBoard.json"))
        {
            ScoreData iniScoreData = new ScoreData();

            iniScoreData.name = "none";
            iniScoreData.scoreValue = 0f;

            for (int i = 0; i < numberScoreStore; i++)
            {
                SaveScoreData(iniScoreData, i.ToString());
            }

            Debug.Log("Creating saves files");
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
        #region Display score
        for (int i = 0; i < numberScoreStore; i++)
        {
            if (finalScore >= _ScoreData.scoreValue)
            {
                scoreText.text = "New Record ! " + finalScore + " Points !!!";
                break;
            }
            else
            {
                scoreText.text = prefixScore + finalScore + suffixScore;
            }
        }
        #endregion

        #region Place the new score
        ScoreData[] scoreDataArray = new ScoreData[numberScoreStore];

        for (int i = 0; i < numberScoreStore; i++)
        {

            string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/scoreData" + i.ToString() + ".json");
            _ScoreData.ReadJson(jsonString);
            Debug.Log("json data : " + jsonString);

            Debug.Log("Score Data n°" + i.ToString() + " : " + _ScoreData.name);
            Debug.Log("Score Data n°" + i.ToString() + " : " + _ScoreData.scoreValue);

            if (finalScore >= _ScoreData.scoreValue)
            {
                scoreDataArray[i].name = "none";
                scoreDataArray[i].scoreValue = finalScore;
            }
            else
            {
                scoreDataArray[i] = _ScoreData;
            }

            SaveScoreData(scoreDataArray[i], i.ToString());
        }
        #endregion
    }



    public void SaveScoreData(ScoreData scoreDataToSave, string scorePosition)
    {
        string scoreDataJson = JsonUtility.ToJson(scoreDataToSave);
        System.IO.File.Delete(Application.persistentDataPath + "/scoreData" + scorePosition + ".json");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/scoreData" + scorePosition + ".json", scoreDataJson);
    }
}


[System.Serializable]
public class ScoreData
{
    public string name;
    public float scoreValue;

    public void ReadJson(string jsonString)
    {
        name = JsonUtility.FromJson<ScoreData>(jsonString).name;
        scoreValue = JsonUtility.FromJson<ScoreData>(jsonString).scoreValue;
        Debug.Log(name);
        Debug.Log(scoreValue);
    }

    public void ResetValues()
    {
        name = null;
        scoreValue = 0f;
    }
}*/