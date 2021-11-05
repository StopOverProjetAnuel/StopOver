using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanwerPlayer : MonoBehaviour
{
    public GameObject crashTestObject;
    public float playersSpeed = 10;
    public KeyCode Key = KeyCode.F1;
    public float lifeTime = 10;


    void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            GameObject spawnedPlayer = Instantiate(crashTestObject ,transform.position + new Vector3 (0, 0, 1) , transform.rotation);
            crashTest2 crashTestScript = (crashTest2)spawnedPlayer.GetComponent(typeof(crashTest2));

            if(crashTestScript != null)
               crashTestScript.speed = playersSpeed ;

            Destroy(spawnedPlayer, lifeTime) ;
        }
    }
}
