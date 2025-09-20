using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperClass : MonoBehaviour
{
    /// <summary>
    /// Position of mouse in pixels on screen (has to be set by SetMousePos()).
    /// </summary>
    public static Vector3 MousePos => _mousePos;
    private static Vector2 _mousePos;
    public static void SetMousePos(Vector2 pos)
    {
        _mousePos = pos;
    }
    public static IEnumerator DelayedFunction(float timeToWait, Action function)
    {
        yield return new WaitForSeconds(timeToWait);
        function();
    }
    public static IEnumerator WaitFrame(Action function)
    {
        yield return null;
        function();
    }
    public static bool CheckIfObjectIsBehind(Vector2 gameObjectPos, Vector2 gameObjectToCheckPos, GlobalEnums.HorizontalDirections gameObjectLookingDirection)
    {
        // sub result - <0 means gameObjectToCheckPos is on right, else its on left. Mult result - <0 gameObjectToCheckPos is in front, else gameObjectToCheckPos is behind
        if ((gameObjectPos.x - gameObjectToCheckPos.x) * ((int)gameObjectLookingDirection) <= 0) return false;
        else return true;
    }
    /// <summary>
    /// Returns minimum of two numbers as minX and maximum as MaxX.
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="x2"></param>
    /// <param name="minX"></param>
    /// <param name="maxX"></param>
    public static void SetMinMaxValues(float x1, float x2, out float minX, out float maxX)
    {
        maxX = Unity.Mathematics.math.max(x1, x2);
        minX = Unity.Mathematics.math.min(x1, x2);
    }

    public static string FormatSecondsToTime(int totalSeconds)
    {
        int hours = (int)totalSeconds / 3600;
        int remainingSeconds = totalSeconds % 3600;
        int minutes = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;
        return string.Format("{0:00},{1:00}:{2:00}", hours,minutes, seconds);
    }
}
