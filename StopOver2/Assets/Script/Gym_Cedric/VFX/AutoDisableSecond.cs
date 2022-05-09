using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisableSecond : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float timeToWait = 3f;
    private WaitForSeconds timer;


    private void Awake()
    {
        timer = new WaitForSeconds(timeToWait);
    }

    private void OnEnable()
    {
        StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        yield return timer;
        gameObject.SetActive(false);
    }
}
