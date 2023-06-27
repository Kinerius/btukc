using SkillActions.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = System.Object;

namespace SkillActions.Editor
{
    public class ReorderableActions
    {
        private const float SPACING = 2;
        private readonly ReorderableList list;
        private readonly SerializedObject serializedObject;
        private readonly GenericMenu addDropdownMenu;
        private readonly Dictionary<uint, ReorderableActions> recursiveEditor = new ();
        private float lastHeight;
        private SerializedProperty serializedProperty;
        private UnityEngine.Object asset;
        private readonly int depth;
        private string name;

        public ReorderableActions(string name, SerializedObject serializedObject, SerializedProperty serializedProperty, UnityEngine.Object asset, int depth = 0)
        {
            this.name = name;
            this.asset = asset;
            this.depth = depth;
            this.serializedProperty = serializedProperty;
            this.serializedObject = serializedObject;

            list = new ReorderableList(this.serializedObject, serializedProperty, true, true, true, true)
            {
                drawElementCallback = DrawListItems,
                elementHeightCallback = ElementHeight,
                drawHeaderCallback = DrawHeader,
                onAddDropdownCallback = DrawAddDropdown,
                onRemoveCallback = OnRemoveCallback,
            };

            var types = AppDomain.CurrentDomain.GetAssemblies()
                                 .SelectMany(assembly => assembly.GetTypes())
                                 .Where(type => type.IsSubclassOf(typeof(ScriptableAction)));

            addDropdownMenu = new GenericMenu();

            foreach (Type type in types)
            {
                string typeName = type.ToString().Split('.')[^1];

                addDropdownMenu.AddItem(new GUIContent(typeName), false, () =>
                {
                    serializedObject.Update();
                    list.serializedProperty.arraySize++;
                    int index = list.serializedProperty.arraySize - 1;

                    var newInstance = (ScriptableAction)ScriptableObject.CreateInstance(type);
                    newInstance.name = $"{typeName}";

                    AssetDatabase.AddObjectToAsset(newInstance, asset);
                    AssetDatabase.SaveAssetIfDirty(asset);

                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    element.objectReferenceValue = newInstance;

                    serializedObject.ApplyModifiedProperties();
                });
            }
        }

        private void OnRemoveCallback(ReorderableList reorderableList)
        {
            var index = reorderableList.index;
            var propertyToRemove = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            if (recursiveEditor.ContainsKey(propertyToRemove.contentHash))
            {
                recursiveEditor.Remove(propertyToRemove.contentHash);
            }

            HashSet<UnityEngine.Object> objectsToRemove = new HashSet<UnityEngine.Object>
                { propertyToRemove.objectReferenceValue };

            RecursiveRemoval(propertyToRemove, ref objectsToRemove);

            foreach (UnityEngine.Object o in objectsToRemove)
                AssetDatabase.RemoveObjectFromAsset(o);

            reorderableList.serializedProperty.DeleteArrayElementAtIndex(index);
            AssetDatabase.SaveAssetIfDirty(asset);
        }

        // The array value that we are trying to delete might contain more scriptable objects within its properties, so we search for them
        private void RecursiveRemoval(SerializedProperty propertyToRemove, ref HashSet<UnityEngine.Object> objectsToRemove)
        {
            var objRef = new SerializedObject(propertyToRemove.objectReferenceValue);
            var iter = objRef.GetIterator();
            var child = true;
            while (iter.NextVisible(child))
            {
                child = false;
                if (recursiveEditor.ContainsKey(iter.contentHash))
                    recursiveEditor.Remove(iter.contentHash);

                if (iter.type == "vector")
                    if (iter.isArray)
                        for (var i = 0; i < iter.arraySize; i++)
                        {
                            var arrayValue = iter.GetArrayElementAtIndex(i);

                            if (arrayValue.propertyType == SerializedPropertyType.ObjectReference)
                                objectsToRemove.Add(arrayValue.objectReferenceValue);

                            RecursiveRemoval(arrayValue, ref objectsToRemove);
                        }
            }
        }

        private void DrawAddDropdown(Rect rect, ReorderableList reorderableList)
        {
            addDropdownMenu.ShowAsContext();
        }

        private float ElementHeight(int index)
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

            var serializedReference = new SerializedObject(element.objectReferenceValue);
            var iterator = serializedReference.GetIterator();
            var enterChildren = true;
            float totalHeight = 0;

            totalHeight += 18 + SPACING; // element header

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (iterator.displayName == "Script") continue;

                if (iterator.serializedObject.targetObject is CompositeScriptableAction)
                {
                    if (iterator.depth == 0) { totalHeight += 18 + SPACING; }
                }
                else
                {
                    totalHeight += EditorGUI.GetPropertyHeight(iterator) + SPACING;
                }
            }

            return totalHeight;
        }

        private void DrawListItems(Rect rect, int index, bool isactive, bool isfocused)
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

            var serializedReference = new SerializedObject(element.objectReferenceValue);

            serializedReference.Update();

            rect.height = 18;
            EditorGUI.LabelField(rect, element.objectReferenceValue.name);
            rect.y += 18 + SPACING;
            rect.x += 10;
            rect.width -= 10;

            var iterator = serializedReference.GetIterator();

            var enterChildren = true;
            var bufferRect = new Rect(rect);

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (iterator.displayName == "Script") continue;

                if (iterator.serializedObject.targetObject is CompositeScriptableAction)
                {
                    if (iterator.depth == 0)
                    {
                        uint hash = iterator.contentHash;

                        if (!recursiveEditor.ContainsKey(hash))
                        {
                            SerializedProperty copyProperty = iterator.Copy();
                            var editor = new ReorderableActions(iterator.displayName, copyProperty.serializedObject, copyProperty, copyProperty.serializedObject.targetObject, depth + 1);
                            recursiveEditor.Add(hash, editor);
                        }

                        float height = 18f;
                        bufferRect.height = height;
                        EditorGUI.LabelField(bufferRect, $"[{depth + 1}] {iterator.displayName}");
                        bufferRect.y += height + SPACING;

                        // we draw the recursive list below the current list
                        recursiveEditor[hash].OnInspectorGUI();
                    }
                }
                else
                {
                    float height = EditorGUI.GetPropertyHeight(iterator);
                    bufferRect.height = height;
                    iterator.serializedObject.Update();
                    EditorGUI.PropertyField(bufferRect, iterator, iterator.propertyType == SerializedPropertyType.Generic);
                    iterator.serializedObject.ApplyModifiedProperties();

                    bufferRect.y += height + SPACING;
                }
            }

            serializedReference.ApplyModifiedProperties();
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, $"[{depth}] {name}");
        }

        public void OnInspectorGUI()
        {
            serializedObject.Update();
            list.DoLayoutList();
            lastHeight = list.GetHeight();
            serializedObject.ApplyModifiedProperties();
        }

        public void OnInspectorGUI(Rect rect)
        {
            serializedObject.Update();
            serializedObject.SetIsDifferentCacheDirty();

            list.DoList(rect);

            lastHeight = list.GetHeight();
            serializedObject.ApplyModifiedProperties();
        }

        public float GetHeight() =>
            lastHeight;
    }
}
