using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
    [ExecuteInEditMode]
public class FPSIndicator : MonoBehaviour
{
    [HideInInspector] public TextMeshPro TMP;
    [SerializeField] private float f = 0.1f;
    private WaitForSeconds seconds;

    private void Start()
    {
        seconds = new WaitForSeconds(f);
        StartCoroutine(FPS());
    }
    public IEnumerator FPS()
    {
        TMP.text = (Mathf.Ceil(1 / Time.deltaTime * 10)) / 10 + " fps"; 
        yield return seconds;
        StartCoroutine(FPS());
    }
}
