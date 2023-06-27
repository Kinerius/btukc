using UnityEditor;
using UnityEngine;

namespace SkillActions.Editor
{
    public static class CustomEditorUtils
    {
        public static void DrawHorizontalGUILayoutLine(int height = 1) {
            GUILayout.Space(4);

            Rect rect = GUILayoutUtility.GetRect(10, height, GUILayout.ExpandWidth(true));
            rect.height = height;
            rect.xMin = 0;
            rect.xMax = EditorGUIUtility.currentViewWidth;

            Color lineColor = new Color(0.10196f, 0.10196f, 0.10196f, 1);
            EditorGUI.DrawRect(rect, lineColor);
            GUILayout.Space(4);
        }

        public static void DrawHorizontalGUILine(Rect rect, int height = 1) {
            GUILayout.Space(4);

            rect.height = height;
            rect.xMin = 0;
            rect.xMax = EditorGUIUtility.currentViewWidth;

            Color lineColor = new Color(0.10196f, 0.10196f, 0.10196f, 1);
            EditorGUI.DrawRect(rect, lineColor);
        }
    }
}
