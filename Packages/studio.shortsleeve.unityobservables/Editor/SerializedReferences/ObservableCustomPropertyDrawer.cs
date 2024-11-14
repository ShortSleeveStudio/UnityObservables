using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    [CustomPropertyDrawer(typeof(Observable<>), true)]
    public class ObservableCustomPropertyDrawer : EventBaseCustomPropertyDrawer
    {
        #region Draw Methods

        protected override bool DidCustomizeProperty(SerializedProperty serializedProperty)
        {
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
    }
}
