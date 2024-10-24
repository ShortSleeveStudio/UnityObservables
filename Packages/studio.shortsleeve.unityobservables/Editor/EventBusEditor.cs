using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CustomEditor(typeof(EventBus<>), true)]
    public class EventBusEditor : Editor
    {
        #region Constants

        private const string IDField = "ID";
        private const string RaiseMethod = "RaiseEventFromEditor";
        private const string EventHandlerListField = "_eventHandlerList";
        protected const string RaisePayloadField = "raisePayload";

        #endregion

        #region Cached

        private GUIStyle _labelColorNormal;
        private GUIStyle _labelColorError;
        private FieldInfo _eventHandlerListField;
        private MethodInfo _raiseMethodInfo;
        private SerializedProperty _raisePayloadProperty;

        private static readonly GUIContent PayloadLabel = new("Payload");

        #endregion

        #region Unity Lifecycle

        protected virtual void OnEnable()
        {
            // Cache fields needed for reflection
            if (_eventHandlerListField == null)
            {
                _eventHandlerListField = GetField(target.GetType(), EventHandlerListField,
                    BindingFlags.Instance | BindingFlags.NonPublic);
            }

            if (_raiseMethodInfo == null)
            {
                _raiseMethodInfo = GetMethod(target.GetType(), RaiseMethod,
                    BindingFlags.Instance | BindingFlags.NonPublic);
            }

            // Cache properties needed for drawing properties out of order
            if (_raisePayloadProperty == null)
            {
                _raisePayloadProperty = serializedObject.FindProperty(RaisePayloadField);
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
            DrawEventHandlers();
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

        private void DrawEventHandlers()
        {
            if (_eventHandlerListField == null)
            {
                EditorGUILayout.HelpBox(
                    "The name of the subscriber list field in EventBus.cs was renamed without a corresponding name change in its custom editor.",
                    MessageType.Error);
                return;
            }

            if (!Application.isPlaying)
                return;


            // Subscriber list header
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Subscribers", EditorStyles.boldLabel);

            // Grab event handler list
            IList eventHandlerList = (IList)_eventHandlerListField.GetValue(target) ?? Array.Empty<object>();

            // Explain if the list is empty
            if (eventHandlerList.Count == 0)
            {
                EditorGUILayout.HelpBox("There are no subscribers.", MessageType.Info);
                return;
            }

            // Draw subscribers list
            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int i = 0; i < eventHandlerList.Count; i++)
                DrawEventHandlerPingButton(eventHandlerList[i]);
            EditorGUILayout.EndVertical();
        }

        private void DrawEventHandlerPingButton(object eventHandler)
        {
            PropertyInfo handlerPropertyInfo =
                eventHandler.GetType().GetProperty(IDField, BindingFlags.Instance | BindingFlags.Public);
            if (handlerPropertyInfo == null)
            {
                EditorGUILayout.HelpBox(
                    "The name of the EventHandlerWrapper ID field in EventHandlerWrapper.cs was renamed without a corresponding name change in its custom editor.",
                    MessageType.Error);
                return;
            }

            object handler = handlerPropertyInfo.GetValue(eventHandler, null);
            string typeName = "<No Type>";
            string objectName = "<No Object>";
            string functionName = "<No Function>";
            UnityEngine.Object uObject = null;
            if (handler is UnityEngine.Object @object)
            {
                uObject = @object;
                typeName = @object.GetType().Name;
            }
            else if (handler is Delegate @delegate)
            {
                uObject = @delegate.Target as UnityEngine.Object;
                typeName = @delegate.Target.GetType().Name;
                functionName = @delegate.Method.Name;
            }

            if (uObject)
                objectName = uObject.name;

            EditorGUI.BeginDisabledGroup(!uObject);
            if (GUILayout.Button($"[{objectName}] {typeName}.{functionName}"))
                EditorGUIUtility.PingObject(uObject);
            EditorGUI.EndDisabledGroup();
        }

        protected virtual void DrawRaiseButton()
        {
            if (!Application.isPlaying)
                return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Raise", EditorStyles.boldLabel);

            // Draw payload field if needed
            if (target is not EventVoid)
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
                _raiseMethodInfo.Invoke(target, null);
            }
        }

        protected virtual bool DidCustomizeProperty(SerializedProperty serializedProperty)
        {
            return serializedProperty.propertyPath == RaisePayloadField;
        }

        #endregion

        #region Helpers

        protected static FieldInfo GetField(Type @type, string name, BindingFlags flags)
        {
            if (@type == null)
                return null;

            FieldInfo fieldInfo = @type.GetField(name, flags);
            if (fieldInfo != null)
                return fieldInfo;

            return GetField(@type.BaseType, name, flags);
        }

        protected static MethodInfo GetMethod(Type @type, string name, BindingFlags flags)
        {
            if (@type == null)
                return null;

            MethodInfo methodInfo = @type.GetMethod(name, flags);
            if (methodInfo != null)
                return methodInfo;

            return GetMethod(@type.BaseType, name, flags);
        }

        #endregion
    }
}