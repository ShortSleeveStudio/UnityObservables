using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CustomEditor(typeof(Observable<>), true)]
    public class ObservableGenericEditor : EventBusEditor
    {
        #region Constants

        private const string ScriptField = "m_Script";
        private const string RaiseMethod = "RaiseObservableFromEditor";
        private const string EditorValueField = "_editorValue";
        private const string RuntimeValueField = "_runtimeValue";

        #endregion

        #region Cached

        private MethodInfo _raiseMethodInfo;

        private static readonly GUIContent ValueLabel = new("Value");

        #endregion

        #region Unity Lifecycle

        protected override void OnEnable()
        {
            base.OnEnable();

            // Cache fields needed for reflection
            if (_raiseMethodInfo == null)
            {
                _raiseMethodInfo = GetMethod(target.GetType(), RaiseMethod,
                    BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }

        #endregion


        #region Draw Methods

        protected override bool DidCustomizeProperty(SerializedProperty serializedProperty)
        {
            if (serializedProperty.propertyPath == ScriptField)
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

            if (serializedProperty.propertyPath == RaisePayloadField)
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
                _raiseMethodInfo.Invoke(target, null);
            }
        }

        #endregion
    }
}