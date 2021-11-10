using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public GameObject blackFlashScreen;
    [HideInInspector] public float timeFlash;
    [HideInInspector] public bool isActive = true;
    [HideInInspector] public bool isActiveTriggered = false;


    public void TriggerFlashSpeed(bool isTriggered)
    {
        if (isTriggered && !isActiveTriggered)
        {
            timeFlash += Time.time;
            FlashSpeed();
            if (timeFlash <= Time.time)
            {
                FlashSpeed();
                isTriggered = false;
                isActive = true;
            }
        }
    }

    private void FlashSpeed()
    {
        if (isActive)
        {
            blackFlashScreen.SetActive(isActive);
            isActive = false;
            Debug.Log("Flash Screen active");
        }
        else
        {
            blackFlashScreen.SetActive(isActive);
            isActive = true;
            Debug.Log("Flash Screen unactive");
        }
    }
}