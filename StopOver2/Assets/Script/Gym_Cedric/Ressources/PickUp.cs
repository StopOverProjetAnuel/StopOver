using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private RessourceManager ressourceManager;
    public float ressourceGive;
    private float currentTimeStart;
    public float timeBeforeGone = 30f;

    [Space]

    public float waitBeforeActive = 1;
    private float timeSpawnSave;

    private void Start()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        currentTimeStart = Time.time + timeBeforeGone;
        timeSpawnSave = Time.time + waitBeforeActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && ressourceManager.currentRessource != ressourceManager.maxRessource && Time.time >= timeSpawnSave)
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