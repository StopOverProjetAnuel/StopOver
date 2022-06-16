using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;

public class LeaderboardSaveNLoad : MonoBehaviour
{
    private pause thePause;
    private DifficultySet difficultySet;

    [Header("Display Score")]
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private List<ScoreData> _ScoreData = new List<ScoreData>(3);
    [SerializeField] private LeaderboardComposition[] allDifficultyLeaderboard;
    private int scorePosSaveName;



    public void LeaderboardAwake()
    {
        thePause = FindObjectOfType<pause>();
        difficultySet = FindObjectOfType<DifficultySet>();

        if (_ScoreData.Count != 3) _ScoreData.AddRange(new List<ScoreData>(3));

        #region Create Folder and saves text
        if (!Directory.Exists(Application.persistentDataPath + "/Scores"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Scores");
        }

        foreach (LeaderboardComposition leaderboardComp in allDifficultyLeaderboard)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/Scores/" + leaderboardComp.difficultyName))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Scores/" + leaderboardComp.difficultyName);
            }

            for (int a = 0; a < _ScoreData.Count; a++)
            {
                _ScoreData[a].name = "none";
                _ScoreData[a].scoreValue = 0f;
                if (!File.Exists(Application.persistentDataPath + "/Scores/" + leaderboardComp.difficultyName + "/score_n°" + a.ToString() + "_" + leaderboardComp.difficultyName + ".fc"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Create(Application.persistentDataPath + "/Scores/" + leaderboardComp.difficultyName + "/score_n°" + a.ToString() + "_" + leaderboardComp.difficultyName + ".fc");
                    string json = JsonUtility.ToJson(_ScoreData[a]);
                    bf.Serialize(file, json);
                    file.Close();
                }
            }
        }
        #endregion
    }

    public void DisplayFinalScore(float finalScore)
    {
        LeaderboardComposition leaderboardComp;
        switch (difficultySet.ReturnDifficulty())
        {
            case 0:
                leaderboardComp = allDifficultyLeaderboard[0];
                break;
            case 1:
                leaderboardComp = allDifficultyLeaderboard[1];
                break;
            case 2:
                leaderboardComp = allDifficultyLeaderboard[2];
                break;
            case 3:
                leaderboardComp = allDifficultyLeaderboard[3];
                break;
            default:
                leaderboardComp = allDifficultyLeaderboard[0];
                break;
        }

        #region Load score
        for (int i = 0; i < _ScoreData.Count; i++)
        {
            LoadScoreData(_ScoreData[i], i.ToString(), leaderboardComp);
        }
        #endregion

        #region Display & save score
        for (int i = 0; i < _ScoreData.Count; i++)
        {
            if (finalScore >= _ScoreData[i].scoreValue)
            {
                scorePosSaveName = i;
                SorteScore(scorePosSaveName, finalScore);
                thePause.TriggerNewRecord(finalScore.ToString());
                break;
            }
            else
            {
                thePause.TriggerNoRecord(finalScore.ToString());
            }
        }
        #endregion

        for (int i = 0; i < _ScoreData.Count; i++)
        {
            leaderboardComp.EndMenuLeaderboardNames[i].text = _ScoreData[i].name;
            leaderboardComp.EndMenuLeaderboardScores[i].text = _ScoreData[i].scoreValue.ToString();
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
        LeaderboardComposition leaderboardComp;
        switch (difficultySet.ReturnDifficulty())
        {
            case 0:
                leaderboardComp = allDifficultyLeaderboard[0];
                break;
            case 1:
                leaderboardComp = allDifficultyLeaderboard[1];
                break;
            case 2:
                leaderboardComp = allDifficultyLeaderboard[2];
                break;
            case 3:
                leaderboardComp = allDifficultyLeaderboard[3];
                break;
            default:
                leaderboardComp = allDifficultyLeaderboard[0];
                break;
        }

        _ScoreData[scorePosSaveName].name = playerName.text;

        for (int i = 0; i < _ScoreData.Count; i++)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Scores/" + leaderboardComp.difficultyName + "/score_n°" + i.ToString() + "_" + leaderboardComp.difficultyName + ".fc");
            string json = JsonUtility.ToJson(_ScoreData[i]);
            bf.Serialize(file, json);
            file.Close();
        }

        for (int i = 0; i < _ScoreData.Count; i++)
        {
            leaderboardComp.EndMenuLeaderboardNames[i].text = _ScoreData[i].name;
            leaderboardComp.EndMenuLeaderboardScores[i].text = _ScoreData[i].scoreValue.ToString();
        }
    }

    public void LoadScoreData(ScoreData scoreDataToLoad, string scorePosition, LeaderboardComposition leaderboardComp)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Scores/" + leaderboardComp.difficultyName + "/score_n°" + scorePosition + "_" + leaderboardComp.difficultyName + ".fc"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/Scores/" + leaderboardComp.difficultyName + "/score_n°" + scorePosition + "_" + leaderboardComp.difficultyName + ".fc", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), scoreDataToLoad);
            file.Close();
        }
    }

    public void LoadAllLeaderboard()
    {
        foreach (LeaderboardComposition leaderboardComp in allDifficultyLeaderboard)
        {
            #region Load score
            for (int i = 0; i < _ScoreData.Count; i++)
            {
                LoadScoreData(_ScoreData[i], i.ToString(), leaderboardComp);
            }
            #endregion

            for (int i = 0; i < _ScoreData.Count; i++)
            {
                leaderboardComp.EndMenuLeaderboardNames[i].text = _ScoreData[i].name;
                leaderboardComp.EndMenuLeaderboardScores[i].text = _ScoreData[i].scoreValue.ToString();
            }
        }
    }
}

[System.Serializable]
public class LeaderboardComposition
{
    public string difficultyName;
    public TextMeshProUGUI[] EndMenuLeaderboardNames;
    public TextMeshProUGUI[] EndMenuLeaderboardScores;
}