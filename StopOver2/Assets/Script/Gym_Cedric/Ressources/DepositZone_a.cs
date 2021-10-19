using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DepositZone_a : MonoBehaviour
{
    /**public float waitDeposit = 3;
    private float previousTime;
    private GameObject m_Player;
    private OverboardController overboardController;
    public float waitAnimation = 2;
    private float previousTimeWait;
    private RessourceManager ressourceManager;
    public GameObject timerFont;
    public GameObject timerTextObj;
    private TextMeshProUGUI timerText;
    public Material unlockMat;
    public Material lockMat;
    private bool countdownLocker;

    private void Awake()
    {
        m_Player = FindObjectOfType<OverboardController>().gameObject;
        overboardController = m_Player.GetComponent<OverboardController>();
        timerText = timerTextObj.GetComponent<TextMeshProUGUI>();
        ressourceManager = FindObjectOfType<RessourceManager>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OverboardController player = other.GetComponent<OverboardController>();
        if (player != null)
        {
            previousTime = Time.time + waitDeposit;
            previousTimeWait = previousTime + waitAnimation;
            countdownLocker = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OverboardController player = other.GetComponent<OverboardController>();
        if (player != null)
        {
            if (previousTime <= Time.time)
            {
                overboardController.enabled = false;
                timerFont.GetComponent<MeshRenderer>().material = lockMat;
                m_Player.GetComponent<Rigidbody>().drag = 1000f;
            }

            if (previousTimeWait <= Time.time)
            {
                ressourceManager.TriggerScore(ressourceManager.currentRessource);
                overboardController.enabled = true;
                timerFont.GetComponent<MeshRenderer>().material = unlockMat;
                m_Player.GetComponent<Rigidbody>().drag = 1f;
            }
        }

        if (countdownLocker == true)
        {
            float t = previousTime - Time.time;
            timerText.SetText("" + Mathf.RoundToInt(t));
            if (t <= 0)
            {
                timerText.SetText("");
                countdownLocker = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (previousTime <= Time.time && previousTimeWait >= Time.time)
        {
            m_Player.transform.position = transform.position;
        }
        else
        {
            timerText.SetText("");
        }
    }*/
}
