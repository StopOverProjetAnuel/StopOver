using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class C_CreatureNV3 : MonoBehaviour
{
    public C_ZoneCreatureNV3 _ZoneCreatureNV3;
    public NavMeshAgent creature;
    public float speedBase;
    public float speedFuit;
    public Transform pointCenterZone;

    public GameObject charcter;
    public float distanceMinCreatureCharcter;

    [Space]
    public float minTimeBetweenDrink;
    public float maxTimeBetweenDrink;
    [Space]
    public float minTimeBetweenEat;
    public float maxTimeBetweenEat;
    [Space]
    public float minCooldownBetweenFase;
    public float maxCooldownBetweenFase;

    private float distanceCreatureDestination;
    private float distanceCreatureCharcter;

    private bool haveToDrink;
    private bool haveToEat;

    private float t1;
    private float currentTimeBetweenDrink;

    private float t2;
    private float currentTimeBetweenEat;

    private float t3;
    private float currentCooldownBetweenFase;
    private bool cooldownBetweenFase;
    private bool haveToDefCooldownTime;

    private bool fuit;
    private bool selectPointFuit;
    private Vector3 currentPointFuit;
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
        Debug.Log(fuit);

        if (!cooldownBetweenFase)
        {
            creature.SetDestination(_ZoneCreatureNV3.currentPointToGo.position);
        }

        distanceCreatureCharcter = Vector3.Distance(charcter.transform.position, transform.position);

        if(distanceCreatureCharcter < distanceMinCreatureCharcter)
        {
            if (!selectPointFuit)
            {
                SelectPointFuit();               
            }

            creature.speed = speedFuit;
            fuit = true;
        }else if(distanceCreatureCharcter > (distanceMinCreatureCharcter * 2.5))
        {
            creature.speed = speedBase;
            selectPointFuit = false;
            fuit = false;
        }
        
        distanceCreatureDestination = Vector3.Distance(_ZoneCreatureNV3.currentPointToGo.position, transform.position);

        if(fuit && distanceCreatureDestination < 0.5f)
        {
                SelectPointFuit();            
        }

        if (!fuit)
        {
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

                if (haveToDrink && !cooldownBetweenFase)
                {
                    cooldownBetweenFase = true;
                    FoundWater();
                }else if(!haveToDrink && haveToEat && !cooldownBetweenFase)
                {
                    cooldownBetweenFase = true;
                    FoundFood();
                }

                if(!haveToDrink && !haveToEat && !cooldownBetweenFase)
                {
                    cooldownBetweenFase = true;
                    NewDestination();
                }

            }

        }

        if (cooldownBetweenFase)
        {
            if (haveToDefCooldownTime)
            {
                currentCooldownBetweenFase = Random.Range(minCooldownBetweenFase, maxCooldownBetweenFase);
                haveToDefCooldownTime = false;
            }
            CooldownBetweenFase();
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

    private void CooldownBetweenFase()
    {
        if(t3 >= 1)
        {
            cooldownBetweenFase = false;
            haveToDefCooldownTime = true;
            t3 = 0;
        }
        else
        {
            t3 += Time.deltaTime / currentCooldownBetweenFase;
        }
    }

    private void SelectPointFuit()
    {
        _ZoneCreatureNV3.Fuit();
        selectPointFuit = true;
    }
}
