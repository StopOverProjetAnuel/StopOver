using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blendtreeExemple : MonoBehaviour
{
    [Range(0.0f , 1.0f)]
    public float speed;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", speed);
    }
}
