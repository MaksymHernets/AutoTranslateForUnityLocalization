using EqualchanceGames.Tools.AutoTranslate.Editor;
using EqualchanceGames.Tools.GUIPro;
using EqualchanceGames.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EqualchanceGames.Tools.AutoTranslate.Windows
{
    public class SearchTextInScenes_EditorWindow : BaseSearch_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search Text in Scenes";

        private CheckListGUI _checkListScenes;
        private bool LC = true;
        private bool LSC = true;

        [MenuItem("Auto Localization/Search Text in Scenes", false, MyProjectSettings_AutoTranslate.BaseIndex + 41)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchTextInScenes_EditorWindow window = GetWindow<SearchTextInScenes_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchText);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            string[] scenes = DatabaseProject.GetPathScenes();
            _checkListScenes = new CheckListGUI(scenes.ToList());
            _checkListScenes.Width = 300;
            _checkListScenes.Height = 1000;
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            string[] scenes = DatabaseProject.GetPathScenes();
            if (_checkListScenes != null) _checkListScenes.Update(scenes.ToList());
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)); // Main Begin 
            EditorGUILayout.BeginVertical(); // Begin 0
            _checkListScenes.DrawButtons("Scenes:");
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.MaxHeight(200)); // Begin 1
            _skipPrefab = LinesGUI.DrawLineToggle("Skip prefabs", _skipPrefab);
            _skipVariantPrefab = LinesGUI.DrawLineToggle("Skip variant prefabs", _skipVariantPrefab);
            _skipEmptyText = LinesGUI.DrawLineToggle("Skip empty text", _skipEmptyText);
            _removeMissStringEvents = LinesGUI.DrawLineToggle("Remove miss stringEvents", _removeMissStringEvents);
            _autoSave = LinesGUI.DrawLineToggle("Auto Save", _autoSave);

            //EditorGUILayout.HelpBox("Not working yet", MessageType.Error);
            //GUI.enabled = false;
            LSC = EditorGUILayout.BeginFoldoutHeaderGroup(LSC, "Skip parent UI Components:");
            if (LSC)
            {
                _checkListSkipParentComponents.DrawButtons();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            LC = EditorGUILayout.BeginFoldoutHeaderGroup(LC , "Search UI Elements:"); // Begin 1
            if ( LC )
            {
                _checkListSearchComponents.Draw();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (GUILayout.Button("Add localization"))
            {
                StartAddLocalization();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
        }

        private void StartAddLocalization()
        {
            try
            {
                EditorUtility.DisplayProgressBar("Add Localization", "Start", 0);

                AddLocalizationParameters parameters = new AddLocalizationParameters();

                //if (string.IsNullOrEmpty(parameters.NameTable)) return "nameTable is null";

                parameters.IsSkipPrefab = _skipPrefab;
                parameters.IsSkipVariantPrefab = _skipVariantPrefab;
                parameters.IsSkipEmptyText = _skipEmptyText;
                parameters.SourceLocale = _selectedLocale;
                parameters.Lists = _checkListSearchComponents.GetElements(true, true);

                //if (_statusLocalizationScene == null) StartSearch();
                //else GetCheckTable();

                _searchTextParameters.SkipPrefab = _skipPrefab;
                _searchTextParameters.SkipVariantPrefab = _skipVariantPrefab;
                _searchTextParameters.SkipEmptyText = _skipEmptyText;
                _searchTextParameters.ListSearchComponents = _checkListSearchComponents.GetElements(true, true);
                _searchTextParameters.ListSkipParentComponents = _checkListSkipParentComponents.GetElements(true, true);
                List<string> paths = _checkListScenes.GetNames(true, true);

                float dola = paths.Count * 0.1f;
                int index = 0;

                foreach (string path in paths)
                {
                    EditorUtility.DisplayProgressBar("Add Localization", "Scene: " + path, index * dola);

                    Scene openScene = EditorSceneManager.OpenScene(path);

                    parameters.NameTable = "StringTable_" + openScene.name + "_Scene";

                    _statusLocalizationScene = SearchTextForLocalization.Search(openScene, _searchTextParameters);

                    AddLocalization.Execute(parameters, _statusLocalizationScene);

                    if (_removeMissStringEvents) ClearUpLocalization.RemoveMiss_LocalizeStringEvent(_statusLocalizationScene.LocalizeStringEvents);

                    //EditorSceneManager.SaveScene(scene);
                    EditorSceneManager.SaveOpenScenes();
                }
                ++index;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
            Debug.Log("Completed Add Localization for scenes");
        }
    }
}