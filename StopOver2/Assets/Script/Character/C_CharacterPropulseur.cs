using UnityEngine;

public class C_CharacterPropulseur : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] Vector2 zRotMinMax = new Vector2(-30f, 30f);
    [SerializeField] Vector2 xRotMinMax = new Vector2(-30f, 30f);

    public GameObject[] arrayPropulseurPointRight;
    public GameObject[] arrayPropulseurPointLeft;

    public float length;
    public float strengthRight;
    public float strengthLeft;

    public float multiStrength;
    public float divisionStrength;
    public float multiLenght;

    public float timeTransitionLean;

    public float dampening;

    private float lastHitDistRight;
    private float currentStrengthRight;

    private float lastHitDistLeft;
    private float currentStrengthLeft;

    private float currentDampening;
    private float currentLenghtRight;
    private float currentLenghtLeft;


    private float t1;
    private float currentTimeTransitionLean;


    public void InitiatePropulsorValue(Rigidbody PlayerRb)
    {
        rb = PlayerRb;

        currentTimeTransitionLean = 0;
        t1 = 0;
        currentLenghtRight = length;
        currentLenghtLeft = length;
        currentDampening = 0;
        currentStrengthRight = strengthRight;
        currentStrengthLeft = strengthLeft;
    }

    public void Propulsing()
    {
        CheckPropulsors(arrayPropulseurPointLeft, currentLenghtLeft, currentStrengthLeft, ref lastHitDistLeft);
        CheckPropulsors(arrayPropulseurPointRight, currentLenghtRight, currentStrengthRight, ref lastHitDistRight);
    }

    void CheckPropulsors(GameObject[] propulsors, float currentLength, float currentStrength, ref float lastHitDist)
    {
        foreach (GameObject propulsPoint in propulsors)
        {
            RaycastHit hit;
            if (Physics.Raycast(propulsPoint.transform.position, propulsPoint.transform.TransformDirection(-Vector3.up), out hit, currentLength))
            {

                lastHitDist = hit.distance;
                float forceAmount = 0;
                float lengthRatio = (currentLength - hit.distance) / currentLength;

                forceAmount = currentStrength * lengthRatio + (currentDampening * (lastHitDist * hit.distance));

                rb.AddForceAtPosition(transform.up * forceAmount * rb.mass, propulsPoint.transform.position);
            }
            else
            {
                lastHitDist = currentLength;
            }

        }
    }

    void LockRotation()
    {
       float currentZRotation = transform.localEulerAngles.z;

        currentZRotation = Mathf.Clamp(currentZRotation, zRotMinMax.x, zRotMinMax.y);

       float currentXRotation = transform.localEulerAngles.x;
       currentXRotation = Mathf.Clamp(currentXRotation, xRotMinMax.x, xRotMinMax.y);

        Vector3 newRotation = new Vector3
            (
                currentXRotation,
                transform.localEulerAngles.y,
                currentZRotation
            );
        transform.localEulerAngles = newRotation;
    }
}
