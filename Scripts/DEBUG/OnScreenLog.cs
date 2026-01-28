using UnityEngine;
using System.Collections.Generic;

public class OnScreenLog : MonoBehaviour
{
    [SerializeField] private int maxLines = 10;
    [SerializeField] private int fontSize = 20;
    [SerializeField] private Font customFont;

    private class LogEntry
    {
        public string Message { get; }
        public Color Color { get; }
        public float ExpiryTime { get; }

        public LogEntry(string message, Color color, float expiryTime)
        {
            Message = message;
            Color = color;
            ExpiryTime = expiryTime;
        }
    }

    private readonly List<LogEntry> logEntries = new List<LogEntry>();

    public void Log(string message)
    {
        AddToLog(message, Color.cyan, Time.time + 5f);
    }

    public void LogWarn(string message)
    {
        AddToLog(message, Color.yellow, float.MaxValue);
    }

    public void LogErr(string message)
    {
        AddToLog("ERROR: " + message, Color.red, float.MaxValue);
    }

    private void AddToLog(string message, Color color, float expiryTime)
    {
        if (logEntries.Count >= maxLines)
        {
            logEntries.RemoveAt(0);
        }

        logEntries.Add(new LogEntry(message, color, expiryTime));
    }

    private void Update()
    {
        logEntries.RemoveAll(entry => entry.ExpiryTime <= Time.time);
    }

    private void OnGUI()
    {
        if (!Debug.isDebugBuild && !Application.isEditor) return;

        GUIStyle logStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = fontSize,
            font = customFont
        };

        float yOffset = 10f;
        float lineHeight = logStyle.lineHeight > 0 ? logStyle.lineHeight : fontSize * 1.2f;

        foreach (var entry in logEntries)
        {
            logStyle.normal.textColor = entry.Color;
            GUI.Label(new Rect(10f, yOffset, Screen.width - 20f, lineHeight), entry.Message, logStyle);
            yOffset += lineHeight;
        }
    }
}