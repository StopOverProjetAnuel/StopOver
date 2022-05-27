using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientation : MonoBehaviour
{
    public bool showDebug;
    public GameObject[] porte;
    public GameObject aiguille;

    private GameObject currentPorte;
    private int oldPorte;
    // Start is called before the first frame update
    void Start()
    {
        currentPorte = porte[0];
        oldPorte = 0;
        if (showDebug)
        {
            Debug.Log("old porte =" + oldPorte);

        }
    }

    private void FixedUpdate()
    {
        aiguille.transform.LookAt(currentPorte.transform.position);


    }

    public void NextPorte()
    {
        oldPorte = oldPorte + 1;
        currentPorte = porte[oldPorte];

        if (showDebug)
        {
            Debug.Log("NextPorte");
            Debug.Log(currentPorte);
            Debug.Log("oldporte =" + oldPorte);

        }
    }

    public void NextPorteSecret(int nbPorteBetweenNextPorte)
    {
        oldPorte = oldPorte + nbPorteBetweenNextPorte;
        currentPorte = porte[oldPorte];
    }
}
