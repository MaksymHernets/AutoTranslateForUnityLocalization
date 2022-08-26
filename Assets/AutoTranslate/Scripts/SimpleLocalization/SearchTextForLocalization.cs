using GoodTime.Tools.Helpers;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
	public static class SearchTextForLocalization 
    {
        public static List<string> GetAvailableForSearchUIElements()
        {
            List<string> Checklists = new List<string>();
            Checklists.Add("Text Legacy");
            Checklists.Add("Dropdown Legacy");
            Checklists.Add("Text Mesh Pro");
            Checklists.Add("Dropdown Mesh Pro");
            return Checklists;
        }

        public static StatusLocalizationScene Search(Scene scene, SearchTextParameters parameters)
        {
            GameObject[] gameObjects = DatabaseProject.GetGameObjects(scene.name);

            return Search(gameObjects, parameters);
        }

        public static StatusLocalizationScene Search(GameObject gameObject, SearchTextParameters parameters)
        {
            GameObject[] gameObjects = new GameObject[1];
            gameObjects[0] = gameObject;

            return Search(gameObjects, parameters);
        }

        public static StatusLocalizationScene Search(GameObject[] gameObjects, SearchTextParameters parameters)
		{
            StatusLocalizationScene statusLocalizationScene = new StatusLocalizationScene();

            if ( parameters.Lists.ContainsKey("Text Legacy") && parameters.Lists["Text Legacy"])
			{
                List<Text> texts = GameObjectHelper.GetComponentsInChildrens<Text>(gameObjects);
                statusLocalizationScene.LegacyTexts = FilterTextLegacy(texts, parameters.SkipPrefab, statusLocalizationScene);
            }
            if ( parameters.Lists.ContainsKey("Text Mesh Pro") && parameters.Lists["Text Mesh Pro"])
            {
                List<TextMeshProUGUI> textMeshs = GameObjectHelper.GetComponentsInChildrens<TextMeshProUGUI>(gameObjects);
                statusLocalizationScene.TextMeshs = FilterTextMesh(textMeshs, parameters.SkipPrefab, statusLocalizationScene);
            }
            if ( parameters.Lists.ContainsKey("Dropdown Legacy") && parameters.Lists["Dropdown Legacy"])
            {
                List<Dropdown> dropdowns = GameObjectHelper.GetComponentsInChildrens<Dropdown>(gameObjects);
                statusLocalizationScene.LegacyDropdowns = FilterDropdownLegacy(dropdowns, parameters.SkipPrefab, statusLocalizationScene);
            }
            if (parameters.Lists.ContainsKey("Dropdown Mesh Pro") && parameters.Lists["Dropdown Mesh Pro"])
            {
                List<TMP_Dropdown> dropdowns = GameObjectHelper.GetComponentsInChildrens<TMP_Dropdown>(gameObjects);
                statusLocalizationScene.TMP_Dropdowns = FilterDropdownTMP(dropdowns, parameters.SkipPrefab, statusLocalizationScene);
            }
            if ( parameters.SkipPrefab == false )
            {
                statusLocalizationScene.Prefabs = GameObjectHelper.DetectPrefabs(gameObjects);
            }
            return statusLocalizationScene;
        }

        public static List<Text> FilterTextLegacy(List<Text> texts, bool skipPrefab, StatusLocalizationScene statusLocalizationScene)
		{
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<Text> result = new List<Text>();

            foreach (Text text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && skipPrefab == true) 
                    continue;

                if (text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent))
                    statusLocalizationScene.LocalizeStringEvents.Add(localizeStringEvent);

                result.Add(text);
            }

            return result;
        }

        public static List<Dropdown> FilterDropdownLegacy(List<Dropdown> texts, bool skipPrefab, StatusLocalizationScene statusLocalizationScene)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<Dropdown> result = new List<Dropdown>();

            foreach (Dropdown text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && skipPrefab == true)
                    continue;

                if (text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent))
                    statusLocalizationScene.LocalizeStringEvents.Add(localizeStringEvent);

                result.Add(text);
            }

            return result;
        }

        public static List<TMP_Dropdown> FilterDropdownTMP(List<TMP_Dropdown> texts, bool skipPrefab, StatusLocalizationScene statusLocalizationScene)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<TMP_Dropdown> result = new List<TMP_Dropdown>();

            foreach (TMP_Dropdown text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && skipPrefab == true)
                    continue;

                if (text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent))
                    statusLocalizationScene.LocalizeStringEvents.Add(localizeStringEvent);

                result.Add(text);
            }

            return result;
        }

        public static List<TextMeshProUGUI> FilterTextMesh(List<TextMeshProUGUI> texts, bool skipPrefab, StatusLocalizationScene statusLocalizationScene)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<TextMeshProUGUI> result = new List<TextMeshProUGUI>();

            foreach (TextMeshProUGUI text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && skipPrefab == true)
                    continue;

                if (text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent))
                    statusLocalizationScene.LocalizeStringEvents.Add(localizeStringEvent);

                result.Add(text);
            }

            return result;
        }
    }
}
