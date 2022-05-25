using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    public bool showDebug;
    public Orientation orientation;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            orientation.NextPorte();
            if (showDebug)
            {
                Debug.Log("characterhit");

            }
            this.gameObject.SetActive(false);
        }
    }
}
