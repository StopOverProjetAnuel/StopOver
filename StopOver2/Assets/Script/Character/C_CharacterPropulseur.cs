using UnityEngine;

public class C_CharacterPropulseur : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] Vector2 zRotMinMax = new Vector2(-30f, 30f);
    [SerializeField] Vector2 xRotMinMax = new Vector2(-30f, 30f);

    public GameObject[] arrayPropulseurPointRight = new GameObject[0];
    public GameObject[] arrayPropulseurPointLeft = new GameObject[0];

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

    [Header("Thrusters Force")]
    public float minThrustersForce = 5f;
    public float maxThrustersForce = 20f;
    public AnimationCurve thrustersForceMultiplier;
    public float thrustersForceSpeedRequire = 30f;
    private float currentTimeTransitionLean;

    private float lastHitDist;



    public void InitiatePropulsorValue(Rigidbody PlayerRb)
    {
        rb = PlayerRb;

        currentTimeTransitionLean = 0;
        currentLenghtRight = length;
        currentLenghtLeft = length;
        currentDampening = 0;
        currentStrengthRight = strengthRight;
        currentStrengthLeft = strengthLeft;

        lastHitDist = 2;
    }

    public void Propulsing(LayerMask mask, float speed)
    {
        CheckPropulsors(arrayPropulseurPointLeft, currentLenghtLeft, currentStrengthLeft, ref lastHitDist, mask, speed);
        CheckPropulsors(arrayPropulseurPointRight, currentLenghtRight, currentStrengthRight, ref lastHitDist, mask, speed);
        //Debug.Log("propuls call");
    }

    void CheckPropulsors(GameObject[] propulsors, float currentLength, float currentStrength, ref float lastHitDist, LayerMask mask, float speed)
    {
        //Debug.Log("Current Length : " + currentLength);
        //Debug.Log("Current Str : " + currentStrength);
        foreach (GameObject propulsPoint in propulsors)
        {
            //Debug.Log("Call propulsor");
            RaycastHit hit;
            Vector3 rayDirection = propulsPoint.transform.position - Vector3.up;
            if (Physics.Raycast(propulsPoint.transform.position, propulsPoint.transform.up * -1f, out hit, currentLength, mask.value))
            {
                lastHitDist = hit.distance;

                float lengthRatio = Mathf.Clamp(hit.distance / length, 0, 1);
                float forceAmount = thrustersForceMultiplier.Evaluate(lengthRatio);

                float forceMultiplier = Mathf.Clamp(rb.velocity.magnitude / thrustersForceSpeedRequire, 0, 1);
                float currentThrustersForce = Mathf.Lerp(minThrustersForce, maxThrustersForce, forceMultiplier);

                rb.AddForceAtPosition(transform.up * forceAmount * currentThrustersForce, propulsPoint.transform.position);
            }
            else
            {
                //Debug.Log("Dont Hit Ground");
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(arrayPropulseurPointLeft[0].transform.position, arrayPropulseurPointLeft[0].transform.position - (Vector3.up * 4));
        Gizmos.DrawLine(arrayPropulseurPointRight[0].transform.position, arrayPropulseurPointRight[0].transform.position - (Vector3.up * 4));
    }
}