using System;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Stats.ScriptableObjects.Editor
{
    public class ReorderableStats
    {
        private readonly ReorderableList list;
        private readonly SerializedObject serializedObject;

        public ReorderableStats(SerializedObject serializedObject, SerializedProperty serializedProperty)
        {
            this.serializedObject = serializedObject;

            list = new ReorderableList(this.serializedObject, serializedProperty, true, true, true, true)
                {
                    drawElementCallback = DrawListItems,
                    drawHeaderCallback = DrawHeader,
                };

        }

        private void DrawListItems(Rect rect, int index, bool isactive, bool isfocused)
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

            var statWidth = 180;
            var whiteSpace = 5;
            var valueTextWidth = 35;
            var initialValueWidth = 180;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, statWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("stat"),
                GUIContent.none
            );

            EditorGUI.LabelField(new Rect(rect.x + statWidth + whiteSpace, rect.y, valueTextWidth, EditorGUIUtility.singleLineHeight), "Value");

            EditorGUI.PropertyField(
                new Rect(rect.x + statWidth + valueTextWidth + whiteSpace*2, rect.y, initialValueWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("initialValue"),
                GUIContent.none
            );
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Stats");
        }

        public void OnInspectorGUI()
        {
            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
