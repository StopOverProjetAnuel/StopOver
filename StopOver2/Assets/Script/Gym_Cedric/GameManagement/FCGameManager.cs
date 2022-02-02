using UnityEngine;
using TMPro;

public class FCGameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    [Header("Timer parameters")]
    public float maxTime = 4f;
    [SerializeField]
    private float currentTime = 0f;

    public bool playTimer = false;



    private void Awake()
    {
        currentTime = maxTime;
    }

    private void Update()
    {
        if (playTimer && currentTime != 0f)
        {
            DecreaseTimer();
            DisplayTimer();
        }
    }

    private void DecreaseTimer()
    {
        currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0, maxTime);
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