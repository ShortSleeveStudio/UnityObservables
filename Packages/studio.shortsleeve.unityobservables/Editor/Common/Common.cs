using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    public static class Common
    {
        #region Constants

        const string IDField = "ID";
        public const string EventField = "Event";
        public const string ScriptField = "m_Script";
        public const string RaiseMethod = "RaiseFromEditor";
        public const string EventHandlerListField = "_eventHandlerList";
        public const string RaisePayloadField = "raisePayload";

        #endregion


        #region Draw Helpers

        public static void DrawEventHandlers(ref PropertyInfo eventHandlerPropertyInfo,
            FieldInfo eventHandlerListField,
            FieldInfo eventField, object target)
        {
            if (eventHandlerListField == null)
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
            IList eventHandlerList = (IList)eventHandlerListField.GetValue(eventField.GetValue(target)) ??
                                     Array.Empty<object>();

            // Explain if the list is empty
            if (eventHandlerList.Count == 0)
            {
                EditorGUILayout.HelpBox("There are no subscribers.", MessageType.Info);
                return;
            }

            // Draw subscribers list
            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int i = 0; i < eventHandlerList.Count; i++)
                DrawEventHandlerPingButton(ref eventHandlerPropertyInfo, eventHandlerList[i]);
            EditorGUILayout.EndVertical();
        }

        private static void DrawEventHandlerPingButton(ref PropertyInfo eventHandlerPropertyInfo, object eventHandler)
        {
            // Cache EventHandlerWrapper property info
            if (eventHandlerPropertyInfo == null)
            {
                eventHandlerPropertyInfo = eventHandler.GetType()
                    .GetProperty(IDField, BindingFlags.Instance | BindingFlags.Public);
            }

            if (eventHandlerPropertyInfo == null)
            {
                EditorGUILayout.HelpBox(
                    "The name of the EventHandlerWrapper ID field in EventHandlerWrapper.cs was renamed without a corresponding name change in its custom editor.",
                    MessageType.Error);
                return;
            }

            object handler = eventHandlerPropertyInfo.GetValue(eventHandler, null);
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

        #region General Helpers

        public static FieldInfo GetField(Type @type, string name, BindingFlags flags)
        {
            if (@type == null)
                return null;

            FieldInfo fieldInfo = @type.GetField(name, flags);
            if (fieldInfo != null)
                return fieldInfo;

            return GetField(@type.BaseType, name, flags);
        }

        public static MethodInfo GetMethod(Type @type, string name, BindingFlags flags)
        {
            if (@type == null)
                return null;

            MethodInfo methodInfo = @type.GetMethod(name, flags);
            if (methodInfo != null)
                return methodInfo;

            return GetMethod(@type.BaseType, name, flags);
        }

        public static SerializedProperty FindProperty(this SerializedProperty property, string name)
        {
            foreach (SerializedProperty child in property.Copy()) 
            {
                if (child.name == name)
                    return child;
            }

            return null;
        }

        #endregion
    }
}