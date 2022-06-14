using UnityEngine;

[RequireComponent(typeof(GMTimer))]
[RequireComponent(typeof(GMVictoryChecker))]
[RequireComponent(typeof(GMMenu))]
[RequireComponent(typeof(GMScoring))]
public class FCGameManager : MonoBehaviour
{
    private GMTimer _GMTimer;
    private GMVictoryChecker _GMVictoryChecker;
    private GMMenu _GMMenu;
    private GMScoring _GMScoring;


    private void Start()
    {
        #region Get Scripts
        _GMTimer = GetComponent<GMTimer>();
        _GMVictoryChecker = GetComponent<GMVictoryChecker>();
        _GMMenu = GetComponent<GMMenu>();
        _GMScoring = GetComponent<GMScoring>();
        #endregion

        Time.timeScale = 1f;

        _GMTimer.InitiateGMTimer(_GMVictoryChecker);
        _GMVictoryChecker.InitiateGMVictoryChecker(_GMScoring);
        _GMMenu.InitiateGMMenu();
        _GMScoring.InitiateGMScoring(_GMTimer);
    }

    private void Update()
    {
        _GMTimer.TriggerGMTimer();
        _GMMenu.TriggerGMMenu();
    }
}