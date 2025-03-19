using EqualchanceGames.Tools.AutoTranslate.Editor;
using EqualchanceGames.Tools.GUIPro;
using EqualchanceGames.Tools.Helpers;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EqualchanceGames.Tools.AutoTranslate.Windows
{
    public class SearchText_EditorWindow : BaseSearch_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search Text in Scene or Prefab";

        private bool LSC = true;
        private bool LC = true;

        [MenuItem("Auto Localization/Search Text in Scene or Prefab", false, MyProjectSettings_AutoTranslate.BaseIndex + 40)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchText_EditorWindow window = GetWindow<SearchText_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchText);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateParameter();
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            UpdateParameter();
            _infoLocalization = string.Empty;
            _statusLocalizationScene = null;
        }

        private void UpdateParameter()
        {
            if (_sharedStringTables != null && _sharedStringTables.Count != 0) _dropdownTables.Selected = _sharedStringTables.First().TableCollectionName;
            else _dropdownTables.Selected = KEYWORD_NEWTABLE;

            _currentScene = DatabaseProject.GetCurrentScene();
            _prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (_prefabStage == null)
            {
                _dropdownTables.Filter = "scene";
                _nameTable = "StringTable_" + _currentScene.name + "_Scene";
            }
            else
            {
                //_dropdownTables.Filter = "prefab";
                _nameTable = "StringTable_" + _prefabStage.prefabContentsRoot.name + "_Prefab";
            }
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);
            if (_prefabStage == null)
            {
                EditorGUILayout.HelpBox("Tip. You can also search for localization text for a prefab by opening prefab edit", MessageType.Info);
            }
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)); // Main Begin 
            EditorGUILayout.BeginVertical(); // Begin 0

            if (_prefabStage == null)
			{
                LinesGUI.DrawTexts("Current Scene", _currentScene.name, 200);
            }
            else
			{
                LinesGUI.DrawTexts("Current Prefab", _prefabStage.prefabContentsRoot.name, 200);
            }

            _dropdownTables.DrawFilter("Filter for string tabels");
            _dropdownTables.Draw();

            TextField_NewStringTable();
            CheckNameStringTable();

            _dropdownLanguages.Draw();

            _skipPrefab = LinesGUI.DrawLineToggle("Skip prefabs", _skipPrefab);
            _skipVariantPrefab = LinesGUI.DrawLineToggle("Skip variant prefabs", _skipVariantPrefab);
            _skipEmptyText = LinesGUI.DrawLineToggle("Skip empty text", _skipEmptyText);
            _removeMissStringEvents = LinesGUI.DrawLineToggle("Remove miss stringEvents", _removeMissStringEvents);
            _autoSave = LinesGUI.DrawLineToggle("Auto Save", _autoSave);

            EditorGUILayout.EndVertical(); // End 0
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            LSC = EditorGUILayout.BeginFoldoutHeaderGroup(LSC, "Skip parent UI Components:");
            if (LSC)
            {
                _checkListSkipParentComponents.DrawButtons();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            LC = EditorGUILayout.BeginFoldoutHeaderGroup(LC, "Search UI Components:");
            if ( LC)
            {
                _checkListSearchComponents.Draw();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal(); // Main End  

            if (GUILayout.Button("Search text for localization"))
            {
                StartSearch();
                FillDispalay_StatusLocalization();
            }

            if (_statusLocalizationScene != null)
            {
                EditorGUILayout.HelpBox(_statusLocalizationScene.ToString(), MessageType.Info);
                _TabsGUI.Draw();
            }

            IsNullOrEmpty_NameStringTable();

            ValidateLocalizationSettings();
            ValidateLocales();

            GUILayout.Space(10);
            if (GUILayout.Button("Add localization"))
            {
                _infoLocalization = StartAddLocalization();
            }

            if (!string.IsNullOrEmpty(_infoLocalization)) EditorGUILayout.HelpBox(_infoLocalization, MessageType.Info);
        }

        private void StartSearch()
        {
            _searchTextParameters.SkipPrefab = _skipPrefab;
            _searchTextParameters.SkipVariantPrefab = _skipVariantPrefab;
            _searchTextParameters.SkipEmptyText = _skipEmptyText;
            _searchTextParameters.ListSearchComponents = _checkListSearchComponents.GetElements(true, true);
            _searchTextParameters.ListSkipParentComponents = _checkListSkipParentComponents.GetElements(true, true);

            if (_prefabStage == null) _statusLocalizationScene = SearchTextForLocalization.Search(_currentScene, _searchTextParameters);
            else _statusLocalizationScene = SearchTextForLocalization.Search(_prefabStage.prefabContentsRoot, _searchTextParameters);
        }

        private string StartAddLocalization()
        {
            AddLocalizationParameters parameters = new AddLocalizationParameters();

            if (_dropdownTables.Selected == KEYWORD_NEWTABLE) parameters.NameTable = _nameTable;
            else parameters.NameTable = _dropdownTables.Selected;

            if (string.IsNullOrEmpty(parameters.NameTable)) return "nameTable is null";

            parameters.IsSkipPrefab = _skipPrefab;
            parameters.IsSkipVariantPrefab= _skipVariantPrefab;
            parameters.IsSkipEmptyText = _skipEmptyText;
            parameters.SourceLocale = _selectedLocale;
            parameters.Lists = _checkListSearchComponents.GetElements(true, true);
            //parameters.ListSkipParentComponents = _checkListSkipParentComponents.GetElements(true, true);

            if (_statusLocalizationScene == null) StartSearch();
            else GetCheckTable();

            AddLocalization.Execute(parameters, _statusLocalizationScene);
            if (_removeMissStringEvents) ClearUpLocalization.RemoveMiss_LocalizeStringEvent(_statusLocalizationScene.LocalizeStringEvents);
            if (_autoSave)
            {
                //EditorSceneManager.SaveScene(scene);
                EditorSceneManager.SaveOpenScenes();
                if(_prefabStage != null) EditorUtility.SetDirty(_prefabStage.prefabContentsRoot);
            }
            
            return "Completed";
        }

    }
}