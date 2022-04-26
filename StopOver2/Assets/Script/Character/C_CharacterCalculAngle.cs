using UnityEngine;

public class C_CharacterCalculAngle : MonoBehaviour
{
    [SerializeField] private StabilizerController[] stabilizers;

    public void calculAngleGetProperties(Rigidbody rb)
    {
        foreach (StabilizerController stabilizer in stabilizers)
        {
            stabilizer.StabilizerGetProperties(rb);
        }
    }

    public void calculAngleCallEvents()
    {
        foreach (StabilizerController stabilizer in stabilizers)
        {
            stabilizer.StabilizerCallEvents();
        }
    }
}
