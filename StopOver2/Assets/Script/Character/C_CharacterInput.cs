using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CharacterInput : C_CharacterManager
{

    [Header("Input")]
    [Space]
    public float horizontalInput;
    public float verticalInput;

    public string boostInputName = "Boost";

    public Transform cameraTranform;

    public Quaternion cameraDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        cameraDirection = Quaternion.AngleAxis(cameraTranform.rotation.eulerAngles.y, Vector3.up);
        cameraDirection.x = transform.localRotation.x;
        cameraDirection.z = transform.localRotation.z;
    }
}
