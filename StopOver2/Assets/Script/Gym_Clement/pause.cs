using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class pause : MonoBehaviour
{
    private Animator anim;
    [SerializeField]private KeyCode k;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(k))
        {
            anim.SetTrigger("Input Menu");
        }
    }

    public void StopTime()
    {
        Time.timeScale = 1;
    }
    public void resetTime()
    {
        Time.timeScale = 1;
    }
}
