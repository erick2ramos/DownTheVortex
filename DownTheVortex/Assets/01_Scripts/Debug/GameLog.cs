using UnityEngine;
using System.Collections;

public static class GameLog
{
    public static void Log(object message, Object context)
    {
        Debug.Log(message, context);
    }

    public static void Log(object message)
    {
        Debug.Log(message);
    }

    public static void LogError(object message)
    {
        Debug.LogError(message);
    }
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }
}
