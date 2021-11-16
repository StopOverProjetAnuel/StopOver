using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterInput : C_CharacterManager
{

    [Header("Input")]
    [Space]
    public float horizontalInput;
    public float verticalInput;
    public float mouseXInput;

    public string boostInputName = "Boost";



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseXInput = Input.GetAxis("Mouse X");


    }
}
