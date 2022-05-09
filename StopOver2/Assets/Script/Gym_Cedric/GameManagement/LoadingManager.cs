using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Image loadingBar;
    private WaitForEndOfFrame wait = new WaitForEndOfFrame();



    private void OnEnable()
    {
        StartCoroutine(WaitToLoad());
    }

    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(2);

        while (gameLevel.progress < 1)
        {
            loadingBar.fillAmount = gameLevel.progress;
            yield return wait;
        }
    }
}
