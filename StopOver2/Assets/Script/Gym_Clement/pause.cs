using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pause : MonoBehaviour
{
    private Animator anim;
    [SerializeField]private KeyCode key;
    [SerializeField]private GameObject MenuPause;
    [SerializeField]private Button firstButton;
    [SerializeField]private GameObject leaderboard;
    [SerializeField]private GameObject option;
    private bool b = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(key))
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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
