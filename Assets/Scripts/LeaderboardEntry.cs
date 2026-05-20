using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public float completionTime;

    public LeaderboardEntry(string name, float time)
    {
        playerName = name;
        completionTime = time;
    }
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class LeaderboardManager : MonoBehaviour
{
    private const string SaveKey = "SupermarketLeaderboard";
    private const int MaxEntries = 5;

    public static void SaveScore(string name, float time)
    {
        LeaderboardData data = LoadLeaderboard();
        data.entries.Add(new LeaderboardEntry(name, time));

        data.entries.Sort((a, b) => a.completionTime.CompareTo(b.completionTime));

        if (data.entries.Count > MaxEntries)
        {
            data.entries.RemoveRange(MaxEntries, data.entries.Count - MaxEntries);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static LeaderboardData LoadLeaderboard()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return new LeaderboardData();

        string json = PlayerPrefs.GetString(SaveKey);
        return JsonUtility.FromJson<LeaderboardData>(json);
    }
}