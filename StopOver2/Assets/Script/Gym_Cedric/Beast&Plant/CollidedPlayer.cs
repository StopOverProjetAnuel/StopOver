using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidedPlayer : MonoBehaviour
{
    private RessourceManager ressourceManager;

    private NudgeBars nudgeBars;
    public string playerWeaponTagName;

    public GameObject droppedItem;
    public float resourceGive;
    public int resistanceLevel;



    private void Start()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        nudgeBars = FindObjectOfType<NudgeBars>();
    }


    /**private void OnCollisionEnter(Collision col)
    {
        Collider other = col.collider;
        if (other.CompareTag(playerWeaponTagName) && nudgeBars.nbState >= resistanceLevel + 1)
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
    }*/

    public void TriggerCollisionPlayer()
    {
        if (nudgeBars.nbState >= resistanceLevel + 1)
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
            //Debug.Log(gameObject.name + " task executed");
        }
        //Debug.Log(gameObject.name + " have been triggered");
    }
}