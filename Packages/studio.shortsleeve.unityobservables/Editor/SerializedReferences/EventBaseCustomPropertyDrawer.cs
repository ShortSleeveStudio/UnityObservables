using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CustomPropertyDrawer(typeof(EventBase<>), true)]
    public class EventBaseCustomPropertyDrawer : PropertyDrawer
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


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Field Title
            EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);

            // Grab target and cache reflection variables
            object target = property.boxedValue;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            CacheVariables(property, target);
            DrawProperties(property);
            Common.DrawEventHandlers(ref _eventHandlerPropertyInfo, _eventHandlerListField, _eventField, target);
            DrawRaiseButton(property, target);
            EditorGUILayout.EndVertical();

            EditorGUI.EndProperty();
        }

        #region Draw Methods

        private void CacheVariables(SerializedProperty property, object target)
        {
            // Cache properties needed for reflection 
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
                _raisePayloadProperty = property.FindProperty(Common.RaisePayloadField);
            }

            // Styles
            if (_labelColorNormal == null)
            {
                _labelColorNormal = new();
                _labelColorNormal.normal.textColor = Color.white;
            }

            if (_labelColorError == null)
            {
                _labelColorError = new();
                _labelColorError.normal.textColor = Color.red;
            }
        }

        private void DrawProperties(SerializedProperty property)
        {
            EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            property.serializedObject.UpdateIfRequiredOrScript();

            foreach (SerializedProperty child in property.Copy())
            {
                if (!DidCustomizeProperty(child))
                    EditorGUILayout.PropertyField(child);
            }

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        protected virtual void DrawRaiseButton(SerializedProperty property, object target)
        {
            if (!Application.isPlaying)
                return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Raise", EditorStyles.boldLabel);

            // Draw payload field if needed
            if (target is not EventVoid)
            {
                EditorGUI.BeginChangeCheck();
                property.serializedObject.UpdateIfRequiredOrScript();
                EditorGUILayout.PropertyField(_raisePayloadProperty, PayloadLabel);
                property.serializedObject.ApplyModifiedProperties();
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
            if (serializedProperty.propertyPath.EndsWith(Common.ScriptField))
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedProperty);
                EditorGUI.EndDisabledGroup();
                return true;
            }

            if (serializedProperty.propertyPath.EndsWith(Common.RaisePayloadField))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}