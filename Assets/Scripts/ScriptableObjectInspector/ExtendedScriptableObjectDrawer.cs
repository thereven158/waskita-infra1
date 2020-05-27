// Developed by Tom Kail at Inkle
// Modifications made by Luiz Wendt
// Nested Scriptable Object Feature by as3mbus 
// Code Clean Up by as3mbus
// Released under the MIT Licence as held at https://opensource.org/licenses/MIT

// Must be placed within a folder named "Editor"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        #region Constants

        private const int DELETE_BUTTON_WIDTH = 60;
        private const int INDENT_WIDTH = 15;
        private const int HORIZONTAL_SPACING = 4;
        private const char HIERARCHY_SEPARATOR = '.';

        #endregion

        #region Property Helper Method

        #region Property Type

        private Type GetPropertyType()
        {
            Type type = fieldInfo.FieldType;
            if (type.IsArray) type = type.GetElementType();
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                type = type.GetGenericArguments()[0];
            return type;
        }

        private Type[] AvailablePropertyType()
        {
            return
            (
                from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where GetPropertyType().IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract
                select assemblyType
            ).ToArray();
        }

        #endregion

        #region Asset Management

        /// <summary>
        /// Destroy internal asset that assigned in property value
        /// </summary>
        /// <param name="property">specified property with value to be deleted</param>
        private static void DestructInternalPropertyValue(SerializedProperty property)
        {
            if (AssetDatabase.Contains(property.serializedObject.targetObject))
            {
                AssetDatabase.RemoveObjectFromAsset(property.objectReferenceValue);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(property.serializedObject.targetObject));
            }

            AssetDatabase.SaveAssets();
            Object.DestroyImmediate(property.objectReferenceValue);
            property.objectReferenceValue = null;
        }

        /// <summary>
        /// Creates a new ScriptableObject asset as a new asset file via the default Save File window
        /// </summary>
        /// <param name="type">object type</param>
        /// <param name="name">default file name</param>
        /// <param name="path">new assets file path</param>
        /// <returns>constructed object</returns>
        private static ScriptableObject CreateAssetWithSavePrompt(Type type, string name, string path)
        {
            path = EditorUtility.SaveFilePanelInProject(
                "Save ScriptableObject",
                name + ".asset", "asset",
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

        /// <summary>
        /// Creates a new ScriptableObject inside an asset file within specified filepath
        /// </summary>
        /// <param name="type">object type</param>
        /// <param name="name">scriptable object asset name</param>
        /// <param name="path">assets file path where new object will be added to</param>
        /// <returns>constructed object</returns>
        private static ScriptableObject CreateInternalAsset(Type type, string name, string path)
        {
            ScriptableObject newAsset = ScriptableObject.CreateInstance(type);
            newAsset.name = name;
            AssetDatabase.AddObjectToAsset(newAsset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newAsset));
            return newAsset;
        }

        /// <summary>
        /// Get asset path of internal value that assigned to specified property
        /// </summary>
        /// <param name="property">specified property</param>
        /// <returns>assets filepath</returns>
        /// <exception cref="Exception">assets path not found which means it's not a persistent assets</exception>
        private static string PropertyAssetPath(SerializedProperty property)
        {
            string assetPath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);

            if (!string.IsNullOrEmpty(assetPath)) return assetPath;
            if (property.serializedObject.targetObject is MonoBehaviour mono)
                assetPath = mono.gameObject.scene.path;
            else if (Selection.activeObject is GameObject gameObject)
                assetPath = gameObject.scene.path;
            else throw new Exception("Unable to find persistent asset path");

            return assetPath;
        }

        private static void RenamePropertyReference(SerializedProperty property, string newName)
        {
            property.objectReferenceValue.name = newName;
            if (AssetDatabase.IsMainAsset(property.objectReferenceValue))
            {
                string assetPath = AssetDatabase.GetAssetPath(property.objectReferenceValue);
                AssetDatabase.RenameAsset(assetPath, newName);
            }

            AssetDatabase.SaveAssets();
        }

        private static bool IsNestedAsset(SerializedProperty property)
        {
            return (AssetDatabase.GetAssetPath(property.objectReferenceValue) ==
                    AssetDatabase.GetAssetPath(property.serializedObject.targetObject));
        }

        #endregion

        private static bool IsThereAnyVisibileProperty(SerializedProperty property)
        {
            ScriptableObject data = (ScriptableObject) property.objectReferenceValue;
            if (!data) return false;
            SerializedObject serializedObject = new SerializedObject(data);

            SerializedProperty prop = serializedObject.GetIterator();
            if (prop == null) return false;

            while (prop.NextVisible(true))
                if (prop.name != "m_Script")
                    return true; //if there's any visible property other than m_script

            return false;
        }

        private string PropertyHierarchyName(SerializedProperty property)
        {
            List<string> hierarchyName = new List<string>();

            // scene prefix
            if (property.serializedObject.targetObject is MonoBehaviour mono)
                hierarchyName.Add($"{mono.gameObject.scene.name}");

            hierarchyName.Add(property.serializedObject.targetObject.name);

            // List Item Detection
            Type type = fieldInfo.FieldType;
            string propertyName = property.displayName;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                propertyName = $"{fieldInfo.Name}-{property.displayName.Split(' ')[1]}";

            hierarchyName.Add(propertyName);

            return string.Join("" + HIERARCHY_SEPARATOR, hierarchyName);
        }

        public static FieldInfo FieldInfo(SerializedProperty property)
        {
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath);
            return fi;
        }

        private static void PropertyModificationCheck(SerializedProperty property)
        {
            //unknown process that might need additional refactoring
            if (GUI.changed) property.serializedObject.ApplyModifiedProperties();
            if (property.objectReferenceValue == null) GUIUtility.ExitGUI();
        }
        
        #endregion

        #region Properties

        private Type[] _typeOptions;

        private Type[] TypeOptions
        {
            get
            {
                _typeOptions = _typeOptions ?? AvailablePropertyType();
                return _typeOptions;
            }
        }

        private string[] _typeStringOptions;

        private string[] TypeStringOptions
        {
            get
            {
                _typeStringOptions = _typeStringOptions ?? TypeOptions
                    .Select(type => type.ToString().Split('.').Last())
                    .ToArray();
                return _typeStringOptions;
            }
        }

        private bool HaveTypeOptions => TypeOptions.Length > 1 || GetPropertyType().IsAbstract;
        private Type SelectedObjectType => HaveTypeOptions ? TypeOptions[typeSelectionIndex] : GetPropertyType();
        private static int IndentOffset => (INDENT_WIDTH * indentLevel);

        #endregion

        #region Editor Variables

        private string valueObjectName;
        private int typeSelectionIndex;
        private bool isDrawn;

        private void AssignValueObjectName(SerializedProperty property)
        {
            if (isDrawn) return;
            valueObjectName = property.objectReferenceValue == null
                ? PropertyHierarchyName(property)
                : property.objectReferenceValue.name;
        }

        #endregion

        #region Unity Derivation Override

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = singleLineHeight + standardVerticalSpacing;
            ScriptableObject data = property.objectReferenceValue as ScriptableObject;

            if (data == null) return totalHeight + CreationPropertyHeight(property);
            if (!IsThereAnyVisibileProperty(property)) return totalHeight;
            if (!property.isExpanded) return totalHeight;

            SerializedObject serializedObject = new SerializedObject(data);
            SerializedProperty prop = serializedObject.GetIterator();
            totalHeight += singleLineHeight + standardVerticalSpacing;
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

            totalHeight += singleLineHeight + standardVerticalSpacing;
            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InitStyles();
            BeginProperty(position, label, property);
            AssignValueObjectName(property);
            if (property.objectReferenceValue != null)
            {
                DrawPropertyField(position, property);
                PropertyModificationCheck(property);

                Rect contentLayout = new Rect()
                {
                    x = position.x,
                    y = position.y + singleLineHeight + standardVerticalSpacing,
                    width = position.width,
                    height = position.height - singleLineHeight - standardVerticalSpacing
                };
                if (property.isExpanded) DrawSerializationContent(contentLayout, property);
            }
            else
            {
                Rect fieldLayout = new Rect(
                    position.x,
                    position.y,
                    position.width,
                    singleLineHeight);
                ObjectField(fieldLayout, property);

                DrawCreationContent(position, property);
            }

            property.serializedObject.ApplyModifiedProperties();
            isDrawn = true;
            EndProperty();
        }


        

        #endregion

        #region Object Serialization GUI Drawer

        private static void DrawPropertyField(Rect position, SerializedProperty property)
        {
            Rect foldoutLayout = new Rect(position.x, position.y, labelWidth, singleLineHeight);
            DrawSerializationFoldOut(foldoutLayout, property);

            float deleteButtonOffset = (IsNestedAsset(property) ? DELETE_BUTTON_WIDTH : 0);
            Rect propertyFieldLayout = new Rect(position)
            {
                x = position.x + labelWidth - IndentOffset,
                width = position.width - labelWidth - deleteButtonOffset - HORIZONTAL_SPACING +
                        IndentOffset,
                height = singleLineHeight
            };
            DrawAssignedPropertyField(propertyFieldLayout, property);

            Rect deletebuttonLayout = new Rect(propertyFieldLayout)
            {
                x = position.x + position.width - DELETE_BUTTON_WIDTH,
                width = DELETE_BUTTON_WIDTH,
            };
            if (IsNestedAsset(property)) DrawDeleteButton(deletebuttonLayout, property);
        }


        private static void DrawSerializationFoldOut(Rect position, SerializedProperty property)
        {
            if (IsThereAnyVisibileProperty(property))
                property.isExpanded = Foldout(
                    position,
                    property.isExpanded,
                    property.displayName,
                    true);
            else
            {
                LabelField(position, property.displayName);
                property.isExpanded = false;
            }
        }

        private static void DrawAssignedPropertyField(Rect position, SerializedProperty property)
        {
            PropertyField(position, property, GUIContent.none, true);
        }

        private static void DrawDeleteButton(Rect position, SerializedProperty property)
        {
            if (!GUI.Button(position, "Delete")) return;
            DestructInternalPropertyValue(property);
        }

        private void DrawSerializationContent(Rect position, SerializedProperty property)
        {
            Rect boxLayout = new Rect(position)
            {
                x = position.x + IndentOffset,
                width = position.width - IndentOffset,
                height = position.height - standardVerticalSpacing
            };
            DrawBoundingBox(boxLayout);

            indentLevel++;

            Rect nameFieldLayout = new Rect(position)
            {
                y = position.y + standardVerticalSpacing,
                width = position.width - HORIZONTAL_SPACING - DELETE_BUTTON_WIDTH,
                height = singleLineHeight
            };
            valueObjectName = TextField(nameFieldLayout, "Name: ", valueObjectName);

            Rect renameButtonRect = new Rect(nameFieldLayout)
            {
                x = nameFieldLayout.x + nameFieldLayout.width + HORIZONTAL_SPACING,
                width = DELETE_BUTTON_WIDTH,
            };
            DrawRenameButton(renameButtonRect, property);

            Rect serializeFieldLayout = new Rect(position)
            {
                y = position.y + singleLineHeight + standardVerticalSpacing + standardVerticalSpacing
            };
            DrawSerializedFields(serializeFieldLayout, property);

            indentLevel--;
        }

        private void DrawRenameButton(Rect position, SerializedProperty property)
        {
            if (!GUI.Button(position, "Rename")) return;
            RenamePropertyReference(property, valueObjectName);
        }

        private static void DrawSerializedFields(Rect position, SerializedProperty property)
        {
            ScriptableObject data = (ScriptableObject) property.objectReferenceValue;
            SerializedObject serializedObject = new SerializedObject(data);

            SerializedProperty prop = serializedObject.GetIterator();
            float y = position.y;

            if (!prop.NextVisible(true)) return;

            // Iterate over all the values and draw them
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
        }

        #endregion

        #region New Object Creation GUI Drawer

        private static float CreationPropertyHeight(SerializedProperty property)
        {
            float totalHeight = singleLineHeight + standardVerticalSpacing * 2;
            if (!property.isExpanded) return totalHeight;
            totalHeight += (singleLineHeight + standardVerticalSpacing) * 3 + singleLineHeight;
            return totalHeight;
        }

        private static void DrawCreationFoldOut(Rect position, SerializedProperty property)
        {
            property.isExpanded = Foldout(
                position,
                property.isExpanded,
                "Create",
                true);
        }

        private void DrawCreationContent(Rect position, SerializedProperty property)
        {
            Rect boxLayout = new Rect(position)
            {
                x = position.x + IndentOffset,
                y = position.y + singleLineHeight + standardVerticalSpacing,
                width = position.width - IndentOffset,
                height = position.height - singleLineHeight - standardVerticalSpacing
            };
            DrawBoundingBox(boxLayout);

            indentLevel++;

            Rect foldoutLayout = new Rect(
                position.x,
                position.y + singleLineHeight + standardVerticalSpacing + standardVerticalSpacing,
                labelWidth,
                singleLineHeight);
            DrawCreationFoldOut(foldoutLayout, property);
            Rect creationContentLayout = new Rect()
            {
                x = foldoutLayout.x + IndentOffset,
                y = foldoutLayout.y,
                width = position.width - IndentOffset,
                height = (singleLineHeight + standardVerticalSpacing) * 3
            };
            if (property.isExpanded) DrawCreationFields(creationContentLayout, property);

            indentLevel--;
        }

        private void DrawCreationFields(Rect position, SerializedProperty property)
        {
            Rect typeSelectionRect = new Rect()
            {
                x = position.x + labelWidth - INDENT_WIDTH * indentLevel * 2,
                y = position.y,
                width = position.width - labelWidth + INDENT_WIDTH * indentLevel * 2,
                height = singleLineHeight,
            };
            DrawTypeSelectionDropdown(typeSelectionRect);
            Rect nameFieldRect = new Rect(position)
            {
                x = position.x - INDENT_WIDTH * indentLevel,
                y = position.y + singleLineHeight + standardVerticalSpacing,
                width = position.width + INDENT_WIDTH * indentLevel,
                height = singleLineHeight
            };
            valueObjectName = TextField(nameFieldRect, "Name: ", valueObjectName);
            Rect buttonExternalRect = new Rect(position)
            {
                y = position.y + 2 * (singleLineHeight + standardVerticalSpacing),
                width = position.width,
                height = singleLineHeight
            };
            DrawCreateExternalButton(buttonExternalRect, property);
            Rect buttonInternalRect = new Rect(buttonExternalRect)
            {
                y = position.y + 3 * (singleLineHeight + standardVerticalSpacing),
            };
            DrawCreateInternalButton(buttonInternalRect, property);
        }

        private void DrawTypeSelectionDropdown(Rect position)
        {
            typeSelectionIndex = Popup(position, typeSelectionIndex, TypeStringOptions);
        }

        private void DrawCreateExternalButton(Rect position, SerializedProperty property)
        {
            if (!GUI.Button(position, "Create New Asset File")) return;

            string selectedAssetPath = "Assets";

            if (property.serializedObject.targetObject is MonoBehaviour behaviour)
            {
                MonoScript ms = MonoScript.FromMonoBehaviour(behaviour);
                selectedAssetPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(ms));
            }

            Type type = SelectedObjectType;
            property.objectReferenceValue = CreateAssetWithSavePrompt(type, valueObjectName, selectedAssetPath);
        }

        private void DrawCreateInternalButton(Rect position, SerializedProperty property)
        {
            if (!GUI.Button(position, "Insert Asset")) return;

            Type type = SelectedObjectType;
            string assetPath = PropertyAssetPath(property);
            string assetName = string.IsNullOrEmpty(valueObjectName)
                ? PropertyHierarchyName(property)
                : valueObjectName;

            property.objectReferenceValue = CreateInternalAsset(type, assetName, assetPath);
        }

        #endregion

        #region Look and Feel

        private static void DrawBoundingBox(Rect boxLayout)
        {
            GUI.Box(boxLayout, "", boxStyle);
        }

        private static GUIStyle boxStyle = null;

        private static void InitStyles()
        {
            if (boxStyle != null) return;
            boxStyle = new GUIStyle(GUI.skin.box)
                {border = new RectOffset(2, 2, 2, 2)};
        }

        #endregion
    }
}