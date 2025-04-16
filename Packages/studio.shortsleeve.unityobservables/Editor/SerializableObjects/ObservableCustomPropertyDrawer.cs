using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityObservables
{
    [CustomPropertyDrawer(typeof(Observable<>), true)]
    public class ObservableCustomPropertyDrawer : EventBaseCustomPropertyDrawer
    {
        #region Constants

        private const string EditorValueField = "editorValue";
        private const string RuntimeValueField = "runtimeValue";

        #endregion

        #region Cached

        private static readonly GUIContent ValueLabel = new("Value");
        private MethodInfo EnsureInitializedMethodInfo;

        #endregion

        #region Draw Methods

        protected override bool DidCustomizeProperty(
            SerializedProperty serializedProperty,
            object target
        )
        {
            if (serializedProperty.propertyPath.EndsWith(Common.ScriptField))
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedProperty);
                EditorGUI.EndDisabledGroup();
                return true;
            }

            if (serializedProperty.propertyPath.EndsWith(EditorValueField))
            {
                if (!Application.isPlaying)
                    EditorGUILayout.PropertyField(serializedProperty, ValueLabel);
                return true;
            }

            if (serializedProperty.propertyPath.EndsWith(RuntimeValueField))
            {
                if (Application.isPlaying)
                {
                    EnsureInitializedMethodInfo.Invoke(target, null);
                    EditorGUILayout.PropertyField(serializedProperty, ValueLabel);
                }
                return true;
            }

            if (serializedProperty.propertyPath.EndsWith(Common.RaisePayloadField))
            {
                return true;
            }

            return false;
        }

        protected override void DrawRaiseButton(SerializedProperty property, object target)
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

        #region Protected API

        protected override void CacheVariables(SerializedProperty property, object target)
        {
            base.CacheVariables(property, target);
            if (EnsureInitializedMethodInfo == null)
            {
                EnsureInitializedMethodInfo = Common.GetMethod(
                    target.GetType(),
                    Common.EnsureInitializedMethod,
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
            }
        }

        #endregion
    }
}
