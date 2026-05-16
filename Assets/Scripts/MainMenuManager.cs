using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Button Clicked!");
        Application.Quit();
    }
}