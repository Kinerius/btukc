using SkillActions.Actions;
using Stats;
using Stats.ScriptableObjects.Editor;
using UnityEditor;
using UnityEngine;

namespace SkillActions.Editor
{
    [CustomEditor(typeof(SkillActionData))]
    public class SkillActionDataEditor : UnityEditor.Editor
    {
        private ReorderableStats reorderableStats;
        private ReorderableActions reorderableActions;

        private SerializedProperty actionsProperty;
        private SerializedProperty statsProperty;

        private void OnEnable()
        {
            actionsProperty = serializedObject.FindProperty("actions");
            statsProperty = serializedObject.FindProperty("abilityStats");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawStats();

            EditorGUILayout.Space();
            CustomEditorUtils.DrawHorizontalGUILayoutLine();

            DrawActions();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawStats()
        {
            var sad = target as SkillActionData;
            if (statsProperty.objectReferenceValue == null)
            {

                if (EditorUtility.IsPersistent(serializedObject.targetObject))
                {
                    serializedObject.Update();
                    var stats = CreateInstance<StatPackageData>();
                    stats.name = "Stats";
                    statsProperty.objectReferenceValue = stats;
                    sad.abilityStats = stats;
                    AssetDatabase.AddObjectToAsset(stats, serializedObject.targetObject);
                    AssetDatabase.SaveAssetIfDirty(stats);
                    serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
            else
            {
                if (reorderableStats == null)
                {
                    var serializedStats = new SerializedObject(statsProperty.objectReferenceValue);
                    var statsOfCollection = serializedStats.FindProperty("stats");

                    reorderableStats = new ReorderableStats(serializedStats, statsOfCollection);
                }

                reorderableStats.OnInspectorGUI();
            }
        }

        private void DrawActions()
        {
            EditorGUILayout.PropertyField(actionsProperty, true);
        }
    }
}
