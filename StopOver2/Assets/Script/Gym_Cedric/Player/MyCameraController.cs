using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
    public Transform myCam;
    public Vector3 OffsetCam;
    public Transform myPlayer;
    private Vector3 myVelocity = Vector3.zero;
    public float smoothTimeCam = 0.1f;



    private void Awake()
    {
        myCam.position = OffsetCam;
    }



    private void FixedUpdate()
    {
        CameraFollow();
    }

    private void CameraFollow()
    {
        transform.position = Vector3.SmoothDamp(transform.position, myPlayer.position, ref myVelocity, smoothTimeCam);
    }
}