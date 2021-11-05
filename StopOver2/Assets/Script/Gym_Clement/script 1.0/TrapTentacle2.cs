using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTentacle2 : MonoBehaviour
{
    public TrapTentacle t;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            t.TouchPlayer();
        }
    }
}
