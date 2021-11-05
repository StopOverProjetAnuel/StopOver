using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystalWallPrototype : MonoBehaviour
{
    [Space(10)]
    [Header("Speed = 300  <=>  Magnitude = 13")]
    [Header("Speed = 200  <=>  Magnitude = 6.4")]
    [Header("Speed = 100  <=>  Magnitude = 3")]
    [Header("Speed = 50   <=>  Magnitude = 2.2")]
    [Header("Speed = 20   <=>  Magnitude = 0.9")]
    [Header("Speed = 10   <=>  Magnitude = 0.5")]
    public float magnitudeToBreak = 3.0f;
    public float speedPenality = 10.0f;
    [Space(10)]
    public float repultionStrength = 5.0f;


    //la magnitude est un chiffre très bas pour pouvoir le faire exploiter par un D non programmeur il faut le multiplier par 100
    private void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Player")
        {
                //Debug.Log("aïe score :" + (Mathf.Ceil(col.relativeVelocity.magnitude * 1000) / 10));

           if (col.relativeVelocity.magnitude * 100 >= magnitudeToBreak)
           {
                col.rigidbody.AddForce(col.transform.forward * -1 * speedPenality, ForceMode.Force);
               Destroy(this.gameObject, 0.0f);
           }
           else
           {
               col.rigidbody.AddForce(col.transform.forward * -1 * repultionStrength, ForceMode.Force);
           }
        }
    }
}