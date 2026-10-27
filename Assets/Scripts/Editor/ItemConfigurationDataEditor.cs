using System;
using System.Linq;
using System.Reflection;
using InventoryCode.Data;
using InventoryCode.Data.ItemConfigurationComponents;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ItemConfigurationData))]
    public sealed class ItemConfigurationDataEditor : UnityEditor.Editor
    {
        private SerializedProperty _componentsProperty;

        private void OnEnable()
        {
            _componentsProperty =
                serializedObject.FindProperty("_components");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(
                serializedObject,
                "_components"
            );

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(
                "Components",
                EditorStyles.boldLabel
            );

            DrawComponents();

            EditorGUILayout.Space();

            if (GUILayout.Button("Add Component"))
            {
                ShowAddComponentMenu();
            }

            serializedObject.ApplyModifiedProperties();
        }

        
        
        private void DrawComponents()
        {
            for (int i = 0; i < _componentsProperty.arraySize; i++)
            {
                SerializedProperty element =
                    _componentsProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                // Header
                Rect headerRect = EditorGUILayout.GetControlRect(
                    false,
                    EditorGUIUtility.singleLineHeight + 4f
                );

                headerRect.xMin += 4f;
                headerRect.xMax -= 4f;

                const float buttonWidth = 65f;
                Rect buttonRect = new Rect(
                    headerRect.xMax - buttonWidth,
                    headerRect.y,
                    buttonWidth,
                    headerRect.height
                );

                Rect foldoutRect = headerRect;
                foldoutRect.xMax = buttonRect.xMin - 4f;

                element.isExpanded = EditorGUI.Foldout(
                    foldoutRect,
                    element.isExpanded,
                    GetComponentName(element),
                    true,
                    EditorStyles.foldout
                );

                // Remove button
                if (GUI.Button(buttonRect, "Remove"))
                {
                    _componentsProperty.DeleteArrayElementAtIndex(i);
                    EditorGUILayout.EndVertical();
                    break;
                }

                // Component fields without the duplicate header
                if (element.isExpanded)
                {
                    EditorGUI.indentLevel++;

                    SerializedProperty child = element.Copy();
                    SerializedProperty end = child.GetEndProperty();

                    bool enterChildren = true;

                    while (child.NextVisible(enterChildren) &&
                           !SerializedProperty.EqualContents(child, end))
                    {
                        enterChildren = false;

                        EditorGUILayout.PropertyField(child, true);
                    }

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void ShowAddComponentMenu()
        {
            GenericMenu menu = new();

            Type[] componentTypes = GetComponentTypes();

            foreach (Type type in componentTypes)
            {
                menu.AddItem(
                    new GUIContent(type.Name),
                    false,
                    () => AddComponent(type)
                );
            }

            if (componentTypes.Length == 0)
            {
                menu.AddDisabledItem(
                    new GUIContent("No Components Available")
                );
            }

            menu.ShowAsContext();
        }

        private void AddComponent(Type type)
        {
            serializedObject.Update();

            int index = _componentsProperty.arraySize;
            _componentsProperty.arraySize++;

            SerializedProperty element =
                _componentsProperty.GetArrayElementAtIndex(index);

            element.managedReferenceValue =
                Activator.CreateInstance(type);

            serializedObject.ApplyModifiedProperties();
        }

        private static Type[] GetComponentTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(GetAssemblyTypes)
                .Where(type =>
                    type != null &&
                    typeof(ItemComponentData).IsAssignableFrom(type) &&
                    !type.IsAbstract &&
                    !type.IsInterface &&
                    !type.ContainsGenericParameters &&
                    type.IsDefined(typeof(SerializableAttribute), false)
                )
                .OrderBy(type => type.Name)
                .ToArray();
        }

        private static Type[] GetAssemblyTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types
                    .Where(type => type != null)
                    .ToArray();
            }
        }

        private static string GetComponentName(
            SerializedProperty property)
        {
            string typeName = property.managedReferenceFullTypename;

            if (string.IsNullOrEmpty(typeName))
                return "Missing Component";

            int separator = typeName.LastIndexOf(' ');

            return separator >= 0
                ? typeName.Substring(separator + 1).Split('.').Last()
                : typeName;
        }
    }
}
