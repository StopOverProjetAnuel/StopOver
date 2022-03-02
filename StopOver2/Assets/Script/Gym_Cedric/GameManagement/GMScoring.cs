using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    [SerializeField] private List<ScoreData> _ScoreData = new List<ScoreData>(3);

    public void InitiateGMScoring(GMTimer gMTimer)
    {
        _GMTimer = gMTimer;
        _RessourceManager = FindObjectOfType<RessourceManager>();


        #region Create Folder and saves text
        if (!Directory.Exists(Application.persistentDataPath + "/score_saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/score_saves");
        }

        for (int i = 0; i < _ScoreData.Count; i++)
        {
            _ScoreData[i].name = "none";
            _ScoreData[i].scoreValue = 0f;
            if (!File.Exists(Application.persistentDataPath + "/score_saves/score_n�" + i.ToString() + ".fc"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/score_saves/score_n�" + i.ToString() + ".fc");
                string json = JsonUtility.ToJson(_ScoreData[i]);
                bf.Serialize(file, json);
                file.Close();
            }
        }
        #endregion
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
        #region Load score
        for (int i = 0; i < _ScoreData.Count; i++)
        {
            LoadScoreData(_ScoreData[i], i.ToString());
        }
        #endregion

        #region Display & save score
        for (int i = 0; i < _ScoreData.Count; i++)
        {
            if (finalScore >= _ScoreData[i].scoreValue)
            {
                scoreText.text = "New Record ! " + finalScore + " Points !!!";
                SorteScore(i);
                break;
            }
            else
            {
                scoreText.text = prefixScore + finalScore + suffixScore;
            }
        }
        #endregion

        #region Save Data
        for (int i = 0; i < _ScoreData.Count; i++)
        {
            SaveScoreData(_ScoreData[i], i.ToString());
        }
        #endregion
    }

    private void SorteScore(int scoringPos)
    {
        for (int i = 0; i < _ScoreData.Count; i++)
        {
            if (finalScore >= _ScoreData[i].scoreValue)
            {
                ScoreData newScore = new ScoreData()
                {
                    name = "none",
                    scoreValue = finalScore
                };

                _ScoreData.Insert(i, newScore);
                break;
            }
        }

        _ScoreData.RemoveAt(3);
    }



    private void SaveScoreData(ScoreData scoreDataToSave, string scorePosition)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/score_saves/score_n�" + scorePosition + ".fc");
        string json = JsonUtility.ToJson(scoreDataToSave);
        bf.Serialize(file, json);
        file.Close();
    }

    private void LoadScoreData(ScoreData scoreDataToLoad, string scorePosition)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/score_saves/score_n�" + scorePosition + ".fc"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/score_saves/score_n�" + scorePosition + ".fc", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), scoreDataToLoad);
            file.Close();
        }
    }
}


[System.Serializable]
public class ScoreData
{
    public string name;
    public float scoreValue;
}