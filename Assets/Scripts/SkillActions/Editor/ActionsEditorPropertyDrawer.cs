using SkillActions.Actions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkillActions.Editor
{
    [CustomPropertyDrawer(typeof(ActionsEditorAttribute))]
    public class ActionsEditorPropertyDrawer : PropertyDrawer
    {
        private Dictionary<int, ReorderableActions> listPerHash = new ();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
            {
                if (!EditorUtility.IsPersistent(property.serializedObject.targetObject)) return;
                var newInstance = ScriptableObject.CreateInstance<CompositeScriptableAction>();
                newInstance.name = $"CompositeScriptableAction_{property.name}";
                AssetDatabase.AddObjectToAsset(newInstance, property.serializedObject.targetObject);
                AssetDatabase.SaveAssetIfDirty(property.serializedObject.targetObject);
                property.objectReferenceValue = newInstance;
            }

            var composite = property.objectReferenceValue as CompositeScriptableAction;

            var reference = new SerializedObject(composite);
            var actions = reference.FindProperty("actions");
            var hash = composite.GetHashCode();
            if (!listPerHash.ContainsKey(hash))
            {
                var reorderableActions = new ReorderableActions(property.name, reference, actions, reference.targetObject);
                listPerHash.Add(hash, reorderableActions);
            }

            EditorGUI.LabelField(position, property.name);
            listPerHash[hash].OnInspectorGUI();
        }
    }
}
