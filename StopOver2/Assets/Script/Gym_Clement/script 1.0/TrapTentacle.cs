using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTentacle : MonoBehaviour
{
    
     private Animator anim;

     void Start()
     {
     anim = GetComponent<Animator>();

     }
     void OnTriggerEnter(Collider other)
     {
         if (other.CompareTag("Player"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                anim.Play("Base Layer.Deploy");
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("catch"))
            {
                anim.Play("Base Layer.idle");
            }
        }

     }

    public void TouchPlayer()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            anim.Play("Base Layer.catch");
        }
    }


}


// voir la suite du code dans l'objet enfant Cylindre(2)