using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private RessourceManager ressourceManager;
    public float ressourceGive;
    private float currentTimeStart;
    public float timeBeforeGone = 30f;

    private void Start()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        currentTimeStart = Time.time + timeBeforeGone;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && ressourceManager.currentRessource != ressourceManager.maxRessource)
        {
            ressourceManager.TriggerRessourceCount(ressourceGive);
            GameObject.Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (currentTimeStart < Time.time)
        {
            Destroy(gameObject);
        }
    }
}