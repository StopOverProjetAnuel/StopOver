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
    [HideInInspector] public float spawnRadiusX = 10f;
    [HideInInspector] public float spawnRadiusZ = 10f;

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

            //Set random float for a random Vector3 x and z
            float randomSpawnX = Random.Range(-spawnRadiusX + transformX, spawnRadiusX + transformX);
            float randomSpawnZ = Random.Range(-spawnRadiusZ + transformZ, spawnRadiusZ + transformZ);

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

    public void RegenerateField()
    {
        ResetField();
        GenerateField();
    }

    public void ResetField()
    {
        if (this.transform.childCount != 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
