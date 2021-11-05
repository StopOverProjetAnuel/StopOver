using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashTest : MonoBehaviour
{
    public float speed = 0;
    public int ressources = 10;
    void Update()
    {
        transform.Translate (Vector3.forward * speed * Time.deltaTime);
    }
}
