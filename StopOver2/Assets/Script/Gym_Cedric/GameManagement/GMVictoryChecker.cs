using UnityEngine;

public class GMVictoryChecker : MonoBehaviour
{
    private GMMenu _GMMenu;


    public void InitiateGMVictoryChecker(GMMenu gMMenu)
    {
        _GMMenu = gMMenu;
    }

    public void TriggerVictory()
    {
        if (Time.timeScale != 0f)
        {
            _GMMenu.OpenEndMenu(true);
            Time.timeScale = 0f;
        }
    }

    public void TriggerDefeat()
    {
        if (Time.timeScale != 0f)
        {
            _GMMenu.OpenEndMenu(false);
            Time.timeScale = 0f;
        }
    }
}