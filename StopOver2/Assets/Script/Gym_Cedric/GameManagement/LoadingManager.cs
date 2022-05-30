using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private TextMeshProUGUI doingText;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingProgressText;



    public void LunchSceneLoad()
    {
        StartCoroutine(LoadAsyncScene());
    }


    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(1);

        while (!gameLevel.isDone)
        {
            float progress = Mathf.Clamp01(gameLevel.progress / 0.9f);

            loadingBar.fillAmount = progress;
            loadingProgressText.text = Mathf.Round(progress * 100f) + "%";

            doingText.text = (gameLevel.progress > 0.9f) ? "Initiating" : "Loading";

            yield return null;
        }
    }
}
