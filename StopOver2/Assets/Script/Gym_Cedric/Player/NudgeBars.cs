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
    public GameObject nudgeBars;
    public Material[] nudgeBarsMaterials;
    public float min1stState;
    public float min2ndState;

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
            nudgeBars.SetActive(true);
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

            nudgeBars.GetComponent<MeshRenderer>().material = nudgeBarsMaterials[Mathf.Clamp(nbState - 1, 0, 2)];
        }
        else
        {
            nudgeBars.SetActive(false);
            nbState = 0;
        }
    }
}
