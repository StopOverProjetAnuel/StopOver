using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private RessourceManager ressourceManager;
    public float ressourceGive;

    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && ressourceManager.currentRessource != ressourceManager.maxRessource)
        {
            ressourceManager.TriggerRessourceCount(ressourceGive);
            GameObject.Destroy(this.gameObject);
        }
    }
}