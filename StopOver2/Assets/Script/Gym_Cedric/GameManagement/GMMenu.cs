using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GMMenu : MonoBehaviour
{
    [Header("Pause & Option Menu")]
    private static bool gameIsPaused = false;
    public GameObject objectMenu;
    private static bool objectOptionMenuIsOpen = false;
    public GameObject objectOptionMenu;
    public GameObject[] objectOptionMenuPanels;

    [Header("Victory/Defeat Menu")]
    public GameObject EndMenu;
    public TextMeshProUGUI EndMenuTitle;
    public string victoryText = "You Won !";
    public string defeatText = "You Lose ;c";

    //Reload Scene
    private Transform startTransformPlayer;
    private List<GameObject> disableObjects = new List<GameObject>();


    public void InitiateGMMenu()
    {
        ResumeGame();
    }

    public void TriggerGMMenu()
    {
        if (objectMenu != null)
        {
            TriggerMenu();
        }
    }

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
        TriggerOptionMenuPanels(0);
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
        Application.Quit();
    }

    public void Retry()
    {
        Scene currentActiveScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentActiveScene.name);
    }

    public void OpenEndMenu(bool isWin)
    {
        EndMenu.SetActive(true);
        if (isWin)
        {
            EndMenuTitle.text = victoryText;
        }
        else
        {
            EndMenuTitle.text = defeatText;
        }
    }

    public void CloseEndMenu()
    {
        EndMenu.SetActive(false);
    }
}