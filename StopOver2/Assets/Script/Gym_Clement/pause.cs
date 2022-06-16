using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class pause : MonoBehaviour
{
    private GMScoring _GMScoring;
    private LoadingManager loadingManager;
    private FMOD_FCManager musicManager;
    private Animator anim;
    private DifficultySet difficultySet;

    [Header("Global Parameters")]
    [SerializeField] private KeyCode[] keyPause;
    [SerializeField] private GameObject MenuPause;
    [SerializeField] private Button firstButton;
    [SerializeField] private GameObject leaderboard;
    [SerializeField] private GameObject option;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private CinemachineVirtualCamera Cmvcam;

    [Header("Victory And Defeat Parameters")]
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private GameObject victoryNoRecordScreen;
    [SerializeField] private TextMeshProUGUI victoryNoRecordScoreText;
    [SerializeField] private GameObject victoryNewRecordScreen;
    [SerializeField] private TextMeshProUGUI victoryNewRecordScoreText;
    [SerializeField] private GameObject[] leaderboardDifficulty;




    private void Start()
    {
        _GMScoring = FindObjectOfType<GMScoring>();
        musicManager = FindObjectOfType<FMOD_FCManager>();
        anim = GetComponent<Animator>();
        loadingManager = GetComponent<LoadingManager>();
        difficultySet = FindObjectOfType<DifficultySet>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyPause[0]) || Input.GetKeyDown(keyPause[1]))
        {
            Time.timeScale = 1;
            anim.SetTrigger("Input Menu");
        }
    }

    #region Menus
    public void OpenMenus(GameObject g)
    {
        g.SetActive(true);
    }

    public void CloseMenu(GameObject g)
    {
        g.SetActive(false);
    }
    #endregion

    #region TimeScale
    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void resetTime()
    {
        Time.timeScale = 1;
    }
    #endregion

    #region Buttons
    public void RetryButton()
    {
        Time.timeScale = 1;
        loadingScreen.SetActive(true);
        musicManager.StopMusic();
        loadingManager.LunchSceneLoad(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        musicManager.StopMusic();
        loadingManager.LunchSceneLoad(0);
    }

    public void ActivePanel()
    {
        Cmvcam.m_Lens.FieldOfView = 60f;
        MenuPause.SetActive(true);
        firstButton.Select();
    }

    public void DisactivePanel()
    {
        MenuPause.SetActive(false);
        CloseMenu(leaderboard);
        CloseMenu(option);
    }
    public void ApplicationQuit()
    {
        Application.Quit();
    }
    #endregion

    public void TriggerAnimMenu()
    {
        anim.SetTrigger("Input Menu");
    }

    #region Victory And Defeat
    public void TriggerVictory()
    {
        _GMScoring.CalculateFinalScore();
    }

    public void TriggerNoRecord(string currentScore)
    {
        victoryNoRecordScreen.SetActive(true);
        victoryNoRecordScoreText.text = currentScore;
    }

    public void TriggerNewRecord(string currentScore)
    {
        victoryNewRecordScreen.SetActive(true);
        victoryNewRecordScoreText.text = currentScore;

        TriggerLeaderboardDifficulty(difficultySet.ReturnDifficulty());
    }

    /// <summary>
    /// Trigger the visual diffiuclty leaderboard
    /// </summary>
    /// <param name="pos"> 0 = easy | 1 = normal | 2 = hard | 3 = impossible </param>
    private void TriggerLeaderboardDifficulty(int pos)
    {
        leaderboardDifficulty[pos].SetActive(true);
    }

    public void TriggerDefeat()
    {
        defeatScreen.SetActive(true);
    }
    #endregion
}
