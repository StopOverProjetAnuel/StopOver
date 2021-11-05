using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapWall : MonoBehaviour
{
    public KeyCode key2Reload;
    public float speedPenality = 5.0f;
    public SpanwerPlayer Player;

    public void OnCollisionEnter(Collision col)
    {
        // if (col.relativeVelocity.magnitude > 20) code mis de coté qui sera surement repris plus tard.
        if (Player.playersSpeed > 20.0f)
        {
            foreach (Transform child in transform)
            {
                gameObject.SetActive(false);
            }

            if (col.gameObject.GetComponent<CrashTest>())
            {
                col.gameObject.GetComponent<CrashTest>().speed -= speedPenality ;
            }
        }
    }
}