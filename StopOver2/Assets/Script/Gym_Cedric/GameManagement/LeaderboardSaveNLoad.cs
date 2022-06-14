using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;

public class LeaderboardSaveNLoad : MonoBehaviour
{
    [Header("Display Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private string prefixScore = "You earned ";
    [SerializeField] private string suffixScore = " points !";
    [SerializeField] private List<ScoreData> _ScoreData = new List<ScoreData>(3);
    [SerializeField] private TextMeshProUGUI[] EndMenuLeaderboardNames;
    [SerializeField] private TextMeshProUGUI[] EndMenuLeaderboardScores;
    private int scorePosSaveName;



    public void LeaderboardAwake()
    {
        if (_ScoreData.Count != 3) _ScoreData.AddRange(new List<ScoreData>(3));

        #region Create Folder and saves text
        if (!Directory.Exists(Application.persistentDataPath + "/score_saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/score_saves");
        }

        for (int i = 0; i < _ScoreData.Count; i++)
        {
            _ScoreData[i].name = "none";
            _ScoreData[i].scoreValue = 0f;
            if (!File.Exists(Application.persistentDataPath + "/score_saves/score_n°" + i.ToString() + ".fc"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/score_saves/score_n°" + i.ToString() + ".fc");
                string json = JsonUtility.ToJson(_ScoreData[i]);
                bf.Serialize(file, json);
                file.Close();
            }
        }
        #endregion
    }

    public void DisplayFinalScore(float finalScore)
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
                scorePosSaveName = i;
                SorteScore(scorePosSaveName, finalScore);
                break;
            }
            else
            {
                scoreText.text = prefixScore + finalScore + suffixScore;
            }
        }
        #endregion

        for (int i = 0; i < _ScoreData.Count; i++)
        {
            EndMenuLeaderboardNames[i].text = _ScoreData[i].name;
            EndMenuLeaderboardScores[i].text = _ScoreData[i].scoreValue.ToString();
        }
    }

    private void SorteScore(int scoringPos, float finalScore)
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

    public void SaveScoreData()
    {
        _ScoreData[scorePosSaveName].name = playerName.text;

        for (int i = 0; i < _ScoreData.Count; i++)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/score_saves/score_n°" + i.ToString() + ".fc");
            string json = JsonUtility.ToJson(_ScoreData[i]);
            bf.Serialize(file, json);
            file.Close();
        }

        for (int i = 0; i < _ScoreData.Count; i++)
        {
            EndMenuLeaderboardNames[i].text = _ScoreData[i].name;
            EndMenuLeaderboardScores[i].text = _ScoreData[i].scoreValue.ToString();
        }
    }

    public void LoadScoreData(ScoreData scoreDataToLoad, string scorePosition)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/score_saves/score_n°" + scorePosition + ".fc"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/score_saves/score_n°" + scorePosition + ".fc", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), scoreDataToLoad);
            file.Close();
        }
    }
}
