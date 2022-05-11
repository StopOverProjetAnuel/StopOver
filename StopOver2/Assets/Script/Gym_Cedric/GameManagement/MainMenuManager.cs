using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Requirement")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject[] allButton;



    private void Update()
    {
        SelectButton();
    }

    private void SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(allButton[0]);
    }

    public void LoadScene()
    {
        loadingScreen.SetActive(true);
    }
}
