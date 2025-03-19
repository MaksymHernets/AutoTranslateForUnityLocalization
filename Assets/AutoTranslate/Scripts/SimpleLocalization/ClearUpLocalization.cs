using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;

namespace EqualchanceGames.Tools.AutoTranslate
{
    public static class ClearUpLocalization
    {
        public static void Execute(SearchTextParameters parameters, Scene scene)
        {
            GameObject[] gameObjects = scene.GetRootGameObjects();

            RemoveAllLocalization(parameters, gameObjects);
        }

        public static void Execute(SearchTextParameters parameters, GameObject prefab)
        {
            RemoveAllLocalizationPrefab(parameters, prefab);
        }

        public static void RemoveAllLocalizationPrefab(SearchTextParameters parameters, GameObject gameObject)
        {
            if (parameters.ListSearchComponents.ContainsKey("LocalizeStringEvent")) Remove_LocalizeStringEvent(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
            if (parameters.ListSearchComponents.ContainsKey("LocalizeSpriteEvent")) RemoveComponents<LocalizeSpriteEvent>(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
            if (parameters.ListSearchComponents.ContainsKey("LocalizeTextureEvent")) RemoveComponents<LocalizeTextureEvent>(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
            if (parameters.ListSearchComponents.ContainsKey("LocalizeAudioClipEvent")) RemoveComponents<LocalizeAudioClipEvent>(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
            if (parameters.ListSearchComponents.ContainsKey("LocalizedGameObjectEvent")) RemoveComponents<LocalizedGameObjectEvent>(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
        }

        public static void RemoveAllLocalization(SearchTextParameters parameters, GameObject[] gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(gameObject) && parameters.SkipPrefab == true) continue;

                EditorUtility.SetDirty(gameObject);
                if (parameters.ListSearchComponents.ContainsKey("LocalizeStringEvent") ) Remove_LocalizeStringEvent(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
                if (parameters.ListSearchComponents.ContainsKey("LocalizeSpriteEvent")) RemoveComponents<LocalizeSpriteEvent>(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
                if (parameters.ListSearchComponents.ContainsKey("LocalizeTextureEvent")) RemoveComponents<LocalizeTextureEvent>(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
                if (parameters.ListSearchComponents.ContainsKey("LocalizeAudioClipEvent")) RemoveComponents<LocalizeAudioClipEvent>(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
                if (parameters.ListSearchComponents.ContainsKey("LocalizedGameObjectEvent")) RemoveComponents<LocalizedGameObjectEvent>(gameObject, parameters.IsRemoveMiss_StringTable, parameters.SkipPrefab);
            }
        }

        public static void Remove_LocalizeStringEvent(GameObject gameObject, bool RemoveMissRefer, bool SkipPrefab)
        {
            LocalizeStringEvent[] localizeStringEvents = gameObject.GetComponentsInChildren<LocalizeStringEvent>();

            if (RemoveMissRefer == true)
            {
                RemoveMiss_LocalizeStringEvent(localizeStringEvents.ToList());
            }
            else
            {
                foreach (LocalizeStringEvent localizeStringEvent in localizeStringEvents)
                {
                    if (PrefabUtility.IsPartOfAnyPrefab(localizeStringEvent.gameObject) && SkipPrefab == true) continue;

                    Undo.DestroyObjectImmediate(localizeStringEvent);
                    //UnityEngine.Object.DestroyImmediate(localizeStringEvent, true);
                }
            }
        }

        public static void RemoveComponents<T>(GameObject gameObject, bool RemoveMissRefer, bool SkipPrefab) where T : Component
        {
            T[] localizeStringEvents = gameObject.GetComponentsInChildren<T>();

            foreach (T localizeStringEvent in localizeStringEvents)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(localizeStringEvent.gameObject) && SkipPrefab == true) continue;

                Undo.DestroyObjectImmediate(localizeStringEvent);
                //UnityEngine.Object.DestroyImmediate(localizeStringEvent, true);
            }
        }

        public static void RemoveMiss_LocalizeStringEvent(List<LocalizeStringEvent> list)
        {
            var locales = LocalizationEditorSettings.GetLocales();
            var tableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (LocalizeStringEvent localizeStringEvent in list)
            {
                var m_SelectedTableCollection = tableCollections.FirstOrDefault(t => t.TableCollectionName == localizeStringEvent.StringReference.TableReference);
                if (localizeStringEvent.StringReference.TableEntryReference.ReferenceType == TableEntryReference.Type.Name)
                {
                    SharedTableData.SharedTableEntry m_SelectedEntry = m_SelectedTableCollection.SharedData.GetEntry(localizeStringEvent.StringReference.TableEntryReference.Key);
                    if (m_SelectedEntry == null)
                    {
                        Undo.DestroyObjectImmediate(localizeStringEvent);
                        //UnityEngine.Object.DestroyImmediate(localizeStringEvent, true);
                    }
                }
                else
                {
                    SharedTableData.SharedTableEntry m_SelectedEntry = m_SelectedTableCollection.SharedData.GetEntry(localizeStringEvent.StringReference.TableEntryReference.KeyId);
                    if (m_SelectedEntry == null)
                    {
                        Undo.DestroyObjectImmediate(localizeStringEvent);
                        //UnityEngine.Object.DestroyImmediate(localizeStringEvent, true);
                    }
                }
            }
        }
    }

    public class ClearUpLocalizationParameters
    {

    }
}