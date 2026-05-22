using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameSceneManager.Instance.LoadScene("Gameplay");
    }
}