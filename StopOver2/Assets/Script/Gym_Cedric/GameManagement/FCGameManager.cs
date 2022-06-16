using UnityEngine;

[RequireComponent(typeof(GMTimer))]
[RequireComponent(typeof(GMVictoryChecker))]
[RequireComponent(typeof(GMScoring))]
public class FCGameManager : MonoBehaviour
{
    private GMTimer _GMTimer;
    private GMVictoryChecker _GMVictoryChecker;
    private GMScoring _GMScoring;


    private void Start()
    {
        #region Get Scripts
        _GMTimer = GetComponent<GMTimer>();
        _GMVictoryChecker = GetComponent<GMVictoryChecker>();
        _GMScoring = GetComponent<GMScoring>();
        #endregion

        Time.timeScale = 1f;

        _GMTimer.InitiateGMTimer(_GMVictoryChecker);
        _GMVictoryChecker.InitiateGMVictoryChecker(_GMScoring);
        _GMScoring.InitiateGMScoring(_GMTimer);
    }

    private void Update()
    {
        _GMTimer.TriggerGMTimer();
    }
}