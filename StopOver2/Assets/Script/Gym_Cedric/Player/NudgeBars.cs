using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NudgeBars : MonoBehaviour
{
    [Header("Player Information")]
    public GameObject m_Player;
    private Rigidbody rb;

    [HideInInspector] public int nbState;

    [Header("Nugde Bars States")]
    public GameObject nudgeBars_model;
    public Material[] nudgeBarsMaterials;
    public float min1stState;
    public float min2ndState;

    //Object Collision
    private CollidedPlayer collidedPlayer;

    [Header("Debug Option")]
    public bool showDebug = false;

    private void Awake()
    {
        rb = m_Player.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        NudgeBarsState();
    }

    private void NudgeBarsState()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            //nudgeBars_model.SetActive(true);
            if (rb.velocity.magnitude >= min2ndState)
            {
                nbState = 3;
            }
            else if (rb.velocity.magnitude >= min1stState)
            {
                nbState = 2;
            }
            else
            {
                nbState = 1;
            }

            //nudgeBars_model.GetComponent<MeshRenderer>().material = nudgeBarsMaterials[nbState - 1];
        }
        else
        {
            //nudgeBars_model.SetActive(false);
            nbState = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.TryGetComponent<CollidedPlayer>(out collidedPlayer))
        {
            if (parent.CompareTag("Destructible"))
            {
                collidedPlayer.TriggerCollisionPlayer(rb);
                #region Debug
                if (showDebug)
                {
                    Debug.Log("NudgeBars get triggered with a destructible object");
                }
                #endregion
            }
        }
        else
        {
            if (other.CompareTag("Destructible"))
            {
                collidedPlayer = other.GetComponent<CollidedPlayer>();
                collidedPlayer.TriggerCollisionPlayer(rb);
                #region Debug
                if (showDebug)
                {
                    Debug.Log("NudgeBars get triggered with a destructible object");
                }
                #endregion
            }
        }
        #region Debug
        if (showDebug)
        {
            Debug.Log("NudgeBars get triggered");
        }
        #endregion
    }
}
