using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DepositZoneTimer : MonoBehaviour
{
    private RessourceManager ressourceManager;
    private GMTimer _Timer;
    
    //Object(s) require
    private GameObject m_Player;

    [Header("Parameters")]

    [Tooltip("Time to wait before the deposit start")]
    [SerializeField] private static float waitDeposit = 1;
    [Tooltip("All velocity that superior will cancel the deposit")]
    [SerializeField] private static float minSpeed = 5f;
    //[Tooltip("Resources that's need to gain time")]
    public static float resourcesRequire = 250f;
    [Tooltip("The greater the value is, the faster the deposit is")]
    [SerializeField] private static float resourcesTakenPerFrame = 1f;
    [Tooltip("How much time is added once the requirement(s) are done, metrix in second")]
    [SerializeField] private static float timeAdded = 60f;

    private float previousTime; //var that determine the time to wait
    private float currentResourcesTaken = 0f;
    private bool checkTimeAdd = false;

    [Space(10)]

    public bool showDebug = false;


    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        _Timer = FindObjectOfType<GMTimer>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_Player != null) previousTime = Time.time + waitDeposit; //Set the first time to wait before starting deposit
    }


    //Futur me (cedric L.)  |  use lerp with curve to drop the resources
    private void OnTriggerStay(Collider other)
    {
        #region Debug
        if (showDebug)
        {
            Debug.Log("Deposit Zone trigered");
            Debug.Log(other.gameObject.name);
        }
        #endregion
        if (other.gameObject != m_Player) return;

        Rigidbody rb = m_Player.GetComponent<Rigidbody>();
        if (ressourceManager.currentRessource == 0 && rb.velocity.magnitude > minSpeed) return;

        if (previousTime <= Time.time && currentResourcesTaken != resourcesRequire)
        {
            #region Debug
        if (showDebug)
        {
            Debug.Log("Player deposing");
        }
            #endregion
            ressourceManager.currentRessource -= 1;
            currentResourcesTaken = Mathf.Clamp(0, resourcesRequire, currentResourcesTaken + 1);
            waitDeposit = Mathf.Clamp(waitDeposit - 0.75f, 0.005f, 1f);
            previousTime = Time.time + waitDeposit;
            return;
        }

        if (!checkTimeAdd)
        {
            _Timer.currentTime += timeAdded;
            checkTimeAdd = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_Player != null) waitDeposit = 1f;
    }
}
