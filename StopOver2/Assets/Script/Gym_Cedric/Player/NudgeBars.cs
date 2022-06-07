using UnityEngine;

public class NudgeBars : MonoBehaviour
{
    private FMOD_FCManager musicManager;
    private C_CharacterSound _CharacterSound;

    [Header("Player Information")]
    [SerializeField] private GameObject m_Player;
    private Rigidbody rb;

    [HideInInspector] public int nbState;

    [Header("Nugde Bars States")]
    public GameObject nudgeBars_model;
    public Material[] nudgeBarsMaterials;
    public float min1stState;
    public float min2ndState;

    //Object Collision
    private CollidedPlayer collidedPlayer;

    [Header("Debug Option")]
    private bool showDebug = false;

    private void Awake()
    {
        rb = m_Player.GetComponent<Rigidbody>();
        musicManager = FindObjectOfType<FMOD_FCManager>();
        _CharacterSound = FindObjectOfType<C_CharacterSound>();
    }

    private void Update()
    {
        NudgeBarsState();
        musicManager.isCollecting = false;
    }

    private void NudgeBarsState()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            //nudgeBars_model.SetActive(true);
            if (rb.velocity.magnitude >= min2ndState)
            {
                nbState = 3;
            }
            else if (rb.velocity.magnitude >= min1stState)
            {
                nbState = 2;
            }
            else
            {
                nbState = 1;
            }

            //nudgeBars_model.GetComponent<MeshRenderer>().material = nudgeBarsMaterials[nbState - 1];
        }
        else
        {
            //nudgeBars_model.SetActive(false);
            nbState = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp nanRef;
        bool isPickUp = other.TryGetComponent<PickUp>(out nanRef);

        Transform parent = other.transform.parent;
        if (parent.TryGetComponent<CollidedPlayer>(out collidedPlayer))
        {
            if (parent.CompareTag("Destructible"))
            {
                collidedPlayer.TriggerCollisionPlayer(rb);
                musicManager.isCollecting = true;
                if (isPickUp) _CharacterSound.GetBiomassTrigger();
                #region Debug
                if (showDebug)
                {
                    Debug.Log("NudgeBars get triggered with a destructible object");
                }
                #endregion
            }
        }
        else
        {
            if (other.CompareTag("Destructible"))
            {
                collidedPlayer = other.GetComponent<CollidedPlayer>();
                collidedPlayer.TriggerCollisionPlayer(rb);
                musicManager.isCollecting = true;
                if (isPickUp) _CharacterSound.GetBiomassTrigger();
                #region Debug
                if (showDebug)
                {
                    Debug.Log("NudgeBars get triggered with a destructible object");
                }
                #endregion
            }
        }
        #region Debug
        if (showDebug)
        {
            Debug.Log("NudgeBars get triggered");
        }
        #endregion
    }
}
