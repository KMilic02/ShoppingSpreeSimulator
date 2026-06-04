using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_Text timeDisplayText;
    public Button submitButton;
    public GameObject leaderboardPosterDisplay;

    private float finalTime;
    private GameMode currentGameMode;

    public void OpenEndGameUI(float rawTime, GameMode mode)
    {
        finalTime = rawTime;
        currentGameMode = mode;
        gameObject.SetActive(true);

        int minutes = Mathf.FloorToInt(finalTime / 60);
        int seconds = Mathf.FloorToInt(finalTime % 60);
        timeDisplayText.text = $"Final Time: {minutes:0}:{seconds:00}";

        submitButton.onClick.RemoveListener(SubmitScore);
        submitButton.onClick.AddListener(SubmitScore);
    }

    private void SubmitScore()
    {
        Debug.Log("[END GAME UI] Submit score sequence started!");

        string playerName = string.IsNullOrEmpty(nameInputField.text) ? "Anonymous" : nameInputField.text;
        LeaderboardManager.SaveScore(playerName, finalTime, currentGameMode);

        if (leaderboardPosterDisplay != null)
        {
            LeaderboardPoster posterScript = leaderboardPosterDisplay.GetComponent<LeaderboardPoster>();
            if (posterScript != null)
            {
                posterScript.gameMode = currentGameMode;
                posterScript.DisplayLeaderboard();
            }
        }

        OVRVirtualKeyboard metaKeyboard = FindObjectOfType<OVRVirtualKeyboard>();
        if (metaKeyboard != null)
        {
            Debug.Log("[END GAME UI] Safely hiding the OVR Virtual Keyboard.");
            metaKeyboard.gameObject.SetActive(false);
        }

        if (nameInputField != null)
        {
            nameInputField.text = "";
        }

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.ResetGameVariablesForNewRun();
        }

        PlayerPrefs.SetString("LastMode", currentGameMode.ToString());
        PlayerPrefs.Save();

        Debug.Log("[END GAME UI] Reloading active scene file...");
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }
}