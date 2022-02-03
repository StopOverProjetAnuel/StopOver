using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private RessourceManager ressourceManager;
    public float ressourceGiveNRemove = 10f;

    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
    }

    private void Update()
    {
        GiveRessources();
        RemoveRessources();
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
}
