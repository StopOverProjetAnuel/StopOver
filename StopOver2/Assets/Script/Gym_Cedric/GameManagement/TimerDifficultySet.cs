using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDifficultySet : MonoBehaviour
{
    private static float difficultyTimer;

    public void SetDifficultyTimer(float time)
    {
        difficultyTimer = time;
    }

    public float ReturnTimer()
    {
        return difficultyTimer;
    }
}
