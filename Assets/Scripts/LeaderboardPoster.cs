using TMPro;
using UnityEngine;

public class LeaderboardPoster : MonoBehaviour
{
    public TMP_Text[] entryRows; // Link your 5 row text components here in Inspector
    public GameMode gameMode = GameMode.Basket;

    void Start()
    {
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        LeaderboardData data = LeaderboardManager.LoadLeaderboard(gameMode);

        // Initialize all rows as empty dashes
        for (int i = 0; i < entryRows.Length; i++)
        {
            entryRows[i].text = $"{i + 1}. ---";
        }

        // Populate rows with real scores
        for (int i = 0; i < data.entries.Count; i++)
        {
            if (i >= entryRows.Length) break;

            var entry = data.entries[i];
            int minutes = Mathf.FloorToInt(entry.completionTime / 60);
            int seconds = Mathf.FloorToInt(entry.completionTime % 60);

            // Format layout: "1. Bob - 1:24"
            entryRows[i].text = $"{i + 1}. {entry.playerName} — {minutes:0}:{seconds:00}";
        }
    }
}