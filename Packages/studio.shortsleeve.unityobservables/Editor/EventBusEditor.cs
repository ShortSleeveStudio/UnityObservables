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
        private const string EventHandlerListField = "_eventHandlerList";

        #endregion

        #region Cached

        private GUIStyle _labelColorNormal;
        private GUIStyle _labelColorError;
        private FieldInfo _eventHandlerListField;

        #endregion

        #region Unity Lifecycle

        void OnEnable()
        {
            // Cache fields needed for reflection
            if (_eventHandlerListField == null)
            {
                _eventHandlerListField = GetField(target.GetType(), EventHandlerListField,
                    BindingFlags.Instance | BindingFlags.NonPublic);
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
        }

        #endregion

        #region Draw Methods

        protected virtual void DrawProperties()
        {
            DrawDefaultInspector();
        }

        void DrawEventHandlers()
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
            EditorGUILayout.LabelField("Subscribers");

            // Grab event handler list
            IList eventHandlerList = (IList)_eventHandlerListField.GetValue(target) ?? Array.Empty<object>();

            // Explain if the list if empty
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

        #endregion

        #region Helpers

        static FieldInfo GetField(Type @type, string name, BindingFlags flags)
        {
            if (@type == null)
                return null;

            FieldInfo fieldInfo = @type.GetField(name, flags);
            if (fieldInfo != null)
                return fieldInfo;

            return GetField(@type.BaseType, name, flags);
        }

        #endregion
    }
}