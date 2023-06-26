using GoodTime.Tools.Helpers;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoodTime.Tools.GUIPro;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public static class SearchTextForLocalization 
    {
        public static List<RowCheckList> GetAvailableForSearchUIElements()
        {
            List<RowCheckList> Checklists = new List<RowCheckList>();
            Checklists.Add(new RowCheckList("Text Legacy", true, true));
            Checklists.Add(new RowCheckList("Text Mesh Legacy", true, true));
            Checklists.Add(new RowCheckList("Text Mesh Pro UI", true, true));
            Checklists.Add(new RowCheckList("Text Mesh Pro", true, true));
            Checklists.Add(new RowCheckList("Dropdown Legacy", false, false));
            Checklists.Add(new RowCheckList("Dropdown Mesh Pro", false, false));
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
                statusLocalizationScene.LegacyTexts = FilterTextLegacy(texts, parameters, statusLocalizationScene);
            }
            if ( parameters.Lists.ContainsKey("Text Mesh Pro UI") && parameters.Lists["Text Mesh Pro UI"])
            {
                List<TextMeshProUGUI> textMeshs = GameObjectHelper.GetComponentsInChildrens<TextMeshProUGUI>(gameObjects);
                statusLocalizationScene.TextMeshProUIs = FilterTextMeshProUI(textMeshs, parameters, statusLocalizationScene);
            }
            if (parameters.Lists.ContainsKey("Text Mesh Legacy") && parameters.Lists["Text Mesh Legacy"])
            {
                List<TextMesh> texts = GameObjectHelper.GetComponentsInChildrens<TextMesh>(gameObjects);
                statusLocalizationScene.LegacyMeshTexts = FilterTextMeshLegacy(texts, parameters, statusLocalizationScene);
            }
            if (parameters.Lists.ContainsKey("Text Mesh Pro") && parameters.Lists["Text Mesh Pro"])
            {
                List<TextMeshPro> textMeshs = GameObjectHelper.GetComponentsInChildrens<TextMeshPro>(gameObjects);
                statusLocalizationScene.TextMeshPros = FilterTextMeshPro(textMeshs, parameters, statusLocalizationScene);
            }
            if ( parameters.Lists.ContainsKey("Dropdown Legacy") && parameters.Lists["Dropdown Legacy"])
            {
                List<Dropdown> dropdowns = GameObjectHelper.GetComponentsInChildrens<Dropdown>(gameObjects);
                statusLocalizationScene.LegacyDropdowns = FilterDropdownLegacy(dropdowns, parameters, statusLocalizationScene);
            }
            if (parameters.Lists.ContainsKey("Dropdown Mesh Pro") && parameters.Lists["Dropdown Mesh Pro"])
            {
                List<TMP_Dropdown> dropdowns = GameObjectHelper.GetComponentsInChildrens<TMP_Dropdown>(gameObjects);
                statusLocalizationScene.TMP_Dropdowns = FilterDropdownTMP(dropdowns, parameters, statusLocalizationScene);
            }
            if (parameters.SkipPrefab == false)
            {
                statusLocalizationScene.Prefabs = GameObjectHelper.DetectPrefabs(gameObjects);
            }
            if (parameters.SkipVariantPrefab == false)
            {
                statusLocalizationScene.VariantPrefabs = GameObjectHelper.DetectVariantPrefabs(gameObjects);
            }
            statusLocalizationScene.LocalizeStringEvents = GetAllLocalizeStringEvents(gameObjects);
            return statusLocalizationScene;
        }

        public static List<Text> FilterTextLegacy(List<Text> texts, SearchTextParameters parameters, StatusLocalizationScene statusLocalizationScene)
		{
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<Text> result = new List<Text>();

            foreach (Text text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && parameters.SkipPrefab == true) 
                    continue;

                if (PrefabUtility.IsPartOfVariantPrefab(text.gameObject) && parameters.SkipVariantPrefab == true)
                    continue;

                if ( string.IsNullOrEmpty(text.text) && parameters.SkipEmptyText == true) 
                    continue;

                result.Add(text);
            }

            return result;
        }

        public static List<Dropdown> FilterDropdownLegacy(List<Dropdown> texts, SearchTextParameters parameters, StatusLocalizationScene statusLocalizationScene)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<Dropdown> result = new List<Dropdown>();

            foreach (Dropdown text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && parameters.SkipPrefab == true)
                    continue;

                if (PrefabUtility.IsPartOfVariantPrefab(text.gameObject) && parameters.SkipVariantPrefab == true)
                    continue;

                result.Add(text);
            }

            return result;
        }

        public static List<TMP_Dropdown> FilterDropdownTMP(List<TMP_Dropdown> texts, SearchTextParameters parameters, StatusLocalizationScene statusLocalizationScene)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<TMP_Dropdown> result = new List<TMP_Dropdown>();

            foreach (TMP_Dropdown text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && parameters.SkipPrefab == true)
                    continue;

                if (PrefabUtility.IsPartOfVariantPrefab(text.gameObject) && parameters.SkipVariantPrefab == true)
                    continue;

                result.Add(text);
            }

            return result;
        }

        public static List<TextMeshProUGUI> FilterTextMeshProUI(List<TextMeshProUGUI> texts, SearchTextParameters parameters, StatusLocalizationScene statusLocalizationScene)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<TextMeshProUGUI> result = new List<TextMeshProUGUI>();

            foreach (TextMeshProUGUI text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && parameters.SkipPrefab == true)
                    continue;

                if (PrefabUtility.IsPartOfVariantPrefab(text.gameObject) && parameters.SkipVariantPrefab == true)
                    continue;

                if (string.IsNullOrEmpty(text.text) && parameters.SkipEmptyText == true)
                    continue;

                result.Add(text);
            }

            return result;
        }

        public static List<TextMesh> FilterTextMeshLegacy(List<TextMesh> texts, SearchTextParameters parameters, StatusLocalizationScene statusLocalizationScene)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<TextMesh> result = new List<TextMesh>();

            foreach (TextMesh text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && parameters.SkipPrefab == true)
                    continue;

                if (PrefabUtility.IsPartOfVariantPrefab(text.gameObject) && parameters.SkipVariantPrefab == true)
                    continue;

                if (string.IsNullOrEmpty(text.text) && parameters.SkipEmptyText == true)
                    continue;

                result.Add(text);
            }

            return result;
        }

        public static List<TextMeshPro> FilterTextMeshPro(List<TextMeshPro> texts, SearchTextParameters parameters, StatusLocalizationScene statusLocalizationScene)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            List<TextMeshPro> result = new List<TextMeshPro>();

            foreach (TextMeshPro text in texts)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(text.gameObject) && parameters.SkipPrefab == true)
                    continue;

                if (PrefabUtility.IsPartOfVariantPrefab(text.gameObject) && parameters.SkipVariantPrefab == true)
                    continue;

                if (string.IsNullOrEmpty(text.text) && parameters.SkipEmptyText == true)
                    continue;

                result.Add(text);
            }

            return result;
        }

        public static List<LocalizeStringEvent> GetAllLocalizeStringEvents(GameObject[] gameObjects)
		{
            List<LocalizeStringEvent> localizeStringEvents = GameObjectHelper.GetComponentsInChildrens<LocalizeStringEvent>(gameObjects);
            return localizeStringEvents;
        }
    }
}
