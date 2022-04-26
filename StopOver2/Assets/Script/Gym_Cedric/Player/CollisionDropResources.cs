using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDropResources : MonoBehaviour
{
    [Header("Parameters Require")]
    private RessourceManager ressourceManager;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject prefabToSpawn;

    [Header("Parameters")]
    private float frontRadial;
    [SerializeField] private float minSpeedToLose = 15f;
    [SerializeField] private float maxSpeedToLose = 100f;
    [SerializeField] private float resourceMaxLoseInPercent = 50f;
    private float resourceForceDivider = 1f;
    [SerializeField] private float resourceCountPerPickUp = 10f;

    [Header("Drop Parameters")]
    [SerializeField] private float frontDegrees = 45f;
    [SerializeField] private float spawnOffset = 1f;
    [SerializeField] private float bumpForce = 1f;
    [SerializeField] private float bumpUpwardModifier = 1f;

    [Header("Debug Option")]
    [SerializeField] private bool showDebug = false;
    [SerializeField] private bool showExplosionRadius = false;
    [SerializeField] private Color explosionColor;


    private void OnEnable()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        rb = GetComponent<Rigidbody>();
        frontRadial = frontDegrees / 90f; //convert "frontDegrees" into radial by dividing it by the max of the dot value in degrees (90°)
    }



    private void OnCollisionEnter(Collision collision)
    {
        Vector3 impactPoint = collision.GetContact(0).point; //take coordinate of the 1st impact
        Vector3 toCollisionPoint = impactPoint - transform.position; //give direction of the impact
        toCollisionPoint.y = 0f;
        Vector3 forward = transform.forward;
        forward.y = 0f;
        float dot = Vector3.Dot(toCollisionPoint.normalized, forward); //convert degrees to radial

        if (dot < frontRadial && ressourceManager.currentRessource != 0 && collision.relativeVelocity.magnitude >= minSpeedToLose)
        {
            float maxL = maxSpeedToLose - minSpeedToLose;
            float cL = Mathf.Clamp(collision.relativeVelocity.magnitude - minSpeedToLose, 0.0001f, maxL);
            resourceForceDivider = Mathf.Clamp(cL / maxL, 0, 1);
            float currentDivider = (resourceMaxLoseInPercent / 100) * resourceForceDivider;

            float resourcesDropped = ressourceManager.currentRessource * currentDivider;
            ressourceManager.currentRessource = Mathf.Clamp(ressourceManager.currentRessource - resourcesDropped, 0, ressourceManager.maxRessource);

            float pickUpAmount = resourcesDropped / resourceCountPerPickUp;
            float pickUpAmountRounded = Mathf.Floor(pickUpAmount);
            float lastAmount = pickUpAmount - pickUpAmountRounded;

            for (float i = 0; i < pickUpAmountRounded; i++)
            {
                SpawnItem(resourceCountPerPickUp);
            }

            if (lastAmount > 0f)
            {
                float realLastAmount = lastAmount * 10;
                ressourceManager.currentRessource += realLastAmount;
            }
            #region Debug
            if (showDebug)
            {
                float e = pickUpAmountRounded / 10 + lastAmount;
                Debug.Log("Drop item : " + e);
                Debug.Log("Impact Force : " + collision.relativeVelocity.magnitude);
                Debug.Log("Current resource lose(s) : " + resourcesDropped);
                Debug.Log("Player lose " + (currentDivider * 100) + "% of his current resource");
            }
            #endregion
        }

        #region Debug
        if (showDebug)
        {
            Debug.Log("Collision Check. Vector dot : " + dot);
        }
        #endregion
    }

    private void SpawnItem(float resourceGive)
    {
        #region Debug
        if (showDebug)
        {
            Debug.Log("Item Spawn");
        }
        #endregion
        Vector3 offset = Random.insideUnitSphere * spawnOffset;
        offset.y = 0.5f;

        GameObject item = Instantiate(prefabToSpawn, transform.position + offset, Quaternion.identity);
        Rigidbody rbItem = item.GetComponent<Rigidbody>();

        if (rbItem)
        {
            rbItem.AddExplosionForce(bumpForce, transform.position, spawnOffset, bumpUpwardModifier);
            #region Debug
            if (showDebug)
            {
                Debug.Log("Explosion Triggered");
            }
            #endregion
        }

        PickUp puItem = item.GetComponent<PickUp>();

        if (puItem)
        {
            puItem.ressourceGive = resourceGive;
            #region Debug
            if (showDebug)
            {
                Debug.Log("Item Resource Count = " + resourceGive);
            }
            #endregion
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showExplosionRadius)
        {
            Gizmos.color = explosionColor;
            Gizmos.DrawWireSphere(transform.position, spawnOffset);
        }
    }
}
