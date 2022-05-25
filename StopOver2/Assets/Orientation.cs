using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientation : MonoBehaviour
{

    public GameObject[] porte;
    public GameObject aiguille;

    private GameObject currentPorte;
    private int oldPorte;
    // Start is called before the first frame update
    void Start()
    {
        currentPorte = porte[0];
        oldPorte = 0;
        Debug.Log("old porte =" + oldPorte);
    }

    private void FixedUpdate()
    {
        aiguille.transform.LookAt(currentPorte.transform.position);


    }

    public void NextPorte()
    {
        currentPorte = porte[oldPorte + 1];
        oldPorte = oldPorte + 1;
        Debug.Log("NextPorte");
        Debug.Log(currentPorte);
        Debug.Log("oldporte =" + oldPorte);
    }
}
