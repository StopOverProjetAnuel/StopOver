using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    private RessourceManager ressourceManager;
    public float ressourceGiveNRemove = 10f;

    private static bool gameIsPaused = false;
    public GameObject objectMenu;
    private static bool objectOptionMenuIsOpen = false;
    public GameObject objectOptionMenu;
    public GameObject[] objectOptionMenuPanels;

    private void Awake()
    {
        ressourceManager = FindObjectOfType<RessourceManager>();
        ResumeGame();
    }

    private void Update()
    {
        GiveRessources();
        RemoveRessources();

        if (objectMenu != null)
        {
            TriggerMenu();
        }
    }

    #region Debug Actions
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
    #endregion

    #region UI
    #region Pause Menu Action
    private void TriggerMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;

            if (gameIsPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        objectMenu.SetActive(false);
        if (gameIsPaused)
        {
            gameIsPaused = !gameIsPaused;
        }

        CloseOptionMenu();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        objectMenu.SetActive(true);
    }
    #endregion

    #region Option Menu Action
    public void TriggerOptionMenu()
    {
        objectOptionMenuIsOpen = !objectOptionMenuIsOpen;

        if (objectOptionMenuIsOpen)
        {
            OpenOptionMenu();
        }
        else
        {
            CloseOptionMenu();
            objectMenu.SetActive(true);
        }
    }

    private void OpenOptionMenu()
    {
        objectOptionMenu.SetActive(true);
        objectMenu.SetActive(false);
    }

    private void CloseOptionMenu()
    {
        objectOptionMenu.SetActive(false);
        if (objectOptionMenuIsOpen)
        {
            objectOptionMenuIsOpen = !objectOptionMenuIsOpen;
        }
    }

    public void TriggerOptionMenuPanels(int currentPanel)
    {
        foreach (GameObject panel in objectOptionMenuPanels)
        {
            panel.SetActive(false);
        }

        objectOptionMenuPanels[currentPanel].SetActive(true);
    }
    #endregion

    public void QuitGame()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
    #endregion
}
