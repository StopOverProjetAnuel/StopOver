using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float verticalStrength = 10.0f;
    public AnimationCurve curve;
    public float strengthDuration = 3.0f;

    private GameObject player;
    private float timer;
    public void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;
        timer = strengthDuration;
        StartCoroutine(Bump(player));
    }

    IEnumerator Bump(GameObject p)
    { 
        timer -= Time.deltaTime;
        Mathf.Clamp(timer, 0, strengthDuration);

        float verticalDisplacement = curve.Evaluate((strengthDuration - timer) / strengthDuration) * verticalStrength * Time.deltaTime;

        if (timer != 0)
        {

            p.transform.Translate(new Vector3(0, verticalDisplacement, 0));
            yield return new WaitForFixedUpdate();
            StartCoroutine(Bump(player));
        }
    }

}
