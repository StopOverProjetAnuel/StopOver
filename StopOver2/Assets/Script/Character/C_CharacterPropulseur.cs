using UnityEngine;

public class C_CharacterPropulseur : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private ThrusterController[] thrusters = new ThrusterController[0];

    [Header("Thruster Parameters")]
    [SerializeField] private float minThrustersForce = 2.5f;
    [SerializeField] private float maxThrustersForce = 50f;
    [Tooltip("Change the current force from the min to max thruster force by the height of the entity compare to the ground")]
    [SerializeField] private AnimationCurve ThrustersForceCurve;

    [Header("Ground Check Parameters")]
    [SerializeField] private float floatingHeight = 5f;
    [SerializeField] private LayerMask floatingMask;



    public void InitiatePropulsorValue(Rigidbody PlayerRb)
    {
        foreach (ThrusterController thruster in thrusters)
        {
            //thruster.ThrusterGetProperties(PlayerRb, floatingMask);
        }
    }

    public void Propulsing()
    {
        float boostPrevension = (Input.GetButton("Boost") && Input.GetButton("Boost2")) ? 10 : 1;

        foreach (ThrusterController thruster in thrusters)
        {
            //thruster.ThrusterCallEvents(minThrustersForce, maxThrustersForce * boostPrevension, ThrustersForceCurve, floatingHeight);
        }
    }
}