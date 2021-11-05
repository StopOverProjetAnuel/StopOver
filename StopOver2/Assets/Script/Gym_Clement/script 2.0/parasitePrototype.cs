using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parasitePrototype : MonoBehaviour
{
    [Space(10)]
    [Header("Speed = 300  <=>  Magnitude = 13")]
    [Header("Speed = 200  <=>  Magnitude = 6.4")]
    [Header("Speed = 100  <=>  Magnitude = 3")]
    [Header("Speed = 50   <=>  Magnitude = 2.2")]
    [Header("Speed = 20   <=>  Magnitude = 0.9")]
    [Header("Speed = 10   <=>  Magnitude = 0.5")]
    public float magnitudeToBreak = 3.0f;
    public float amountBiomass = 100.0f;
    public GameObject parasite;
    [Space(10)]
    public float repultionStrength = 5.0f;
    //[Space(10)]
    //public string managersTag = "Manager";
    //public GameObject ressourceManager;

    private void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("aïe score :" + (Mathf.Ceil(col.relativeVelocity.magnitude * 1000) / 10));

            if (col.relativeVelocity.magnitude * 100 >= magnitudeToBreak)
            {
               // RessourceManager ressourceScript = (RessourceManager)ressourceManager.GetComponent(typeof(RessourceManager));
               //ressourceScript.currentRessource += amountBiomass;
                parasite.transform.parent = col.transform;
                parasite.transform.localPosition = new Vector3(0, 1, 0);
                Destroy(this.gameObject, 0.0f);
            }
            else
            {
                col.rigidbody.AddForce(col.transform.forward * -1 * repultionStrength, ForceMode.Force);
            }
        }
    }
}
