using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidedPlayer : MonoBehaviour
{
    [Header("Require Information")]
    private RessourceManager ressourceManager;

    private NudgeBars nudgeBars;

    [Header("Parameters")]
    public bool isDropOnDestroy = false;
    public GameObject droppedItem;
    public float resourceGive;
    public int resistanceLevel;

    [Space(10)]

    public bool showDebug = false;


    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        nudgeBars = FindObjectOfType<NudgeBars>();
    }

    #region OLD
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
    #endregion

    public void TriggerCollisionPlayer(Rigidbody rb)
    {
        if (nudgeBars.nbState >= resistanceLevel + 1)
        {
            GettingDestroy();
        }
        else
        {
            Vector3 storedVelocity = rb.velocity;
            storedVelocity.x *= 0.5f;
            storedVelocity.z *= 0.5f;
            rb.velocity = storedVelocity;
            GettingDestroy();
        }

        #region Debug
        if (showDebug)
        {
            Debug.Log(gameObject.name + " have been triggered");
        }
        #endregion
    }

    private void GettingDestroy()
    {

        if (ressourceManager.currentRessource != ressourceManager.maxRessource)
        {
            ressourceManager.TriggerRessourceCount(resourceGive);
        }
        else if (isDropOnDestroy)
        {
            GameObject item = Instantiate(droppedItem, this.transform.position, Quaternion.identity);
            item.GetComponent<PickUp>().ressourceGive = resourceGive;
        }

        gameObject.SetActive(false);

        #region Debug
        if (showDebug)
        {
            Debug.Log(gameObject.name + " task executed");
        }
        #endregion
    }
}