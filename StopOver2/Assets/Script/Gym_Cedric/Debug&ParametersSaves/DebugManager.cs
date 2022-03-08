using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private RessourceManager ressourceManager;
    [SerializeField] private float ressourceGiveNRemove = 10f;

    [Header("Teleportation")]
    [SerializeField] private Transform playerPos;
    [SerializeField] private KeyCode startPosInput;
    [SerializeField] private Transform startPos;
    [SerializeField] private KeyCode endPosInput;
    [SerializeField] private Transform endPos;

    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        playerPos = FindObjectOfType<C_CharacterManager>().transform;
    }

    private void Update()
    {
        GiveRessources();
        RemoveRessources();
        StartEndTp();
    }

    #region ResourceManagement
    private void GiveRessources()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ressourceManager.currentRessource += ressourceGiveNRemove;
        }
    }

    private void RemoveRessources()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ressourceManager.currentRessource -= ressourceGiveNRemove;
        }
    }
    #endregion

    private void StartEndTp()
    {
        playerPos.position = Input.GetKeyDown(startPosInput) ? startPos.position : playerPos.position;
        playerPos.position = Input.GetKeyDown(endPosInput) ? endPos.position : playerPos.position;
    }
}
