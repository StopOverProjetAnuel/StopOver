using UnityEngine;

public class C_CharacterAnim : MonoBehaviour
{
    [Header("Player Animation Parameters")]
    [SerializeField] private GameObject characterModel;
    [SerializeField] private float leanAngleZ = 25f;
    [SerializeField] private float leanAngleY = 15f;
    [SerializeField] private float leaningSpeed = 0.1f;

    [Header("Camera Focus Parameters")]
    [SerializeField] private GameObject cameraFocus;
    [SerializeField] private Vector3 offsetCamFocus = new Vector3(0f, 1.5f, 10f);
    [SerializeField] private float cameraMaxOffset = 12f;
    private float smoothMouseMovement = 0f;
    private float refVelocity = 0f;



    public void charaAnimCallEvents(float mouseX)
    {
        GetInput(mouseX);
        LeaningModel();
        OffsetTurnVehicle();
    }

    private void GetInput(float mouseX)
    {
        smoothMouseMovement = Mathf.SmoothDamp(smoothMouseMovement, mouseX, ref refVelocity, leaningSpeed);
    }

    private void LeaningModel()
    {
        float leaningAngleY = smoothMouseMovement * leanAngleY * Time.timeScale;
        float leaningAngleZ = -smoothMouseMovement * leanAngleZ * Time.timeScale;

        Vector3 leaningAngle = new Vector3(0, leaningAngleY, leaningAngleZ);

        characterModel.transform.localRotation = Quaternion.Euler(leaningAngle);
    }

    private void OffsetTurnVehicle()
    {
        float changeOffset = smoothMouseMovement * cameraMaxOffset;
        Vector3 newOffsetPos = Vector3.right * changeOffset + offsetCamFocus;
        cameraFocus.transform.localPosition = newOffsetPos;
    }
}
