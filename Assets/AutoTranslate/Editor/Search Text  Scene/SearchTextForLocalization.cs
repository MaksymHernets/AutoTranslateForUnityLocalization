using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Localization.Tables.SharedTableData;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
	public static class SearchTextForLocalization 
    {
        public static string Search(SearchTextParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.NameTable)) return "nameTable is null";

            GameObject[] gameObjects = SimpleDatabaseProject.GetGameObjects(parameters.Scene.name);
            if (gameObjects.Length == 0) return "gameObjects is 0";

            List<Text> listsText = SimpleDatabaseProject.GetGameObjectsText(gameObjects);
            if (listsText.Count == 0) return "texts is 0";

            SharedTableData sharedTable = SimpleInterfaceStringTable.GetSharedTable(parameters.NameTable);

            if (sharedTable != null) SimpleInterfaceStringTable.Clear_StringTable(sharedTable);
            else sharedTable = SimpleInterfaceStringTable.AddStringTable(parameters.NameTable);

            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            StringTable stringTable = default(StringTable);

            foreach (var text in listsText)
            {
                if (parameters.SkipPrefab == true && PrefabUtility.IsPartOfAnyPrefab(text.gameObject)) continue;

                if ( text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent) )
                    localizeStringEvent = text.gameObject.GetComponent<LocalizeStringEvent>();
                else
                    localizeStringEvent = text.gameObject.AddComponent<LocalizeStringEvent>();

                sharedTableEntry = SharedTableData_AddEntry(sharedTable, text);

                stringTable = SimpleInterfaceStringTable.GetStringTable(sharedTable, parameters.SourceLocale);
                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                LocalizeStringEvent_ClearUpdate(localizeStringEvent);

                LocalizeStringEvent_SignTable(localizeStringEvent, sharedTable.TableCollectionName, sharedTableEntry.Key);

                LocalizeStringEvent_SignUpdate(localizeStringEvent, text);
            }

            EditorSceneManager.SaveScene(parameters.Scene);

            return "Completed";
        }

        private static SharedTableEntry SharedTableData_AddEntry(SharedTableData sharedTable, Text text)
		{
            string name = String.Format("[{0}][{1}]", text.gameObject.name, text.gameObject.transform.parent?.name);
            int variants = 1;
            while (sharedTable.Contains(name))
            {
                name = String.Format("[{0}][{1}][{2}]", text.gameObject.name, text.gameObject.transform.parent?.name, variants);
                ++variants;
            }
            SharedTableEntry sharedTableEntry = sharedTable.AddKey(name);
            return sharedTableEntry;
        }

        private static void LocalizeStringEvent_ClearUpdate(LocalizeStringEvent localizeStringEvent)
		{
            while (localizeStringEvent.OnUpdateString.GetPersistentEventCount() != 0)
            {
                UnityEventTools.RemovePersistentListener(localizeStringEvent.OnUpdateString, 0);
            }
        }

        private static void LocalizeStringEvent_SignTable(LocalizeStringEvent localizeStringEvent, string TableCollectionName, string Key)
		{
            LocalizedString localizedString = new LocalizedString();
            localizedString.SetReference(TableCollectionName, Key);
            localizeStringEvent.StringReference = localizedString;
        }

        private static void LocalizeStringEvent_SignUpdate(LocalizeStringEvent localizeStringEvent, Text text)
		{
            var targetinfo = UnityEvent.GetValidMethodInfo(text, "set_text", new Type[] { typeof(string) });
            if (targetinfo != null)
            {
                UnityAction<string> action = Delegate.CreateDelegate(typeof(UnityAction<string>), text, targetinfo, false) as UnityAction<string>;
                UnityEventTools.AddPersistentListener(localizeStringEvent.OnUpdateString, action);
                EditorUtility.SetDirty(text.gameObject);
            }
        }

        public static StatusLocalizationScene CheckTextAboutLocalization(Scene scene)
        {
            StatusLocalizationScene statusLocalizationScene = new StatusLocalizationScene();

            GameObject[] gameObjects = SimpleDatabaseProject.GetGameObjects(scene.name);
            List<Text> texts = SimpleDatabaseProject.GetGameObjectsText(gameObjects);

            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);

            foreach (Text text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject)) 
                    ++statusLocalizationScene.CountPrefabs;
                if ( text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent) )
                    ++statusLocalizationScene.CountTextLocalization;
            }
            statusLocalizationScene.CountText = texts.Count;
            return statusLocalizationScene;
        }
    }

    public class StatusLocalizationScene
	{
        public int CountText = 0;
        public int CountPrefabs = 0;
        public int CountTextLocalization = 0;

		public override string ToString()
		{
            return String.Format(" Found text: {0} \n Found localize string event: {1} \n Prefabs: {2}", 
                CountText, CountTextLocalization, CountPrefabs);
		}
	}
}
