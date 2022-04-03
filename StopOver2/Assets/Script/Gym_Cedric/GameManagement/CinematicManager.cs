using UnityEngine;
using Cinemachine;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private GMVictoryChecker _GMVictoryChecker;
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineVirtualCamera fcMainCamera;

    private Animator cinematicAnimator;

    private int skipCount = 0;
    private bool isSkip = false;
    [SerializeField] private Transform camLookAt;
    [SerializeField] private Transform camFollow;



    private void Awake()
    {
        cinematicAnimator = GetComponent<Animator>();
        cinematicAnimator.SetBool("StartCinematic", true);
    }

    private void Update()
    {
        SkipPointAdd();
        SkipCinematic();
    }

    private void SkipPointAdd()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            skipCount += 1;
        }
    }

    private void SkipCinematic()
    {
        if (skipCount >= 2 && !isSkip)
        {
            cinematicAnimator.SetTrigger("SkipStartCinematic");
            isSkip = true;
        }
    }



    private void Victory()
    {
        EnableAnimator();
        _GMVictoryChecker.TriggerVictory();
    }

    private void StartGame()
    {
        cinematicAnimator.SetBool("StartCinematic", false);
        player.GetComponent<Rigidbody>().isKinematic = false;
        DisableAnimator();
    }

    private void DisableAnimator()
    {
        cinematicAnimator.enabled = false;
    }

    private void EnableAnimator()
    {
        cinematicAnimator.enabled = true;
    }



    private void GiveCameraFocus()
    {
        fcMainCamera.LookAt = camLookAt;
        fcMainCamera.Follow = camFollow;
    }
}
