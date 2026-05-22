using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [Header("Transition")]
    public Image fadePanel;
    public float fadeDuration = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Pindah scene dengan fade
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        // Fade to black
        yield return StartCoroutine(Fade(0f, 1f));

        // Load scene
        SceneManager.LoadScene(sceneName);

        // Fade in
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color color = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        color.a = to;
        fadePanel.color = color;
    }

    // Dipanggil dari Yarn
    public void GoToEnding()
    {
        LoadScene("Ending");
    }

    public void GoToMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        LoadScene("Gameplay");
    }
}