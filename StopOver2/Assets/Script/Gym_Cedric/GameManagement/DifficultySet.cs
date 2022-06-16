using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySet : MonoBehaviour
{
    private static float difficultyTimer;
    private static float pointPerTime;
    [Tooltip("The difficulty mode is reference with an int : 0 = easy | 1 = normal | 2 = hard | 3 = impossible")]
    private static int difficultyPos;

    public void SetDifficultyTimer(float time)
    {
        difficultyTimer = time;
    }

    public float ReturnTimer()
    {
        return difficultyTimer;
    }

    public void SetPointPerTime(float point)
    {
        pointPerTime = point;
    }

    public float ReturnPointPerTimer()
    {
        return pointPerTime;
    }

    /// <summary>
    /// Set the party difficulty with an int
    /// </summary>
    /// <param name="difficulty"> 0 = easy | 1 = normal | 2 = hard | 3 = impossible </param>
    public void SetDifficulty(int difficulty)
    {
        difficultyPos = difficulty;
    }

    public int ReturnDifficulty()
    {
        return difficultyPos;
    }
}
