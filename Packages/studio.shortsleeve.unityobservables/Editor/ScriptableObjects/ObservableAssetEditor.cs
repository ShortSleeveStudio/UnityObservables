using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CustomEditor(typeof(ObservableAsset<>), true)]
    public class ObservableAssetEditor : EventAssetBaseEditor
    {
        #region Constants

        private const string EditorValueField = "editorValue";
        private const string RuntimeValueField = "runtimeValue";

        #endregion

        #region Cached

        private static readonly GUIContent ValueLabel = new("Value");

        #endregion

        #region Draw Methods

        protected override bool DidCustomizeProperty(SerializedProperty serializedProperty)
        {
            if (serializedProperty.propertyPath == Common.ScriptField)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedProperty);
                EditorGUI.EndDisabledGroup();
                return true;
            }

            if (serializedProperty.propertyPath == EditorValueField)
            {
                if (!Application.isPlaying)
                    EditorGUILayout.PropertyField(serializedProperty, ValueLabel);
                return true;
            }

            if (serializedProperty.propertyPath == RuntimeValueField)
            {
                if (Application.isPlaying)
                    EditorGUILayout.PropertyField(serializedProperty, ValueLabel);
                return true;
            }

            if (serializedProperty.propertyPath == Common.RaisePayloadField)
            {
                return true;
            }

            return false;
        }

        protected override void DrawRaiseButton()
        {
            if (!Application.isPlaying)
                return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Raise", EditorStyles.boldLabel);

            if (GUILayout.Button("Raise"))
            {
                RaiseMethodInfo.Invoke(target, null);
            }
        }

        #endregion
    }
}