using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geyserPrototype : MonoBehaviour
{
    public float verticalBumpStregth = 10.0f;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grow"))
            {
                other.attachedRigidbody.AddForce(Vector3.up * verticalBumpStregth, ForceMode.Impulse);
            }
        }
    }
}
