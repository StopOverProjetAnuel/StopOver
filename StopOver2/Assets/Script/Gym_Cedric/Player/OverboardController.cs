using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverboardController : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tf;
    public LayerMask myLM;
    public Vector3 groundedCheckBox = new Vector3(1, 0.5f, 1);
    public float movementSpeed = 500f;
    public float sprintSpeed = 1000f;
    private float currentSpeed;
    public float rotateSpeed = 720f;
    public float jumpForce = 10f;
    public int airJumpCount = 1;
    private int jumpLeft;
    public GameObject mainBodyModel;
    public float inclDegrees = 5f;
    public Vector3 gravityForceFall;
    private Vector3 gravityReset = new Vector3(0, -9.81f, 0);

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        jumpLeft = airJumpCount;
        currentSpeed = movementSpeed;
    }

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
        else if (Input.GetButton("Horizontal"))
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



    private void Update()
    {
        Sprint();
        MyJump();
        ResetAirJump();
        GravityFallAcc();
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

    private void MyJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
        else if (Input.GetButtonDown("Jump") && jumpLeft != 0)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            jumpLeft -= 1;
        }
    }

    private bool IsGrounded()
    {
        bool ig = Physics.CheckBox(tf.position, groundedCheckBox, Quaternion.identity, myLM);
        //Debug.Log("IsGrounded = " + ig);
        return ig;
    }

    private void ResetAirJump()
    {
        if (IsGrounded())
        {
            jumpLeft = airJumpCount;
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
