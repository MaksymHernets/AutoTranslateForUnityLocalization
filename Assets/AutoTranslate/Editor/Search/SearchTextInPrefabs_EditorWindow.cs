using EqualchanceGames.Tools.AutoTranslate.Editor;
using EqualchanceGames.Tools.GUIPro;
using EqualchanceGames.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EqualchanceGames.Tools.AutoTranslate.Windows
{
    public class SearchTextInPrefabs_EditorWindow : BaseSearch_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search Text in Prefabs";

        private CheckListGUI _checkListScenes;
        private bool LC = true;
        private bool LSC = true;

        [MenuItem("Auto Localization/Search Text in Prefabs", false, MyProjectSettings_AutoTranslate.BaseIndex + 42)]
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
            _checkListScenes.Width = 300;
            _checkListScenes.Height = 1000;
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            List<GameObject> gameObjects = DatabaseProject.GetPrefabs();
            _checkListScenes.UpdateCheck(gameObjects.Select(w => w.name).ToList());
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)); // Main Begin 
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true)); // Begin 0
            _checkListScenes.DrawButtons("Scenes:");
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.MaxHeight(200)); // Begin 1
            _skipPrefab = LinesGUI.DrawLineToggle("Skip prefabs", _skipPrefab);
            _skipVariantPrefab = LinesGUI.DrawLineToggle("Skip variant prefabs", _skipVariantPrefab);
            _skipEmptyText = LinesGUI.DrawLineToggle("Skip empty text", _skipEmptyText);
            _removeMissStringEvents = LinesGUI.DrawLineToggle("Remove miss stringEvents", _removeMissStringEvents);
            _autoSave = LinesGUI.DrawLineToggle("Auto Save", _autoSave);

            LSC = EditorGUILayout.BeginFoldoutHeaderGroup(LSC, "Skip parent UI Components:");
            if (LSC)
            {
                _checkListSkipParentComponents.DrawButtons();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            LC = EditorGUILayout.BeginFoldoutHeaderGroup(LC, "Search UI Elements:"); // Begin 1
            if (LC)
            {
                _checkListSearchComponents.Draw();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (GUILayout.Button("Add localization"))
            {
                StartAddLocalization();
            }
            //EditorGUILayout.HelpBox("Not working yet", MessageType.Error);
            //GUI.enabled = false;
            EditorGUILayout.EndVertical();
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
                parameters.Lists = _checkListSearchComponents.GetElements(true, true);

                _searchTextParameters.SkipPrefab = _skipPrefab;
                _searchTextParameters.SkipVariantPrefab = _skipVariantPrefab;
                _searchTextParameters.SkipEmptyText = _skipEmptyText;
                _searchTextParameters.ListSearchComponents = _checkListSearchComponents.GetElements(true, true);
                _searchTextParameters.ListSkipParentComponents = _checkListSkipParentComponents.GetElements(true, true);
                List<GameObject> gameObjects = DatabaseProject.GetPrefabs(_checkListScenes.GetNames(true, true));

                float dola = gameObjects.Count * 0.1f;
                int index = 0;

                foreach (GameObject gameObject in gameObjects)
                {
                    EditorUtility.DisplayProgressBar("Add Localization", "Prefabs: " + gameObject.name, index * dola);

                    parameters.NameTable = "StringTable_" + gameObject.name + "_Prefab";

                    _statusLocalizationScene = SearchTextForLocalization.Search(gameObject, _searchTextParameters);

                    AddLocalization.Execute(parameters, _statusLocalizationScene);

                    EditorUtility.SetDirty(gameObject);
                }
                ++index;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
            Debug.Log("Completed Add Localization for prefabs");
        }
    }
}