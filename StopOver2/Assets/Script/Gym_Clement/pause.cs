using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pause : MonoBehaviour
{
    private LoadingManager loadingManager;
    private FMOD_FCManager musicManager;
    private Animator anim;

    [Header("Parameters")]
    [SerializeField]private KeyCode[] keyPause;
    [SerializeField]private GameObject MenuPause;
    [SerializeField]private Button firstButton;
    [SerializeField]private GameObject leaderboard;
    [SerializeField]private GameObject option;
    [SerializeField]private GameObject loadingScreen;
    private bool b = false;



    private void Start()
    {
        musicManager = FindObjectOfType<FMOD_FCManager>();
        anim = GetComponent<Animator>();
        loadingManager = GetComponent<LoadingManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyPause[0]) || Input.GetKeyDown(keyPause[1]))
        {
            Time.timeScale = 1;
            anim.SetTrigger("Input Menu");
        }
    }

    public void OpenMenus(GameObject g)
    {
        g.SetActive(true);
    }

    public void CloseMenu(GameObject g)
    {
        g.SetActive(false);
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void resetTime()
    {
        Time.timeScale = 1;
    }

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
        MenuPause.SetActive(true);
        firstButton.Select();
    }

    public void DisactivePanel()
    {
        MenuPause.SetActive(false);
        CloseMenu(leaderboard);
        CloseMenu(option);
    }
}
