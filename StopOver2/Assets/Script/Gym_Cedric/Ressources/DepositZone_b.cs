using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DepositZone_b : MonoBehaviour
{
    public float waitDeposit = 1;
    private float previousTime;
    private GameObject m_Player;
    private RessourceManager ressourceManager;
    public GameObject timerFont;
    public Material chargeMat;
    public Material unchargeMat;

    private void Awake()
    {
        m_Player = FindObjectOfType<OverboardController>().gameObject;
        ressourceManager = FindObjectOfType<RessourceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OverboardController player = other.GetComponent<OverboardController>();
        if (player != null)
        {
            previousTime = Time.time + waitDeposit;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OverboardController player = other.GetComponent<OverboardController>();
        Rigidbody rb = m_Player.GetComponent<Rigidbody>();
        if (player != null && ressourceManager.currentRessource != 0 && rb.velocity.magnitude <= 1f)
        {
            if (previousTime <= Time.time)
            {
                timerFont.GetComponent<MeshRenderer>().material = chargeMat;
                ressourceManager.TriggerScore(1);
                waitDeposit = Mathf.Clamp(waitDeposit - 0.75f, 0.005f, 1f);
                previousTime = Time.time + waitDeposit;
            }
        }
        else
        {
            timerFont.GetComponent<MeshRenderer>().material = unchargeMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        OverboardController player = other.GetComponent<OverboardController>();
        if (player != null)
        {
            timerFont.GetComponent<MeshRenderer>().material = unchargeMat;
            waitDeposit = 1f;
        }
    }
}
