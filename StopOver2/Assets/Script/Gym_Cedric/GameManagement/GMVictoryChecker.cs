using UnityEngine;

public class GMVictoryChecker : MonoBehaviour
{
    private GMMenu _GMMenu;
    private GMScoring _GMScoring;


    public void InitiateGMVictoryChecker(GMMenu gMMenu, GMScoring gMScoring)
    {
        _GMMenu = gMMenu;
        _GMScoring = gMScoring;
    }

    public void TriggerVictory()
    {
        if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
            _GMMenu.OpenEndMenu(true);
            _GMScoring.CalculateFinalScore();
        }
    }

    public void TriggerDefeat()
    {
        if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
            _GMMenu.OpenEndMenu(false);
        }
    }
}