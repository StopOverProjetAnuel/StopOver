using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverboardController : MonoBehaviour
{
    #region Variables
    [HideInInspector] public Rigidbody rb;

    #region Movement Variables
    [Header("Movement Parameters")]
    private float currentSpeed;
    public float movementSpeed;
    public float sprintSpeed;
    public float rotateSpeed;

    [Header("Fall Parameters")]
    public Vector3 gravityForceFall;
    private Vector3 gravityReset = new Vector3(0, -9.81f, 0);
    #endregion

    #region Recolt Variables
    [HideInInspector] public int nbState;
    #endregion

    #region Animation Variables
    [Header("Vehicle procedural animation")]
    public GameObject mainBodyModel;
    public float inclDegrees;
    #endregion

    #region Camera Effects
    private CameraEffects cameraEffects;
    public float flashDuration = 0.1f;
    private bool isFlashUsed = false;
    #endregion
    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = movementSpeed;

        cameraEffects = FindObjectOfType<CameraEffects>();
    }

    private void FixedUpdate()
    {
        MyMovement();
        MyRotate();
    }

    #region Movement
    private void MyMovement()
    {
        if (Input.GetButton("Vertical"))
        {
            rb.AddRelativeForce(0, 0, Input.GetAxis("Vertical") * currentSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        if (Input.GetButton("Horizontal"))
        {
            rb.AddRelativeForce(Input.GetAxis("Horizontal") * currentSpeed * Time.fixedDeltaTime, 0, 0, ForceMode.Acceleration);
        }

        Transform mBM = mainBodyModel.transform;
        mBM.localRotation = Quaternion.Euler(inclDegrees * Input.GetAxis("Vertical"), 0, inclDegrees * (-1 * Input.GetAxis("Horizontal")));
    }

    private void MyRotate()
    {
        rb.AddRelativeTorque(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.fixedDeltaTime, 0, ForceMode.Acceleration);
    }
    #endregion

    private void Update()
    {
        Sprint();
        GravityFallAcc();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            cameraEffects.TriggerFlashSpeed(true);
        }
        else if (currentSpeed != movementSpeed)
        {
            currentSpeed = movementSpeed;
            cameraEffects.isActiveTriggered = false;
        }
    }

    private void GravityFallAcc()
    {
        if (rb.velocity.y <= -1f)
        {
            Physics.gravity = gravityForceFall;
            rb.drag = 0.1f;
        }
        else
        {
            Physics.gravity = gravityReset;
            rb.drag = 1f;
        }
    }
}