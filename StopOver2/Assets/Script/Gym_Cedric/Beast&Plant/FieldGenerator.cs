using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public GameObject[] spawnObjects;
    public LayerMask ground = 0;

    //Object Variables
    [HideInInspector] public int spawnAmmount = 1;
    [HideInInspector] public Vector3 minRandomRotation = Vector3.zero;
    [HideInInspector] public Vector3 maxRandomRotation = Vector3.zero;
    [HideInInspector] public float minRandomScale = 1f;
    [HideInInspector] public float maxRandomScale = 1f;

    //Spawner Variables
    [HideInInspector] public int whichSpawner = 0;
    [HideInInspector] public float spawnRadius = 10f;
    [HideInInspector] public Vector2 spawnWidthNLength = new Vector2(10, 10);

    private void Awake()
    {
        //Call method
        GenerateField();
    }

    public void GenerateField()
    {
        //Execute the spawn for the ammount require
        for (int i = 0; i < spawnAmmount; i++)
        {
            //Get the iniate position of the spawner in x and z
            float transformX = this.transform.position.x;
            float transformZ = this.transform.position.z;

            //Initiate temporal variables
            float randomSpawnX = 0;
            float randomSpawnZ = 0;

            //Choose between diffenrent spawning form
            if (whichSpawner == 0) //The circle one
            {
                //Randomise inseide a circle
                Vector2 randomSpawn = Random.insideUnitCircle * spawnRadius;
                //Add the randomise with the actual spawner position
                Vector2 transformVec2 = new Vector2(randomSpawn.x + transformX, randomSpawn.y + transformZ);

                //Attribute the Vector2 values for the new Position
                randomSpawnX = transformVec2.x;
                randomSpawnZ = transformVec2.y;
            }
            else if (whichSpawner == 1) //The square one
            {
                //Set random float for a random Vector3 x and z
                randomSpawnX = Random.Range(-spawnWidthNLength.x + transformX, spawnWidthNLength.x + transformX);
                randomSpawnZ = Random.Range(-spawnWidthNLength.y + transformZ, spawnWidthNLength.y + transformZ);
            }

            //Set random Vector3 with the both previous float
            Vector3 randomPosition = new Vector3(randomSpawnX, 200, randomSpawnZ);

            //Set the ground height
            RaycastHit groundHit;
            if (Physics.Raycast(randomPosition, Vector3.down, out groundHit, 250, ground))
            {
                randomPosition = groundHit.point;
            }

            //Set random Vector3
            float randomRx = Random.Range(minRandomRotation.x, maxRandomRotation.x);
            float randomRy = Random.Range(minRandomRotation.y, maxRandomRotation.y);
            float randomRz = Random.Range(minRandomRotation.z, maxRandomRotation.z);
            Vector3 randomVector3 = new Vector3(randomRx, randomRy, randomRz);

            //Set the rotation with the random Vector3
            Quaternion randomRotation = Quaternion.Euler(randomVector3);

            //Choose a random gameobject
            int randomObject = Random.Range(0, spawnObjects.Length);

            //Spawn the object
            GameObject spawnedObject = Instantiate(spawnObjects[randomObject], randomPosition, randomRotation);

            //Set random Scale
            float randomScale = Random.Range(minRandomScale, maxRandomScale);
            spawnedObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            //Name the object with the i value
            spawnedObject.name = spawnedObject.name + " " + "n°" + i;

            //Place it as a child of the generator
            spawnedObject.transform.parent = this.transform;
        }
    }

    #region Select Spawner Form
    public void SelectCircle()
    {
        whichSpawner = 0;
    }

    public void SelectSquarre()
    {
        whichSpawner = 1;
    }
    #endregion

    public void RegenerateField()
    {
        ResetField();
        GenerateField();
    }

    public void ResetField()
    {
        if (this.transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                if (Application.isEditor == true)
                {
                    DestroyImmediate(child.gameObject, true);
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}