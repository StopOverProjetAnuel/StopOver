using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;

    public void LoadScene()
    {
        loadingScreen.SetActive(true);
    }
}
