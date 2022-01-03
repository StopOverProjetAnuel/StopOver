using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DepositZone_b : MonoBehaviour
{
    public string playerTag = "Player";
    public float waitDeposit = 1;
    private float previousTime;
    public float minSpeed;
    private GameObject m_Player;
    private RessourceManager ressourceManager;
    public GameObject timerFont;
    public Material chargeMat;
    public Material unchargeMat;

    [Space(10)]

    public bool showDebug = false;


    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_Player != null)
        {
            previousTime = Time.time + waitDeposit;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = m_Player.GetComponent<Rigidbody>();
        if (other.gameObject == m_Player)
        {
            if (ressourceManager.currentRessource != 0 && rb.velocity.magnitude <= minSpeed)
            {
                if (previousTime <= Time.time)
                {
                    timerFont.GetComponent<MeshRenderer>().material = chargeMat;
                    ressourceManager.TriggerScore(1);
                    waitDeposit = Mathf.Clamp(waitDeposit - 0.75f, 0.005f, 1f);
                    previousTime = Time.time + waitDeposit;
                }

                #region Debug
                if (showDebug)
                {
                    Debug.Log("Player deposing");
                }
                #endregion
            }
            else
            {
                timerFont.GetComponent<MeshRenderer>().material = unchargeMat;
            }
            #region Debug
            if (showDebug)
            {
                Debug.Log("Player detected");
            }
            #endregion
        }
        #region Debug
        if (showDebug)
        {
            Debug.Log("Deposit Zone trigered");
            Debug.Log(other.gameObject.name);
        }
        #endregion
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_Player != null)
        {
            timerFont.GetComponent<MeshRenderer>().material = unchargeMat;
            waitDeposit = 1f;
        }
    }
}
