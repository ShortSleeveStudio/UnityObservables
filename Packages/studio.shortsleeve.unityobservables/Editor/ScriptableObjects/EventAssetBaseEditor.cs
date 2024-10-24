using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CustomEditor(typeof(EventAssetBase<>), true)]
    public class EventAssetBaseEditor : Editor
    {
        #region Cached

        private GUIStyle _labelColorNormal;
        private GUIStyle _labelColorError;
        private FieldInfo _eventField;
        private FieldInfo _eventHandlerListField;
        private PropertyInfo _eventHandlerPropertyInfo;
        private SerializedProperty _raisePayloadProperty;

        protected MethodInfo RaiseMethodInfo;

        private static readonly GUIContent PayloadLabel = new("Payload");

        #endregion

        #region Unity Lifecycle

        protected virtual void OnEnable()
        {
            // Cache fields needed for reflection
            if (_eventField == null)
            {
                _eventField = Common.GetField(target.GetType(), Common.EventField,
                    BindingFlags.Instance | BindingFlags.NonPublic);
            }

            if (_eventField != null && _eventHandlerListField == null)
            {
                _eventHandlerListField = Common.GetField(_eventField.GetValue(target).GetType(),
                    Common.EventHandlerListField,
                    BindingFlags.Instance | BindingFlags.NonPublic);
            }

            if (RaiseMethodInfo == null)
            {
                RaiseMethodInfo = Common.GetMethod(target.GetType(), Common.RaiseMethod,
                    BindingFlags.Instance | BindingFlags.NonPublic);
            }

            // Cache properties needed for drawing properties out of order
            if (_raisePayloadProperty == null)
            {
                _raisePayloadProperty = serializedObject.FindProperty(Common.RaisePayloadField);
            }


            // Styles
            _labelColorNormal = new();
            _labelColorNormal.normal.textColor = Color.white;
            _labelColorError = new();
            _labelColorError.normal.textColor = Color.red;
        }

        #endregion

        #region IMGUI Lifecycle

        public override void OnInspectorGUI()
        {
            DrawProperties();
            Common.DrawEventHandlers(ref _eventHandlerPropertyInfo, _eventHandlerListField, _eventField, target);
            DrawRaiseButton();
        }

        #endregion

        #region Draw Methods

        private void DrawProperties()
        {
            EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty serializedProperty = serializedObject.GetIterator();
            serializedProperty.Next(true);
            while (serializedProperty.NextVisible(false))
            {
                if (!DidCustomizeProperty(serializedProperty))
                    EditorGUILayout.PropertyField(serializedProperty);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }


        protected virtual void DrawRaiseButton()
        {
            if (!Application.isPlaying)
                return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Raise", EditorStyles.boldLabel);

            // Draw payload field if needed
            if (target is not EventAssetVoid)
            {
                EditorGUI.BeginChangeCheck();
                serializedObject.UpdateIfRequiredOrScript();
                EditorGUILayout.PropertyField(_raisePayloadProperty, PayloadLabel);
                serializedObject.ApplyModifiedProperties();
                EditorGUI.EndChangeCheck();
            }

            // Draw raise button
            if (GUILayout.Button("Raise"))
            {
                RaiseMethodInfo.Invoke(target, null);
            }
        }

        protected virtual bool DidCustomizeProperty(SerializedProperty serializedProperty)
        {
            if (serializedProperty.propertyPath == Common.ScriptField)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedProperty);
                EditorGUI.EndDisabledGroup();
                return true;
            }

            if (serializedProperty.propertyPath == Common.RaisePayloadField)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}