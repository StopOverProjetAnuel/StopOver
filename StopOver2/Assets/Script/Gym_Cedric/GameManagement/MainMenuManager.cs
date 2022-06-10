using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject SplashScreen;
    [SerializeField] private GameObject MainScreen;

    public void EnableMenu(GameObject g)
    {
        g.SetActive(true);
    }

    public void DisableMenu(GameObject g)
    {
        g.SetActive(false);
    }

    public void DisableSplashScreen()
    {
        SplashScreen.SetActive(false);
    }
    public void EnableMainScreen()
    {
        MainScreen.SetActive(true);
    }
    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
