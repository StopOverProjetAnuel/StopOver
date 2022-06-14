using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySet : MonoBehaviour
{
    private static float difficultyTimer;
    private static float pointPerTime;

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
}
