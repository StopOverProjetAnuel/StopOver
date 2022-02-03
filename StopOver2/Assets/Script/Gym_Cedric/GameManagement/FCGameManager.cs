using UnityEngine;

[RequireComponent(typeof(GMTimer))]
[RequireComponent(typeof(GMVictoryChecker))]
[RequireComponent(typeof(GMMenu))]
public class FCGameManager : MonoBehaviour
{
    private GMTimer _GMTimer;
    private GMVictoryChecker _GMVictoryChecker;
    private GMMenu _GMMenu;



    private void Awake()
    {
        #region Get Scripts
        _GMTimer = GetComponent<GMTimer>();
        _GMVictoryChecker = GetComponent<GMVictoryChecker>();
        _GMMenu = GetComponent<GMMenu>();
        #endregion

        Time.timeScale = 1f;

        _GMTimer.InitiateGMTimer(_GMVictoryChecker);
        _GMVictoryChecker.InitiateGMVictoryChecker(_GMMenu);
        _GMMenu.InitiateGMMenu();
    }

    private void Update()
    {
        _GMTimer.TriggerGMTimer();
        _GMMenu.TriggerGMMenu();
    }
}