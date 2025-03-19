using EqualchanceGames.Tools.Helpers;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EqualchanceGames.Tools.GUIPro;
using System.Security.Cryptography;
//using ASTools.Validator;

namespace EqualchanceGames.Tools.AutoTranslate
{
    public static class SearchTextForLocalization 
    {
        public static List<RowCheckList> GetAvailableForSearchUIComponents()
        {
            List<RowCheckList> Checklists = new List<RowCheckList>();
            Checklists.Add(new RowCheckList("Text Legacy", true, true));
            Checklists.Add(new RowCheckList("Text Mesh Legacy", true, true));
            Checklists.Add(new RowCheckList("Text Mesh Pro UI", true, true));
            Checklists.Add(new RowCheckList("Text Mesh Pro", true, true));
            //Checklists.Add(new RowCheckList("Dropdown Legacy", false, false));
            //Checklists.Add(new RowCheckList("Dropdown Mesh Pro", false, false));
            return Checklists;
        }

        public static List<RowCheckList> GetAvailableForSkipParentComponents()
        {
            List<RowCheckList> Checklists = new List<RowCheckList>();
            Checklists.Add(new RowCheckList("Toggle", false, true));
            Checklists.Add(new RowCheckList("Button", false, true));
            Checklists.Add(new RowCheckList("Input Field Legacy", false, true));
            Checklists.Add(new RowCheckList("Dropdown Legacy", false, true));
            Checklists.Add(new RowCheckList("Input Field Pro", false, true));
            Checklists.Add(new RowCheckList("Dropdown Pro", false, true));
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

            if ( parameters.ListSearchComponents.ContainsKey("Text Legacy") && parameters.ListSearchComponents["Text Legacy"])
			{
                List<Text> texts = GameObjectHelper.GetComponentsInChildrens<Text>(gameObjects);
                statusLocalizationScene.LegacyTexts = FilterTextLegacy(texts, parameters, statusLocalizationScene);
                if (parameters.ListSkipParentComponents.Count != 0)
                    statusLocalizationScene.LegacyTexts = FilterParentComponents<Text>(statusLocalizationScene.LegacyTexts, parameters.ListSkipParentComponents);
            }
            if ( parameters.ListSearchComponents.ContainsKey("Text Mesh Pro UI") && parameters.ListSearchComponents["Text Mesh Pro UI"])
            {
                List<TextMeshProUGUI> textMeshs = GameObjectHelper.GetComponentsInChildrens<TextMeshProUGUI>(gameObjects);
                statusLocalizationScene.TextMeshProUIs = FilterTextMeshProUI(textMeshs, parameters, statusLocalizationScene);
                if (parameters.ListSkipParentComponents.Count != 0)
                    statusLocalizationScene.TextMeshProUIs = FilterParentComponents<TextMeshProUGUI>(statusLocalizationScene.TextMeshProUIs, parameters.ListSkipParentComponents);
            }
            if (parameters.ListSearchComponents.ContainsKey("Text Mesh Legacy") && parameters.ListSearchComponents["Text Mesh Legacy"])
            {
                List<TextMesh> texts = GameObjectHelper.GetComponentsInChildrens<TextMesh>(gameObjects);
                statusLocalizationScene.LegacyMeshTexts = FilterTextMeshLegacy(texts, parameters, statusLocalizationScene);
                if (parameters.ListSkipParentComponents.Count != 0)
                    statusLocalizationScene.LegacyMeshTexts = FilterParentComponents<TextMesh>(statusLocalizationScene.LegacyMeshTexts, parameters.ListSkipParentComponents);
            }
            if (parameters.ListSearchComponents.ContainsKey("Text Mesh Pro") && parameters.ListSearchComponents["Text Mesh Pro"])
            {
                List<TextMeshPro> textMeshs = GameObjectHelper.GetComponentsInChildrens<TextMeshPro>(gameObjects);
                statusLocalizationScene.TextMeshPros = FilterTextMeshPro(textMeshs, parameters, statusLocalizationScene);
                if (parameters.ListSkipParentComponents.Count != 0)
                    statusLocalizationScene.TextMeshPros = FilterParentComponents<TextMeshPro>(statusLocalizationScene.TextMeshPros, parameters.ListSkipParentComponents);
            }
            //if ( parameters.ListSearchComponents.ContainsKey("Dropdown Legacy") && parameters.ListSearchComponents["Dropdown Legacy"])
            //{
            //    List<Dropdown> dropdowns = GameObjectHelper.GetComponentsInChildrens<Dropdown>(gameObjects);
            //    statusLocalizationScene.LegacyDropdowns = FilterDropdownLegacy(dropdowns, parameters, statusLocalizationScene);
            //}
            //if (parameters.ListSearchComponents.ContainsKey("Dropdown Mesh Pro") && parameters.ListSearchComponents["Dropdown Mesh Pro"])
            //{
            //    List<TMP_Dropdown> dropdowns = GameObjectHelper.GetComponentsInChildrens<TMP_Dropdown>(gameObjects);
            //    statusLocalizationScene.TMP_Dropdowns = FilterDropdownTMP(dropdowns, parameters, statusLocalizationScene);
            //}
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

        public static List<T> FilterParentComponents<T>(List<T> components, Dictionary<string, bool> skipParentComponents) where T : Component
        {
            List<T> result = new List<T>();
            foreach (T component in components)
            {
                Transform parent = component.transform.parent;
                if (skipParentComponents.ContainsKey("Toggle") && skipParentComponents["Toggle"])
                {
                    Toggle toggle = default(Toggle);
                    if (parent.TryGetComponent<Toggle>(out toggle)) continue;
                }
                if (skipParentComponents.ContainsKey("Button") && skipParentComponents["Button"])
                {
                    Button button = default(Button);
                    if (parent.TryGetComponent<Button>(out button)) continue;
                }
                if (skipParentComponents.ContainsKey("Input Field Legacy") && skipParentComponents["Input Field Legacy"])
                {
                    InputField inputField = default(InputField);
                    if (parent.TryGetComponent<InputField>(out inputField)) continue;
                }
                if (skipParentComponents.ContainsKey("Dropdown Legacy") && skipParentComponents["Dropdown Legacy"])
                {
                    Dropdown dropdown = default(Dropdown);
                    if (parent.TryGetComponent<Dropdown>(out dropdown)) continue;
                }
                if (skipParentComponents.ContainsKey("Input Field Pro") && skipParentComponents["Input Field Pro"])
                {
                    TMP_InputField inputField = default(TMP_InputField);
                    if (parent.TryGetComponent<TMP_InputField>(out inputField)) continue;
                }
                if (skipParentComponents.ContainsKey("Dropdown Pro") && skipParentComponents["Dropdown Pro"])
                {
                    TMP_Dropdown dropdown = default(TMP_Dropdown);
                    if (parent.TryGetComponent<TMP_Dropdown>(out dropdown)) continue;
                }
                result.Add(component);
            }
            return result;
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
