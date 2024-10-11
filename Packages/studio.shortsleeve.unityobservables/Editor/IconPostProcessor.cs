using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Studio.ShortSleeve.UnityObservables
{
    internal class IconPostProcessor : AssetPostprocessor
    {
        private const string TexturePath = "Packages/studio.shortsleeve.unityobservables/Editor/Textures";
        private const string EventIconPath = TexturePath + "/EventIcon.png";
        private const string ObservableIconPath = TexturePath + "/ObservableIcon.png";

        #region Unity Lifecycle

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            Texture2D eventIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(EventIconPath);
            Texture2D observableIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(ObservableIconPath);
            List<MonoImporter> events = new();
            List<MonoImporter> observables = new();
            foreach (string importedAssetPath in importedAssets)
            {
                if (AssetImporter.GetAtPath(importedAssetPath) is MonoImporter monoImporter)
                {
                    MonoScript monoScript = monoImporter.GetScript();
                    if (IsSubclassOfRawGeneric(monoScript.GetClass(), typeof(Observable<>)))
                    {
                        if (monoImporter.GetIcon() != observableIcon)
                        {
                            observables.Add(monoImporter);
                        }
                    }

                    else if (IsSubclassOfRawGeneric(monoScript.GetClass(), typeof(EventBus<>)))
                    {
                        if (monoImporter.GetIcon() != eventIcon)
                        {
                            events.Add(monoImporter);
                        }
                    }
                }
            }

            if (events.Count > 0 || observables.Count > 0)
            {
                EditorApplication.delayCall += () =>
                {
                    for (int i = 0; i < events.Count; i++)
                    {
                        events[i].SetIcon(eventIcon);
                        events[i].SaveAndReimport();

                    }

                    for (int i = 0; i < observables.Count; i++)
                    {
                        observables[i].SetIcon(observableIcon);
                        observables[i].SaveAndReimport();
                    }
                };
            }
        }

        #endregion

        #region Helpers

        private static bool IsSubclassOfRawGeneric(Type subType, Type baseType)
        {
            while (subType != null && subType != typeof(object))
            {
                Type cur = subType.IsGenericType ? subType.GetGenericTypeDefinition() : subType;
                if (baseType == cur)
                    return true;

                subType = subType.BaseType;
            }

            return false;
        }

        #endregion
    }
}