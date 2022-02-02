using UnityEngine;

public class FCTriggerTimer : MonoBehaviour
{
    private FCGameManager _FCGameManager;

    [SerializeField] private bool triggerAtEnter = true;
    [SerializeField] private Color debugBoxColliderColor = Color.red;



    private void Awake()
    {
        _FCGameManager = FindObjectOfType<FCGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_FCGameManager && triggerAtEnter && other.tag == "Player")
        {
            _FCGameManager.playTimer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_FCGameManager && !triggerAtEnter && other.tag == "Player")
        {
            _FCGameManager.playTimer = false;
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