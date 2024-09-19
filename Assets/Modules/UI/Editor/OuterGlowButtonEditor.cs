#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(OuterGlowButton))]
public class OuterGlowButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        OuterGlowButton targetMenuButton = (OuterGlowButton)target;
        
        EditorGUILayout.LabelField("Glow", EditorStyles.boldLabel);

        targetMenuButton.outglowDuration = EditorGUILayout.FloatField("Glow duration", targetMenuButton.outglowDuration);
        targetMenuButton.outglowOnNormal = EditorGUILayout.Toggle("Glow on normal", targetMenuButton.outglowOnNormal);
        targetMenuButton.outglowOnHovered = EditorGUILayout.Toggle("Glow on hovered", targetMenuButton.outglowOnHovered);
        targetMenuButton.outglowOnPressed = EditorGUILayout.Toggle("Glow on pressed", targetMenuButton.outglowOnPressed);
        targetMenuButton.outglowOnSelected = EditorGUILayout.Toggle("Glow on selected", targetMenuButton.outglowOnSelected);
        targetMenuButton.outglowOnDisabled = EditorGUILayout.Toggle("Glow on disabled", targetMenuButton.outglowOnDisabled);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Focus", EditorStyles.boldLabel);
        targetMenuButton.focusOnEnable = EditorGUILayout.Toggle("Focus on enable", targetMenuButton.focusOnEnable);
        Debug.Log("Focus on enable: " + targetMenuButton.focusOnEnable);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Button", EditorStyles.boldLabel);
        
        base.OnInspectorGUI();
        
        EditorUtility.SetDirty(targetMenuButton);
    }
}

#endif