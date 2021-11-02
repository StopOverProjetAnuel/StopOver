using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class C_CreatureNV3 : MonoBehaviour
{
    public C_ZoneCreatureNV3 _ZoneCreatureNV3;

    public NavMeshAgent creature;

    public Transform pointCenterZone;

    private float distanceCreatureDestination;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame 
    void Update()
    {
        creature.SetDestination(_ZoneCreatureNV3.currentPointToGo.position);

        distanceCreatureDestination = Vector3.Distance(_ZoneCreatureNV3.currentPointToGo.position, transform.position);

        if(distanceCreatureDestination < 0.5f)
        {
            _ZoneCreatureNV3.MakeNewDestination();
        }
    }
}
