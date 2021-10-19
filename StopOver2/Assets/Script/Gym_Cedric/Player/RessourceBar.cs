using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceBar : MonoBehaviour
{
    private RessourceManager ressourceManager;

    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
    }

    private void Update()
    {
        float b = ressourceManager.currentRessource / ressourceManager.maxRessource;
        transform.localScale = new Vector3(b, 1, 1);
    }
}