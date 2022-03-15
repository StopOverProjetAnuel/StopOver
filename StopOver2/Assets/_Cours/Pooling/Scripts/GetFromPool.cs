using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFromPool : MonoBehaviour
{
    public string ID = "Arrow";
    public string ID2 = "Sphere";


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
		{
            ObjectPool.Instance.GetFromPool(ID, transform);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            ObjectPool.Instance.GetFromPool(ID2, transform);
        }
    }
}
