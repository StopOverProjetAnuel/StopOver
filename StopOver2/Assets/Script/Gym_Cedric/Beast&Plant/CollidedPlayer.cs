using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidedPlayer : MonoBehaviour
{
    private RessourceManager ressourceManager;
    public GameObject droppedItem;
    public string playerTagName;
    public float resourceGive;

    private void Start()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTagName)
        {
            if (ressourceManager.currentRessource != ressourceManager.maxRessource)
            {
                ressourceManager.TriggerRessourceCount(resourceGive);
            }
            else
            {
                GameObject item = Instantiate(droppedItem, this.transform.position, Quaternion.identity);
                item.GetComponent<PickUp>().ressourceGive = resourceGive;
            }
            Transform.Destroy(gameObject);
        }
    }
}