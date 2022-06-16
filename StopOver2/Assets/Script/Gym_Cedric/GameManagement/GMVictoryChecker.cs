using UnityEngine;

public class GMVictoryChecker : MonoBehaviour
{
    #region Handlers
    private pause menuPause;
    private GMScoring _GMScoring;
    #endregion


    public void InitiateGMVictoryChecker(GMScoring gMScoring)
    {
        menuPause = FindObjectOfType<pause>();
        _GMScoring = gMScoring;
    }

    public void TriggerVictory()
    {
        if (Time.timeScale != 0f) Time.timeScale = 0f;

        _GMScoring.CalculateFinalScore();
    }

    public void TriggerDefeat()
    {
        if (Time.timeScale != 0f) Time.timeScale = 0f;
        
        Time.timeScale = 0f;
        menuPause.TriggerDefeat();
    }
}