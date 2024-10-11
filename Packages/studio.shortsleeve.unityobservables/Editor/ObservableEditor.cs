using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CustomEditor(typeof(Observable<>), true)]
    public class ObservableGenericEditor : EventBusEditor
    {
        #region Constants

        private const string ScriptField = "m_Script";
        private const string EditorValueField = "_editorValue";
        private const string RuntimeValueField = "_runtimeValue";

        #endregion

        #region Cached

        private static readonly GUIContent ValueLabel = new("Value");

        #endregion

        #region Draw Methods

        protected override void DrawProperties()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty serializedProperty = serializedObject.GetIterator();
            serializedProperty.Next(true);
            while (serializedProperty.NextVisible(false))
            {
                if (serializedProperty.propertyPath == ScriptField)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(serializedProperty);
                    EditorGUI.EndDisabledGroup();
                    continue;
                }

                if (serializedProperty.propertyPath == EditorValueField)
                {
                    if (!Application.isPlaying)
                        EditorGUILayout.PropertyField(serializedProperty, ValueLabel);
                    continue;
                }

                if (serializedProperty.propertyPath == RuntimeValueField)
                {
                    if (Application.isPlaying)
                        EditorGUILayout.PropertyField(serializedProperty, ValueLabel);
                    continue;
                }

                EditorGUILayout.PropertyField(serializedProperty);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        #endregion
    }
}