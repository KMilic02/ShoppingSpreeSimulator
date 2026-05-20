using TMPro;
using UnityEngine;

public class VRConsoleLogger : MonoBehaviour
{
    public TMP_Text debugTextDisplay;
    private string myLogQueue = "";

    void OnEnable()
    {
        // Tells Unity to send log messages to our custom function
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Color errors red so they jump out at you in VR
        if (type == LogType.Exception || type == LogType.Error)
        {
            myLogQueue += $"<color=red>[ERROR]</color> {logString}\n";
        }
        else
        {
            myLogQueue += $"[LOG] {logString}\n";
        }

        // Keep the text on screen from getting infinitely long
        if (myLogQueue.Length > 1000)
        {
            myLogQueue = myLogQueue.Substring(myLogQueue.Length - 1000);
        }

        debugTextDisplay.text = myLogQueue;
    }
}