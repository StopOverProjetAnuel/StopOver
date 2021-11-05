using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crashTest2 : MonoBehaviour
{
    public float speed = 0;
    public int ressources = 10;

    [Space]
    [Space]
    [Space]

    [Header("stabilisateur d'altitude")]
    public float YPos = 1;
    public float altiltudeSpeed = 10;
    public float brakingStrength = 10;

    private Rigidbody rb;
    private void Start()
    {
        if(rb == null)
        {
           rb = GetComponent<Rigidbody>();
        }
    }
    void Update()
    {
        rb.AddForce(Vector3.forward * speed * Time.deltaTime * 1000 , ForceMode.Acceleration) ;

        rb.AddForce(new Vector3(0,(YPos - transform.position.y) * Time.deltaTime * altiltudeSpeed *100, 0), ForceMode.Force);
        if(Mathf.Ceil(transform.position.y - YPos) < 1.0f)
        {
            rb.velocity = rb.velocity/brakingStrength;
        }
    }
}

