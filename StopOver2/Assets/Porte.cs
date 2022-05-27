using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    public bool showDebug;
    public Orientation orientation;
    public bool secretPorte;
    public int nbPorteBetweenNextPorte;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (secretPorte)
            {
                orientation.NextPorteSecret(nbPorteBetweenNextPorte);
            }
            else
            {
                orientation.NextPorte();

            }
            if (showDebug)
            {
                Debug.Log("characterhit");

            }
            this.gameObject.SetActive(false);
        }
    }
}
