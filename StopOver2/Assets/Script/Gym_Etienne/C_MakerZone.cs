using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MakerZone : MonoBehaviour
{
    [Header("Point Zone")]
    public Transform[] arrayPointZone;
    [Space]
    public float maxX;
    public float maxZ;
    public float minX;
    public float minZ;
    [Space]
    [Header("Parametre Mob")]
    public GameObject mob;
    public int maxMobSpawn;
    public List<GameObject> mobsInZone = new List<GameObject>();
    [Space]
    //public List<float> positionSave = new List<float>();


    private Vector3 positionToCalcul;
    private GameObject currentMob;
    private float currentPositionX;
    private float currentPositionZ;
    private Vector3 currentPosition;
    private bool canSpawn;

    [SerializeField]private float currentMaxX;
    [SerializeField] private float currentMinX;
    [SerializeField] private float currentMaxZ;
    [SerializeField] private float currentMinZ;
    
    //public Transform[] arrayPoint;
    // Start is called before the first frame update
    void Start()
    {
        currentMaxX = maxX;
        currentMaxZ = maxZ;
        currentMinX = minX;
        currentMinZ = minZ;
        
        CreateShape();

        canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            SpawnMobs();
        }
    }

    void CreateShape()
    {
        foreach (Transform currentPointZone in arrayPointZone)
        {
            positionToCalcul = currentPointZone.transform.position;

            if(positionToCalcul.x > currentMaxX)
            {
                currentMaxX = positionToCalcul.x;
            }
            if(positionToCalcul.x < currentMinX)
            {
                currentMinX = positionToCalcul.x;
            }
            if(positionToCalcul.z > currentMaxZ)
            {
                currentMaxZ = positionToCalcul.z;
            }
            if(positionToCalcul.z < currentMinZ)
            {
                currentMinZ = positionToCalcul.z;
            }
            
            
        }
    }

    void SpawnMobs()
    {
        for (int i = 0; i < maxMobSpawn +1; i++)
        {
            Debug.Log(i);
            currentPositionX = Random.Range(currentMinX, currentMaxX);
            currentPositionZ = Random.Range(currentMinZ, currentMaxZ);
            currentPosition = new Vector3(currentPositionX, transform.position.y, currentPositionZ);
            currentMob = Instantiate(mob , currentPosition, Quaternion.identity, transform);

            mobsInZone.Add(currentMob);
            if(i >= maxMobSpawn)
            {
                canSpawn = false;
            }

        }
    }
}
