using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CameraManager : MonoBehaviour
{

    public Transform cameraTranform;
    public Transform target;
    public Quaternion cameraDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*cameraDirection = Quaternion.AngleAxis(cameraTranform.rotation.eulerAngles.y, Vector3.up);
        cameraDirection.x = transform.localRotation.x;
        cameraDirection.z = transform.localRotation.z;

        cameraTranform.rotation = cameraDirection;*/


        cameraTranform.LookAt(target);
    }
}
