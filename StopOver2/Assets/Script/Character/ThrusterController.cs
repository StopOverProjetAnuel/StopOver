using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    //Requirement//
    private Rigidbody rb;

    //Thruster Parameters//
    private float minThrustersForce = 2.5f;
    private float maxThrustersForce = 50f;
    private AnimationCurve ThrustersForceCurve; //Change the current force from the min to max thruster force by the height of the entity compare to the ground

    //Ground Check Parameters//
    private float floatingHeight = 5f;
    private LayerMask floatingMask;


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
    public void ThrusterCallEvents(float minForce, float maxForce, AnimationCurve forceCurve, float height)
    {

    }

    private bool SphereGroundCheck()
    {
        return true;
    }
}
