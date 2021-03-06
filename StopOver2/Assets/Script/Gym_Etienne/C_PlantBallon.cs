using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_PlantBallon : MonoBehaviour
{
    [SerializeField] private FMOD_SoundCaller soundCaller;

    public float speedUpBallon;
    public float timeBefforDestruc;

    public GameObject tige;
    public GameObject ballon;

    private bool ballonGoUp;
    private Rigidbody rbBallon;
    // Start is called before the first frame update
    void Start()
    {
        rbBallon = this.GetComponent<Rigidbody>();
        FMOD_SoundCaller soundCall;
        soundCaller = (TryGetComponent<FMOD_SoundCaller>(out soundCall)) ? soundCaller = soundCall : null;
    }

    private void Update()
    {
        if (ballonGoUp)
        {
            rbBallon.AddForce(new Vector3(0, Mathf.SmoothStep(0, speedUpBallon, 3), 0));
            
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            soundCaller.SoundStart();
            Destroy(tige);
            StartCoroutine(BallonGoUp());

        }
    }

    IEnumerator BallonGoUp()
    {
        ballonGoUp = true;
        yield return new WaitForSeconds(timeBefforDestruc);
        ballonGoUp = false;
        Destroy(this.gameObject);
    }


}
