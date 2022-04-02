using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePool : PoolableItem
{
    public float lifeTime = 3f;

    float timer;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > timer + lifeTime)
		{
            ObjectPool.Instance.ReturnToPool(ID, gameObject);
            timer = Time.time;
		}
    }

    public override void OnObjectPool ()
    {
        // Du code
        timer = Time.time;
    }

	public override void OnObjectStore ()
	{
        Rigidbody r = GetComponent<Rigidbody>();

        if (r)
        {
            r.velocity = Vector3.zero;
        }

        transform.localScale = Vector3.one;
    }
}
