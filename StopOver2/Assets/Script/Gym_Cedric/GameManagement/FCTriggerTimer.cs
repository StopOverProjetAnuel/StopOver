using UnityEngine;

public class FCTriggerTimer : MonoBehaviour
{
    private FMOD_SoundCaller soundCaller;
    private GMTimer _GMTimer;
    [SerializeField] private Animator cinematicAnimator;

    [SerializeField] private bool triggerAtEnter = true;
    [SerializeField] private Color debugBoxColliderColor = Color.red;



    private void Awake()
    {
        _GMTimer = FindObjectOfType<GMTimer>();
        soundCaller = TryGetComponent<FMOD_SoundCaller>(out FMOD_SoundCaller sound) ? soundCaller = sound : null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_GMTimer && other.tag == "Player")
        {
            if (triggerAtEnter)
            {
                _GMTimer.playTimer = true;
                if (soundCaller) soundCaller.SoundStart();
            }
            else
            {
                _GMTimer.playTimer = false;

                if (cinematicAnimator)
                {
                    cinematicAnimator.enabled = true;
                    cinematicAnimator.Play("end course");
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = debugBoxColliderColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);

        Gizmos.color = debugBoxColliderColor * new Color(1, 1, 1, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}