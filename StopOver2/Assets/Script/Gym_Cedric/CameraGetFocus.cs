using UnityEngine;
using Cinemachine;

public class CameraGetFocus : MonoBehaviour
{
    private CinemachineVirtualCamera fcMainCamera;

    [SerializeField] private Transform camLookAt;
    [SerializeField] private Transform camFollow;



    private void Start()
    {
        fcMainCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void GetFocusCamera()
    {
        fcMainCamera.LookAt = camLookAt;
        fcMainCamera.Follow = camFollow;
    }

    public void UnFocusCamera()
    {

        fcMainCamera.LookAt = null;
        fcMainCamera.Follow = null;
    }
}
