using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ZoneCreatureNV3 : MonoBehaviour
{

    public float radiusZone;
    public GameObject[] foodCreatureNV3;
    public GameObject water;

    [Space]

    public Transform currentPointToGo;
    public GameObject currentFoodSelected;

    public bool goToWater;
    public bool goToEat;
    // Start is called before the first frame update
    void Start()
    {
        goToWater = false;
        goToEat = false;

        MakeNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeNewDestination()
    {
        currentPointToGo.localPosition = Random.insideUnitSphere * radiusZone;
        currentPointToGo.position = new Vector3(currentPointToGo.position.x, 0.85f, currentPointToGo.position.z);
    }

    public void SelectFood()
    {
        goToEat = true;

        currentFoodSelected = foodCreatureNV3[Random.Range(0, foodCreatureNV3.Length)];
        currentPointToGo.position = currentFoodSelected.transform.position;
    }

    public void FoundWater()
    {
        goToWater = true;

        currentPointToGo.position = water.transform.position;
    }
}
