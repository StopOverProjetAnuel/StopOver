using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SignFullTank : MonoBehaviour
{
    public Material mat_FullTank;
    public RessourceManager ressourceManager;
    // Start is called before the first frame update
    void Start()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        mat_FullTank.SetFloat("_Fill", 0);
    }

    // Update is called once per frame
    void Update()
    {
        mat_FullTank.SetFloat("_Fill", ressourceManager.currentRessource / ressourceManager.maxRessource);
    }
}
