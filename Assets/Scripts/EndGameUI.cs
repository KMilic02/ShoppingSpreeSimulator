using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_Text timeDisplayText;
    public Button submitButton;
    public GameObject leaderboardPosterDisplay;

    private float finalTime;

    public void OpenEndGameUI(float rawTime)
    {
        finalTime = rawTime;
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
        LeaderboardManager.SaveScore(playerName, finalTime);
        
        if (leaderboardPosterDisplay != null)
        {
            LeaderboardPoster posterScript = leaderboardPosterDisplay.GetComponent<LeaderboardPoster>();
            if (posterScript != null)
            {
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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); 
        if (playerObj != null)
        {
            CharacterController cc = playerObj.GetComponent<CharacterController>();
            Rigidbody rb = playerObj.GetComponent<Rigidbody>();

            if (cc != null) cc.enabled = false; 

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Vector3 safeSpawnPos = new Vector3(17.84f, 0f, -3.067f);
            playerObj.transform.position = safeSpawnPos;
            playerObj.transform.rotation = Quaternion.identity;

            if (cc != null) cc.enabled = true;

            Debug.Log("[END GAME UI] Teleported player safely above the floor grid.");
        }

        gameObject.SetActive(false);
    }
}