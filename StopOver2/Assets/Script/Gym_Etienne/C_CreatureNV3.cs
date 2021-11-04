using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class C_CreatureNV3 : MonoBehaviour
{
    public C_ZoneCreatureNV3 _ZoneCreatureNV3;

    public NavMeshAgent creature;

    public Transform pointCenterZone;

    public float minTimeBetweenDrink;
    public float maxTimeBetweenDrink;

    public float minTimeBetweenEat;
    public float maxTimeBetweenEat;

    private float distanceCreatureDestination;

    private bool haveToDrink;
    private bool haveToEat;

    private float t1;
    private float currentTimeBetweenDrink;

    private float t2;
    private float currentTimeBetweenEat;
    // Start is called before the first frame update
    void Start()
    {
        haveToDrink = false;
        haveToEat = false;

        NewDestination();
    }

    // Update is called once per frame 
    void Update()
    {
        creature.SetDestination(_ZoneCreatureNV3.currentPointToGo.position);

        distanceCreatureDestination = Vector3.Distance(_ZoneCreatureNV3.currentPointToGo.position, transform.position);


        if(distanceCreatureDestination < 0.5f)
        {
            if (_ZoneCreatureNV3.goToWater)
            {
                currentTimeBetweenDrink = Random.Range(minTimeBetweenDrink, maxTimeBetweenDrink);
                haveToDrink = false;
                _ZoneCreatureNV3.goToWater = false;
            }

            if (_ZoneCreatureNV3.goToEat)
            {
                currentTimeBetweenEat = Random.Range(minTimeBetweenEat, maxTimeBetweenEat);
                haveToEat = false;
                _ZoneCreatureNV3.goToEat = false;
            }

            if (haveToDrink)
            {
                FoundWater();
            }else if(!haveToDrink && haveToEat)
            {
                FoundFood();
            }

            if(!haveToDrink && !haveToEat)
            {
                NewDestination();
            }

        }

        

    }


    private void FixedUpdate()
    {
        if (!haveToDrink)
        {
            if(t1 >= 1)
            {
                haveToDrink = true;
                t1 = 0;
            }
            else
            {
                t1 += Time.deltaTime / currentTimeBetweenDrink;
            }

        }

        if (!haveToEat)
        {
            if(t2 >= 1)
            {
                haveToEat = true;
                t2 = 0;
            }
            else
            {
                t2 += Time.deltaTime / currentTimeBetweenEat;
            }
        }
    }

    private void NewDestination()
    {
        _ZoneCreatureNV3.MakeNewDestination();
    }

    private void FoundFood()
    {
        _ZoneCreatureNV3.SelectFood();
    }

    private void FoundWater()
    {
        _ZoneCreatureNV3.FoundWater();
    }
}
