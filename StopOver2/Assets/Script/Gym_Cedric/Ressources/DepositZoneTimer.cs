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

    [Tooltip("Time to wait before the deposit start | WARNING | this need to be the same time as the 2nd key frame from the animation curve")]
    [SerializeField] private float depositMaxTime = 1f;
    [SerializeField] private AnimationCurve smoothDeposit = new AnimationCurve();
    [Tooltip("All velocity that superior will cancel the deposit")]
    [SerializeField] private float minSpeed = 5f;
    [Tooltip("Resources that's need to gain time")]
    [SerializeField] private float resourcesRequire = 250f;
    [Tooltip("The greater the value is, the faster the deposit is")]
    [SerializeField] private float resourcesTakenPerFrame = 1f;
    [Tooltip("How much time is added once the requirement(s) are done, metrix in second")]
    [SerializeField] private float timeAdded = 60f;

    private float currentTimer; //var that determine the time to wait
    private float currentResourcesTaken = 0f;
    //private bool checkTimeAdd = false;
    private float saveCurrentResources;

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
        if (other.gameObject.tag != "Player") return;

        saveCurrentResources = ressourceManager.currentRessource;
    }


    //Futur me (cedric L.)  |  use lerp with curve to drop the resources
    private void OnTriggerStay(Collider other)
    {
        #region Debug
        if (showDebug)
        {
            Debug.Log("Deposit Zone trigered");
            Debug.Log(other.gameObject.tag.ToString());
        }
        #endregion
        if (other.gameObject.tag != "Player") return;

        Debug.Log("collider is a player");
        Rigidbody rb = m_Player.GetComponent<Rigidbody>();
        if (ressourceManager.currentRessource == 0 && rb.velocity.magnitude > minSpeed) return;

        Debug.Log("player can deposit");
        if (currentTimer <= depositMaxTime && currentResourcesTaken != resourcesRequire)
        {
            #region Debug
        if (showDebug)
        {
            Debug.Log("Player deposing");
        }
            #endregion
            currentTimer = Mathf.Clamp(currentTimer + 1 * Time.deltaTime / 2, 0, depositMaxTime);
            float smoothLerp = smoothDeposit.Evaluate(currentTimer);
            currentResourcesTaken = Mathf.Lerp(0, resourcesRequire, smoothLerp);
            ressourceManager.currentRessource = Mathf.Lerp(saveCurrentResources, saveCurrentResources - resourcesRequire, smoothLerp);
            _Timer.currentTime += timeAdded * Time.deltaTime / 2 / depositMaxTime;
            return;
        }

        //Add all the time at the end
        /*if (!checkTimeAdd && currentTimer >= depositMaxTime)
        {
            _Timer.currentTime += timeAdded;
            checkTimeAdd = true;
        }*/
    }
}
