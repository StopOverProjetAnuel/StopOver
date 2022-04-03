using UnityEngine;
using Cinemachine;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private GMVictoryChecker _GMVictoryChecker;
    [SerializeField] private GameObject player;
    private CameraGetFocus _CameraGetFocus;

    private Animator cinematicAnimator;
    [SerializeField] private AnimationClip startCinematic;
    [SerializeField] private AnimationClip endCinematic;

    private int skipCount = 0;
    private bool isSkip = false;



    private void Awake()
    {
        cinematicAnimator = GetComponent<Animator>();
        _CameraGetFocus = FindObjectOfType<CameraGetFocus>();
    }

    private void Start()
    {
        cinematicAnimator.Play(startCinematic.name);
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



    private void ClearFocusCamera()
    {
        _CameraGetFocus.UnFocusCamera();
    }

    private void GiveFocusCamera()
    {
        _CameraGetFocus.GetFocusCamera();
    }
}
