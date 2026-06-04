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

public enum GameMode
{
    Basket,
    Trolley
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class LeaderboardManager : MonoBehaviour
{
    private const int MaxEntries = 5;

    public static string GetKey(GameMode mode) => $"Leaderboard_{mode}";

    public static void SaveScore(string name, float time, GameMode mode)
    {
        string key = GetKey(mode);
        LeaderboardData data = LoadLeaderboard(mode);
        data.entries.Add(new LeaderboardEntry(name, time));

        data.entries.Sort((a, b) => a.completionTime.CompareTo(b.completionTime));

        if (data.entries.Count > MaxEntries)
        {
            data.entries.RemoveRange(MaxEntries, data.entries.Count - MaxEntries);
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static LeaderboardData LoadLeaderboard(GameMode mode)
    {
        string key = GetKey(mode);
        if (!PlayerPrefs.HasKey(key))
            return new LeaderboardData();

        string json = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<LeaderboardData>(json);
    }
}