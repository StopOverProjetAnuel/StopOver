using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CollidedPlayer : MonoBehaviour
{
    [Header("Require Information")]
    private RessourceManager ressourceManager;

    private NudgeBars nudgeBars;

    [Header("Parameters")]
    public bool isDropOnDestroy = false;
    public static GameObject droppedItem;
    public float resourceGive;
    public int resistanceLevel;
    public bool isDestroyWithRequirement = false;
    public float ifNotRequireSpeedMultiplier = 0.75f;

    [Header("VFX Parameters")]
    [SerializeField] private string ID = "";

    [Space(10)]

    [SerializeField] private bool showDebug = false;


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
            InstantiateVFX();
        }
        else if (!isDestroyWithRequirement || nudgeBars.nbState >= resistanceLevel)
        {
            Vector3 storedVelocity = rb.velocity;
            storedVelocity.x *= ifNotRequireSpeedMultiplier;
            storedVelocity.z *= ifNotRequireSpeedMultiplier;
            rb.velocity = storedVelocity;
            GettingDestroy();
            InstantiateVFX();
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

    private void InstantiateVFX()
    {
        ObjectPool.Instance.GetFromPool(ID, transform);

        Transform g = ObjectPool.Instance.GetFromPool(ID, transform).transform;
        Transform player = FindObjectOfType<C_CharacterManager>().gameObject.transform;
        Vector3 newRotationVector = player.rotation.eulerAngles;
        g.rotation = Quaternion.Euler(newRotationVector);
    }
}