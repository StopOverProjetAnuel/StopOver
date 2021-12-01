using UnityEngine;

public class CollisionDrop : MonoBehaviour
{
    private RessourceManager ressourceManager;

    private Rigidbody rb;

    [SerializeField] GameObject prefabToSpawn;

    [Header("Parameters")]
    [SerializeField] float frontDegrees = 45f;
    private float frontRadial;
    [SerializeField] float minSpeedLose = 15f;
    [SerializeField] float resourceForceDivider = 2f;
    [SerializeField] float resourceCountPerPickUp = 10f;

    [Header("Drop Parameters")]
    [SerializeField] float spawnOffset = 1f;
    [SerializeField] float bumpForce = 1f;
    [SerializeField] float bumpUpwardModifier = 1f;

    [Header("Debug Option")]
    [SerializeField] bool showDebug = false;
    [SerializeField] bool showExplosionRadius = false;
    [SerializeField] Color explosionColor;


    private void OnEnable()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        frontRadial = frontDegrees / 90f; //convert "frontDegrees" into radial by dividing it by the max of the dot value in degrees (90°)
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 impactPoint = collision.GetContact(0).point; //take coordinate of the 1st impact
        Vector3 toCollisionPoint = impactPoint - transform.position; //give direction of the impact
        float dot = Vector3.Dot(toCollisionPoint.normalized, transform.forward); //convert degrees to radial

        if (dot < frontRadial && ressourceManager.currentRessource != 0)
        {
            float a;
            a = ressourceManager.currentRessource / resourceForceDivider;
            ressourceManager.currentRessource = a;

            float b = a / resourceCountPerPickUp;
            float c = Mathf.Floor(b);
            float d = b - c;

            for (float i = 0; i < c; i++)
            {
                SpawnItem(resourceCountPerPickUp);
            }

            if (d > 0f)
            {
                float e = d * 10;
                SpawnItem(e);
            }
            #region Debug
            if (showDebug)
            {
                float e = c / 10 + d;
                Debug.Log("Drop item : " + e);
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
