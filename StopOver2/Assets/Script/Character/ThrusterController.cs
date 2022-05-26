using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    //Requirement//
    private Rigidbody rb;

    //Thruster Parameters//
    private float minThrustersForce;
    private float maxThrustersForce;
    private float thrustersForceMultiplier;
    private AnimationCurve ThrustersForceCurve; //Change the current force from the min to max thruster force by the height of the entity compare to the ground

    //Ground Check Parameters//
    private float floatingHeight;
    private LayerMask floatingMask;
    private RaycastHit hit;
    private bool isGrounded;

    [Header("Debug")]
    [SerializeField] private bool showDebug = false;



    /// <summary>
    /// Give the properties of this simulator script
    /// </summary>
    /// <param name="rigidbody"> Require a rigidbody to simulate the floating physics </param>
    /// <param name="mask"> Determine what will be detect by the thruster range, and activate it </param>
    public void ThrusterGetProperties(Rigidbody rigidbody, LayerMask mask)
    {
        rb = rigidbody;
        floatingMask = mask;
    }

    /// <summary>
    /// Call all events to simulate a thruster force
    /// </summary>
    /// <param name="minForce"> Minimal thruster force </param>
    /// <param name="maxForce"> Maximal thruster force </param>
    /// <param name="forceCurve"> Use a curve to smooth between the min and max thruster force by using the floating height </param>
    /// <param name="height"> The floating height </param>
    public void ThrusterCallEvents(float minForce, float maxForce, float forceMultiplier, AnimationCurve forceCurve, float height)
    {
        minThrustersForce = minForce;
        maxThrustersForce = maxForce;
        thrustersForceMultiplier = forceMultiplier;
        ThrustersForceCurve = forceCurve;
        floatingHeight = height;

        SphereGroundCheck();
        SimulateFloating();
    }

    /// <summary>
    /// Check the ground inside a sphere radius by a layer
    /// </summary>
    /// <returns> will return true if it touch an object with the correct layer, and a hit </returns>
    private void SphereGroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, floatingHeight, floatingMask);
        if (showDebug)
        {
            Debug.Log("dir : " + -transform.up);
        }
    }

    /// <summary>
    /// Function that simulate a floating physics
    /// </summary>
    private void SimulateFloating()
    {
        float currentThrusterForce = minThrustersForce;

        if (!isGrounded) currentThrusterForce = 1f;
        else
        {
            float heightRatio = Mathf.Clamp(hit.distance / floatingHeight, 0f, 1f);
            float smoothHeightRatio = ThrustersForceCurve.Evaluate(heightRatio);
            currentThrusterForce = Mathf.Lerp(maxThrustersForce, minThrustersForce, smoothHeightRatio);
            if (showDebug)
            {
                Debug.Log("hit Distance : " + hit.distance);
            }
        }

        rb.AddForceAtPosition(currentThrusterForce * transform.up, transform.position, ForceMode.Acceleration);
        if (showDebug)
        {
            Debug.Log("touch ground : " + isGrounded);
            Debug.Log("thruster Force : " + currentThrusterForce);
        }
    }




    private void OnDrawGizmos()
    {
        //if (!showDebug) return; 

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * floatingHeight);
    }
}
