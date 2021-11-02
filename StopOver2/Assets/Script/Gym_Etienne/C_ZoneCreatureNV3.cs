using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ZoneCreatureNV3 : MonoBehaviour
{

    public float radiusZone;

    public Transform currentPointToGo;

    // Start is called before the first frame update
    void Start()
    {
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
}
