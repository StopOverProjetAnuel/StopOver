using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float speedBonus = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"));
        {
            other.GetComponent<CrashTest>().speed += speedBonus;
        }

    }

}
