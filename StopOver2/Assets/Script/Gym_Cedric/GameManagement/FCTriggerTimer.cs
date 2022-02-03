using UnityEngine;

public class FCTriggerTimer : MonoBehaviour
{
    private GMTimer _GMTimer;
    private GMVictoryChecker _GMVictoryChecker;

    [SerializeField] private bool triggerAtEnter = true;
    [SerializeField] private Color debugBoxColliderColor = Color.red;



    private void Awake()
    {
        _GMTimer = FindObjectOfType<GMTimer>();
        _GMVictoryChecker = FindObjectOfType<GMVictoryChecker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_GMTimer && other.tag == "Player")
        {
            if (triggerAtEnter)
            {
                _GMTimer.playTimer = true;
            }
            else
            {
                _GMVictoryChecker.TriggerVictory();
                _GMTimer.playTimer = false;
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