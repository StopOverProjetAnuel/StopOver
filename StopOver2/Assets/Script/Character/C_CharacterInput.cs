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
    public float boostInput;

    public string boostInputName = "Boost";

    ²                                                               
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseXInput = Input.GetAxis("Mouse X");
        boostInput = Input.GetAxis(boostInputName);


    }
}
