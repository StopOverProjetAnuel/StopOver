using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidedPlayer : MonoBehaviour
{
    private RessourceManager ressourceManager;

    private GameObject m_Player;
    private OverboardController overboardController;
    public string playerWeaponTagName;

    public GameObject droppedItem;
    public float resourceGive;
    public int resistanceLevel;



    private void Start()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();

        m_Player = GameObject.FindWithTag("Player");
        overboardController = m_Player.GetComponent<OverboardController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerWeaponTagName) && overboardController.nbState >= resistanceLevel + 1)
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