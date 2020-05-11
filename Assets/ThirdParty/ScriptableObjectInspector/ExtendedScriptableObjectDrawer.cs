// Developed by Tom Kail at Inkle
// Modifications made by Luiz Wendt
// Nested Scriptable Object Feature by as3mbus 
// Released under the MIT Licence as held at https://opensource.org/licenses/MIT

// Must be placed within a folder named "Editor"

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUIUtility;

namespace Github.ScriptableObjectExtension
{
    /// <summary>
    /// Extends how ScriptableObject object references are displayed in the inspector
    /// Shows you all values under the object reference
    /// Also provides a button to create a new ScriptableObject if property is null.
    /// todo: enable custom editors for scriptable objects
    /// </summary>
    ///
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public class ExtendedScriptableObjectDrawer : PropertyDrawer
    {
        private const int CREATE_BUTTON_WIDTH = 48;
        private int index;
        private Type[] typeOptions;
        private string[] stringOptions;

        private Type[] TypeOptions
        {
            get
            {
                typeOptions = typeOptions ?? AvailableType();
                return typeOptions;
            }
        }

        private string[] StringOptions
        {
            get
            {
                stringOptions = stringOptions ?? TypeOptions
                    .Select(type => type.ToString().Split('.').Last())
                    .ToArray();
                return stringOptions;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = singleLineHeight;
            if ((TypeOptions.Length > 1 || GetPropertyType().IsAbstract) && !IsThereAnyVisibileProperty(property))
                return totalHeight * 2;
            if (!IsThereAnyVisibileProperty(property)) return totalHeight;
            if (!property.isExpanded) return totalHeight;
            ScriptableObject data = property.objectReferenceValue as ScriptableObject;
            if (data == null) return singleLineHeight * 2;
            SerializedObject serializedObject = new SerializedObject(data);
            SerializedProperty prop = serializedObject.GetIterator();
            totalHeight += standardVerticalSpacing;
            if (!prop.NextVisible(true)) return totalHeight;
            do
            {
                if (prop.name == "m_Script") continue;
                SerializedProperty subProp = serializedObject.FindProperty(prop.name);
                float height = EditorGUI.GetPropertyHeight(subProp, null, true) +
                               standardVerticalSpacing;
                totalHeight += height;
            } while (prop.NextVisible(false));

            // Add a tiny bit of height if open for the background

            return totalHeight;
        }

        private Type[] AvailableType()
        {
            return
            (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where GetPropertyType().IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract
                select assemblyType
            ).ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BeginProperty(position, label, property);
            if (property.objectReferenceValue != null)
            {
                DrawFoldOut(position, property);
                DrawAssignedPropertyField(position, property);
                if (GUI.changed) property.serializedObject.ApplyModifiedProperties();
                if (property.objectReferenceValue == null) GUIUtility.ExitGUI();
                if (property.isExpanded) DrawSerializedFields(position, property);
            }
            else
            {
                DrawTypeSelectionDropdown(position);

                Rect fieldLayout = new Rect(
                    position.x,
                    position.y,
                    position.width - 2 - CREATE_BUTTON_WIDTH * 2,
                    singleLineHeight);

                ObjectField(fieldLayout, property);

                DrawCreateExternalButton(position, property);
                DrawCreateInternalButton(position, property);
            }

            property.serializedObject.ApplyModifiedProperties();
            EndProperty();
        }

        private static void DrawFoldOut(Rect position, SerializedProperty property)
        {
            Rect foldoutLayout = new Rect(position.x, position.y, labelWidth, singleLineHeight);
            if (IsThereAnyVisibileProperty(property))
            {
                property.isExpanded = Foldout(
                    foldoutLayout,
                    property.isExpanded,
                    property.displayName, true);
            }
            else
            {
                LabelField(
                    foldoutLayout, property.displayName);
                property.isExpanded = false;
            }
        }

        private static void DrawAssignedPropertyField(Rect position, SerializedProperty property)
        {
            bool isNestedObject = (AssetDatabase.GetAssetPath(property.objectReferenceValue) ==
                                   AssetDatabase.GetAssetPath(property.serializedObject.targetObject));

            int buttonOffset = isNestedObject ? 60 : 0;

            Rect fieldLayout = new Rect(
                labelWidth + 14,
                position.y,
                position.width - labelWidth - buttonOffset,
                singleLineHeight);

            PropertyField(
                fieldLayout,
                property,
                GUIContent.none,
                true
            );

            if (!isNestedObject) return;
            DrawDeleteInternalButton(position, property);
        }

        private static void DrawSerializedFields(Rect position, SerializedProperty property)
        {
            float boxPosY = position.y + singleLineHeight + standardVerticalSpacing - 1;
            float boxHeight = position.height - singleLineHeight - standardVerticalSpacing;
            // Draw a background that shows us clearly which fields are part of the ScriptableObject

            Rect boxLayout = new Rect(0, boxPosY, Screen.width, boxHeight);
            GUI.Box(boxLayout, "");

            indentLevel++;
            ScriptableObject data = (ScriptableObject) property.objectReferenceValue;
            SerializedObject serializedObject = new SerializedObject(data);


            // Iterate over all the values and draw them
            SerializedProperty prop = serializedObject.GetIterator();
            float y = position.y + singleLineHeight + standardVerticalSpacing;

            if (!prop.NextVisible(true)) return;

            do
            {
                // Don't bother drawing the class file
                if (prop.name == "m_Script") continue;

                float fieldHeight =
                    EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);

                Rect fieldLayout = new Rect(position.x, y, position.width, fieldHeight);

                PropertyField(fieldLayout, prop, true);
                y += fieldHeight + standardVerticalSpacing;
            } while (prop.NextVisible(false));

            if (GUI.changed) serializedObject.ApplyModifiedProperties();

            indentLevel--;
        }

        private void DrawTypeSelectionDropdown(Rect position)
        {
            if (typeOptions.Length <= 1 && !GetPropertyType().IsAbstract) return;
            Rect labelLayout = new Rect(
                position.x + 14,
                position.y + singleLineHeight + standardVerticalSpacing,
                labelWidth - 14,
                singleLineHeight);

            LabelField(labelLayout, "Add : ");

            Rect popupLayout = new Rect(
                labelWidth + 14 - (14 * (indentLevel - 1)),
                position.y + singleLineHeight + standardVerticalSpacing,
                position.width - labelWidth + (14 * (indentLevel - 1)),
                singleLineHeight);

            index = Popup(popupLayout, index, StringOptions);
        }

        private void DrawCreateExternalButton(Rect position, SerializedProperty property)
        {
            Rect buttonLayout = new Rect(
                position.x + position.width - CREATE_BUTTON_WIDTH,
                position.y,
                CREATE_BUTTON_WIDTH,
                singleLineHeight);

            if (!GUI.Button(buttonLayout, "+ Out")) return;

            string selectedAssetPath = "Assets";

            if (property.serializedObject.targetObject is MonoBehaviour behaviour)
            {
                MonoScript ms = MonoScript.FromMonoBehaviour(behaviour);
                selectedAssetPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
            }

            Type type = (TypeOptions.Length <= 1) ? GetPropertyType() : TypeOptions[index];
            property.objectReferenceValue = CreateAssetWithSavePrompt(type, selectedAssetPath);
        }

        private void DrawCreateInternalButton(Rect position, SerializedProperty property)
        {
            Rect buttonLayout = new Rect(
                position.x + position.width - CREATE_BUTTON_WIDTH * 2,
                position.y,
                CREATE_BUTTON_WIDTH,
                singleLineHeight);
            if (!GUI.Button(buttonLayout, "+ In")) return;

            Type type = (TypeOptions.Length <= 1 && !GetPropertyType().IsAbstract) ? GetPropertyType() : TypeOptions[index];
            string assetPath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
            string additionalName = "";

            if (string.IsNullOrEmpty(assetPath))
                if (property.serializedObject.targetObject is MonoBehaviour mono)
                {
                    GameObject gameObject = mono.gameObject;
                    assetPath = gameObject.scene.path;
                    additionalName = gameObject.scene.name;
                }
                else if (Selection.activeObject is GameObject gameObject)
                {
                    assetPath = gameObject.scene.path;
                    additionalName = gameObject.scene.name;
                }
                else throw new Exception("Unable to find persistent asset path");

            string assetName = $"{additionalName}_{property.serializedObject.targetObject.name}_{property.name}";
            property.objectReferenceValue = CreateInternalAsset(type, assetName, assetPath);
        }

        private Type GetPropertyType()
        {
            Type type = fieldInfo.FieldType;
            if (type.IsArray) type = type.GetElementType();
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                type = type.GetGenericArguments()[0];
            return type;
        }

        private static void DrawDeleteInternalButton(Rect position, SerializedProperty property)
        {
            Rect buttonLayout = new Rect(
                position.x + position.width - 60,
                position.y,
                60,
                singleLineHeight);

            if (!GUI.Button(buttonLayout, "Del")) return;

            if (AssetDatabase.Contains(property.serializedObject.targetObject))
            {
                AssetDatabase.RemoveObjectFromAsset(property.objectReferenceValue);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(property.serializedObject.targetObject));
            }

            AssetDatabase.SaveAssets();
            Object.DestroyImmediate(property.objectReferenceValue);
            property.objectReferenceValue = null;
        }

        // Creates a new ScriptableObject via the default Save File window
        private static ScriptableObject CreateAssetWithSavePrompt(Type type, string path)
        {
            path = EditorUtility.SaveFilePanelInProject(
                "Save ScriptableObject",
                "New " + type.Name + ".asset", "asset",
                "Enter a file name for the ScriptableObject.", path);
            if (path == "") return null;
            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            PingObject(asset);
            return asset;
        }

        private static ScriptableObject CreateInternalAsset(Type type, string name, string path)
        {
            ScriptableObject newAsset = ScriptableObject.CreateInstance(type);
            newAsset.name = name;
            AssetDatabase.AddObjectToAsset(newAsset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newAsset));
            return newAsset;
        }

        private static bool IsThereAnyVisibileProperty(SerializedProperty property)
        {
            ScriptableObject data = (ScriptableObject) property.objectReferenceValue;
            if (!data) return false;
            SerializedObject serializedObject = new SerializedObject(data);

            SerializedProperty prop = serializedObject.GetIterator();
            if (prop == null) return false;

            while (prop.NextVisible(true))
                if (prop.name != "m_Script")
                    return true; //if theres any visible property other than m_script

            return false;
        }
    }
}