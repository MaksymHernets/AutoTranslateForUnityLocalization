using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
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
            GameObject[] gameObjects = SimpleDatabaseProject.GetGameObjects(parameters.Scene.name);
            if (gameObjects.Length == 0) return "gameObjects is 0";

            List<Text> listsText = SimpleDatabaseProject.GetGameObjectsText(gameObjects);
            if (listsText.Count == 0) return "texts is 0";

            SharedTableData sharedTable = GetOrAdd_SharedTableData(parameters.NameTable);

            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            StringTable stringTable = default(StringTable);

            foreach (Text text in listsText)
            {
                if (parameters.SkipPrefab == true && PrefabUtility.IsPartOfAnyPrefab(text.gameObject)) continue;

                localizeStringEvent = GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableData_AddEntry(sharedTable, text);

                stringTable = SimpleInterfaceStringTable.GetStringTable(sharedTable, parameters.SourceLocale);
                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                localizeStringEvent.Clear_OnUpdateString();

                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);

                localizeStringEvent.Sign_OnUpdateString(text);
            }

            EditorSceneManager.SaveScene(parameters.Scene);

            return "Completed";
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
                if (text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent))
                    ++statusLocalizationScene.CountTextLocalization;
            }
            statusLocalizationScene.CountText = texts.Count;
            return statusLocalizationScene;
        }

        private static SharedTableData GetOrAdd_SharedTableData(string nameTable)
		{
            SharedTableData sharedTable = SimpleInterfaceStringTable.GetSharedTable(nameTable);

            if (sharedTable != null) SimpleInterfaceStringTable.Clear_StringTable(sharedTable);
            else sharedTable = SimpleInterfaceStringTable.AddStringTable(nameTable);

            return sharedTable;
        }

        private static LocalizeStringEvent GetOrAdd_LocalizeStringEventComponent(GameObject gameObject)
		{
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            if (gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent))
                return gameObject.GetComponent<LocalizeStringEvent>();
            else
                return gameObject.AddComponent<LocalizeStringEvent>();
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
    }
}
