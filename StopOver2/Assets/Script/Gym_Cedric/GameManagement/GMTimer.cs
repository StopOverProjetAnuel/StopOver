using UnityEngine;
using TMPro;

public class GMTimer : MonoBehaviour
{
    private GMVictoryChecker _GMVictoryChecker;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [Header("Timer parameters")]
    public float maxTime = 240f;
    public float currentTime = 0f;

    public bool playTimer = false;



    public void InitiateGMTimer(GMVictoryChecker gMVictoryChecker)
    {
        _GMVictoryChecker = gMVictoryChecker;

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
