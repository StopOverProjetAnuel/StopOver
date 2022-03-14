using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePool : MonoBehaviour
{
    public string ID;
    public float lifeTime = 3f;

    float timer;
    // Start is called before the first frame update
    void OnEnable()
    {
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > timer + lifeTime)
		{
            ObjectPool.Instance.ReturnToPool(ID, gameObject);
            timer = Time.time;
		}
    }
}
