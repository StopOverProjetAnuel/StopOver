using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverboardController : MonoBehaviour
{
    #region Variables
    private Rigidbody rb;

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

    #region Nudge Bars
    [Header("Nugde Bars States")]
    public GameObject nudgeBars;
    public Material[] nudgeBarsMaterials;
    public float min1stState;
    public float min2ndState;
    #endregion

    #endregion

    #region Animation Variables
    [Header("Vehicle procedural animation")]
    public GameObject mainBodyModel;
    public float inclDegrees;
    #endregion
    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = movementSpeed;
    }

    #region Movement
    private void FixedUpdate()
    {
        MyMovement();
        MyRotate();
    }

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
        NudgeBarsState();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else if (currentSpeed != movementSpeed)
        {
            currentSpeed = movementSpeed;
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

    private void NudgeBarsState()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            nudgeBars.SetActive(true);
            if (rb.velocity.magnitude >= min2ndState)
            {
                nbState = 3;
            }
            else if (rb.velocity.magnitude >= min1stState)
            {
                nbState = 2;
            }
            else
            {
                nbState = 1;
            }

            nudgeBars.GetComponent<MeshRenderer>().material = nudgeBarsMaterials[Mathf.Clamp(nbState - 1, 0, 2)];
        }
        else
        {
            nudgeBars.SetActive(false);
            nbState = 0;
        }
    }
}