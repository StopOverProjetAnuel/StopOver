using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giantTreePrototype : MonoBehaviour
{
    [Space(10)]
    [Header("Speed = 300  <=>  Magnitude = 13")]
    [Header("Speed = 200  <=>  Magnitude = 6.4")]
    [Header("Speed = 100  <=>  Magnitude = 3")]
    [Header("Speed = 50   <=>  Magnitude = 2.2")]
    [Header("Speed = 20   <=>  Magnitude = 0.9")]
    [Header("Speed = 10   <=>  Magnitude = 0.5")]
    public float magnitude1 = 0.0f;
    public float magnitude2 = 2.0f;
    public float magnitude3 = 3.0f;
    public float magnitude4 = 6.0f;
    public float magnitude5 = 12.0f;
    [Space(10)]
    public GameObject reward;
    [Space(10)]
    public float reward2 = 3.0f;
    public float reward3 = 9.0f;
    public float reward4 = 18.0f;
    public float reward5 = 25.0f;
    
    //la magnitude est un chiffre très bas pour pouvoir le faire exploiter par un GD non programmeur il faut le multiplier par 100


    private void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Player")
        {
            Debug.Log("aïe score :" + (Mathf.Ceil(col.relativeVelocity.magnitude * 1000) / 10));

            if (col.relativeVelocity.magnitude * 100 >= magnitude5)
            {
                StartCoroutine(SpawnReward(reward5));
            }
            else if (col.relativeVelocity.magnitude * 100 >= magnitude4)
            {
                StartCoroutine(SpawnReward(reward4));
            }
            else if (col.relativeVelocity.magnitude * 100 >= magnitude3)
            {
                StartCoroutine(SpawnReward(reward3));
            }
            else if (col.relativeVelocity.magnitude * 100 >= magnitude2)
            {
                StartCoroutine(SpawnReward(reward2));
            }
            else if (col.relativeVelocity.magnitude * 100 >= magnitude1)
            {

            }
        }
    }
    private IEnumerator SpawnReward(float t)
    {
        yield return 0;
        GameObject ressource = Instantiate(reward, (transform.position + new Vector3(0, 10, 0) + new Vector3(Random.insideUnitCircle.x * 3, 0, Random.insideUnitCircle.y * 3)), Quaternion.identity);
        Destroy(ressource, 3.0f);

        if (t >= 0.0f)
        {
            StartCoroutine(SpawnReward(t - 1));
        }
        
    }
}
