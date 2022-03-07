using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreTMP : MonoBehaviour
{
    public TextMeshPro TMP;
    public int score;

    void Start()
    {
        TMP = GetComponent<TextMeshPro>();
    }


    void Update()
    {
        TMP.text = score.ToString();
    }
}
