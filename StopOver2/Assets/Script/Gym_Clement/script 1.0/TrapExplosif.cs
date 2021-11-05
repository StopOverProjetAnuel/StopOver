using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TrapExplosif : MonoBehaviour
{

    public float power = 10.0f;
    public float radius = 10.0f;
    public SpanwerPlayer Player;
    public KeyCode key2Reload;
    public GameObject mesh;

    private SphereCollider col;


    private void Start()
    {
        col = GetComponent<SphereCollider>();
    }
    public void OnCollisionEnter (Collision other)
    {

        if (other.gameObject.tag == "Player")
        {
           if (Player.playersSpeed > 10.0f)
           {
              Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
                foreach (Collider hit in colliders) 
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce( power, transform.position, radius, 3.0f);
                        col.enabled = false;
                        mesh.SetActive(false);
                    }
                }
           }
          
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(key2Reload))
        {
            col.enabled = true;
            mesh.SetActive(true);
        }
    }
}
