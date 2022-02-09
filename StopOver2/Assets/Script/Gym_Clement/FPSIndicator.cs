using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
    [ExecuteInEditMode]
public class FPSIndicator : MonoBehaviour
{
    [HideInInspector]
    public TextMeshPro TMP;

    private void Start()
    {
        StartCoroutine(FPS());
    }
    public IEnumerator FPS()
    {
        TMP.text = (Mathf.Ceil(1 / Time.deltaTime * 10)) / 10 + " fps"; 
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FPS());
    }
}
