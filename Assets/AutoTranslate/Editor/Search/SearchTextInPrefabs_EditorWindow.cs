using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.GUIPro;
using GoodTime.Tools.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class SearchTextInPrefabs_EditorWindow : BaseSearch_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search Text in Prefabs";

        private CheckListGUI _checkListScenes;

        [MenuItem("Window/Auto Localization/Search Text in Prefabs", false, MyProjectSettings_AutoTranslate.BaseIndex + 42)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchTextInPrefabs_EditorWindow window = GetWindow<SearchTextInPrefabs_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchText);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            List<GameObject> gameObjects = DatabaseProject.GetPrefabs();
            _checkListScenes = new CheckListGUI(gameObjects.Select(w => w.name).ToList());
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            List<GameObject> gameObjects = DatabaseProject.GetPrefabs();
            _checkListScenes.UpdateCheck(gameObjects.Select(w => w.name).ToList(), false);
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.MinHeight(170)); // Main Begin 
            EditorGUILayout.BeginFadeGroup(1); // Begin 0
            _checkListScenes.DrawButtons("Scenes:");
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.BeginFadeGroup(2); // Begin 1
            _skipPrefab = LinesGUI.DrawLineToggle("Skip prefabs", _skipPrefab);
            _skipVariantPrefab = LinesGUI.DrawLineToggle("Skip variant prefabs", _skipVariantPrefab);
            _skipEmptyText = LinesGUI.DrawLineToggle("Skip empty text", _skipEmptyText);
            _removeMissStringEvents = LinesGUI.DrawLineToggle("Remove miss stringEvents", _removeMissStringEvents);
            _autoSave = LinesGUI.DrawLineToggle("Auto Save", _autoSave);

            //EditorGUILayout.HelpBox("Not working yet", MessageType.Error);
            //GUI.enabled = false;
            if (GUILayout.Button("Add localization"))
            {
                StartAddLocalization();
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndHorizontal();
        }

        private void StartAddLocalization()
        {
            try
            {
                EditorUtility.DisplayProgressBar("Add Localization", "Load prefabs", 0);

                AddLocalizationParameters parameters = new AddLocalizationParameters();

                parameters.IsSkipPrefab = _skipPrefab;
                parameters.IsSkipVariantPrefab = _skipVariantPrefab;
                parameters.IsSkipEmptyText = _skipEmptyText;
                parameters.SourceLocale = _selectedLocale;
                parameters.Lists = _checkListSearchElements.GetElements(true, true);

                _searchTextParameters.Lists = _checkListScenes.GetElements(true, true);
                List<GameObject> gameObjects = DatabaseProject.GetPrefabs(_checkListScenes.GetNames(true, true));

                float dola = gameObjects.Count * 0.1f;
                int index = 0;

                foreach (GameObject gameObject in gameObjects)
                {
                    EditorUtility.DisplayProgressBar("Add Localization", "Prefabs: " + gameObject.name, index * dola);

                    parameters.NameTable = "StringTable_" + gameObject.name + "_Prefab";

                    _statusLocalizationScene = SearchTextForLocalization.Search(_prefabStage.prefabContentsRoot, _searchTextParameters);

                    AddLocalization.Execute(parameters, _statusLocalizationScene);

                    EditorUtility.SetDirty(gameObject);
                }
                ++index;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}