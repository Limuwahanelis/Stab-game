using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DebugOnlyAttribute))]
public class DebugOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsInspectorInDebugMode())
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    private bool IsInspectorInDebugMode()
    {
        // Unity doesn't expose a "debug mode" API, so we have to hack it a bit:
        var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        var inspectorWindow = EditorWindow.focusedWindow;
        return inspectorWindow && inspectorWindow.GetType() == inspectorType
               && inspectorWindow.ToString().Contains("Debug");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return IsInspectorInDebugMode() ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
    }
}
