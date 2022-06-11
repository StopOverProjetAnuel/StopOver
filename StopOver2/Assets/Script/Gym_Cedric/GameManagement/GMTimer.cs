using UnityEngine;
using TMPro;

public class GMTimer : MonoBehaviour
{
    [Header("Require")]
    [SerializeField] private TextMeshProUGUI timerText;
    private FMOD_SoundCaller soundCaller;
    private GMVictoryChecker _GMVictoryChecker;
    private TimerDifficultySet timerDifficultySet;

    [Header("Timer parameters")]
    public float maxTime = 240f;
    public float currentTime = 0f;

    public bool playTimer = false;



    public void InitiateGMTimer(GMVictoryChecker gMVictoryChecker)
    {
        timerDifficultySet = GetComponent<TimerDifficultySet>();
        maxTime = timerDifficultySet.ReturnTimer();
        _GMVictoryChecker = gMVictoryChecker;
        soundCaller = TryGetComponent<FMOD_SoundCaller>(out FMOD_SoundCaller sound) ? sound : null;

        currentTime = maxTime;
        DisplayTimer();
    }

    public void TriggerGMTimer()
    {
        if (playTimer && currentTime != 0f)
        {
            DecreaseTimer();
            DisplayTimer();
        }
        else if (currentTime == 0f)
        {
            _GMVictoryChecker.TriggerDefeat();
        }

        TriggerLastMinute();
    }

    private bool getTrigger = false;
    private void TriggerLastMinute()
    {
        if (!getTrigger && currentTime <= 60f)
        {
            getTrigger = true;
            soundCaller.SoundStart();
        }
        else if (getTrigger && currentTime > 60f)
        {
            getTrigger = false;
        }
    }

    private void DecreaseTimer()
    {
        currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0, maxTime * 2);
    }

    private void DisplayTimer()
    {
        float minValue = currentTime / 60f;
        float minValueDisplay = Mathf.Floor(minValue);
        float secValue = (minValue - minValueDisplay) * 100;
        float secValueDisplay = Mathf.Round(secValue * 59 / 100);

        string a = null;
        if (minValueDisplay < 10f)
        {
            a = "0";
        }
        string b = null;
        if (secValueDisplay < 10f)
        {
            b = "0";
        }

        timerText.text = a + minValueDisplay + ":" + b + secValueDisplay;
    }
}
