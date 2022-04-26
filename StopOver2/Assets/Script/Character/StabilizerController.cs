using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizerController : MonoBehaviour
{
    [Header("Stabilizer Parameters")]
    [SerializeField] private float raycastMaxDistance = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float stabilizerForce = 10f;
    [SerializeField] private Vector3 stabilizerDir = Vector3.up;

    [Header("Parameters Require")]
    [SerializeField] private Transform objectToStabilize;
    private Rigidbody rb;



    public void StabilizerGetProperties(Rigidbody pRb)
    {
        rb = pRb;
    }

    public void StabilizerCallEvents()
    {
        if (IsTouchGround()) StabilizeObject();
    }

    private bool IsTouchGround()
    {
        bool touchGround = Physics.Raycast(transform.localPosition, transform.localPosition + Vector3.forward, raycastMaxDistance, groundLayer);
        return touchGround;
    }

    private void StabilizeObject()
    {
        Vector3 dirForce = stabilizerDir * stabilizerForce;
        rb.AddRelativeForce(dirForce, ForceMode.Acceleration);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.localPosition, transform.localPosition + Vector3.forward);
    }
}
