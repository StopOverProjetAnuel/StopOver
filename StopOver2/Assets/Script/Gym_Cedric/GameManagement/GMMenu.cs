using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GMMenu : MonoBehaviour
{
    private FMOD_FCManager musicManager;

    [Header("Pause & Option Menu")]
    [SerializeField] GameObject objectMenu;
    private static bool gameIsPaused = false;
    private static bool objectOptionMenuIsOpen = false;
    [SerializeField] GameObject objectOptionMenu;
    [SerializeField] GameObject[] objectOptionMenuPanels;

    [Header("Victory/Defeat Menu")]
    [SerializeField] GameObject EndMenu;
    [SerializeField] TextMeshProUGUI EndMenuTitle;
    [SerializeField] string victoryText = "You Won !";
    [SerializeField] string defeatText = "You Lose ;c";
    [SerializeField] GameObject EndMenuButton;
    [SerializeField] GameObject EndMenuSave;
    [SerializeField] GameObject EndMenuLeaderboard;

    //Reload Scene
    Transform startTransformPlayer;
    List<GameObject> disableObjects = new List<GameObject>();


    public void InitiateGMMenu()
    {
        ResumeGame();
        musicManager = FindObjectOfType<FMOD_FCManager>();
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
                musicManager.pause = 1;
            }
            else
            {
                ResumeGame();
                musicManager.pause = 0;
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

        SwapMouseVisible(false);
        CloseOptionMenu();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        objectMenu.SetActive(true);
        SwapMouseVisible(true);
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
        musicManager.StopMusic();
    }

    public void Retry()
    {
        musicManager.StopMusic();
        SwapMouseVisible(false);
        Scene currentActiveScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentActiveScene.name);
    }

    private void SwapMouseVisible(bool isVisible)
    {
        Cursor.visible = isVisible;
    }


    #region End Menu
    public void OpenEndMenu(bool isWin)
    {
        EndMenu.SetActive(true);
        if (isWin)
        {
            EndMenuTitle.text = victoryText;
            SwapMouseVisible(true);
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

    public void OpenEndMenuButton()
    {
        EndMenuButton.SetActive(true);
        SwapMouseVisible(true);
    }

    public void TriggerEndMenuSave()
    {
        if (!EndMenuSave.activeSelf)
        {
            EndMenuSave.SetActive(true);
            SwapMouseVisible(true);
        }
        else
        {
            EndMenuSave.SetActive(false);
        }
    }

    public void TriggerEndMenuLeaderboard()
    {
        if (!EndMenuLeaderboard.activeSelf)
        {
            EndMenuLeaderboard.SetActive(true);
            SwapMouseVisible(true);
        }
        else
        {
            EndMenuLeaderboard.SetActive(false);
        }
    }
    #endregion
}