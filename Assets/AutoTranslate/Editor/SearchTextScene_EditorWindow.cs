using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.Helpers;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.SceneManagement; // For Unity 2019.4 !!!!
using UnityEditor.SceneManagement;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class SearchTextScene_EditorWindow : BaseSearchText_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search text for Localization in Scene";

        private Scene _currentScene;

        [MenuItem("Window/Auto Localization/Search Text for Tables in Scene", false, 40)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchTextScene_EditorWindow window = GetWindow<SearchTextScene_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchTextScene);
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
            if ( _dropdownTables != null )
			{
                if (_sharedStringTables != null && _sharedStringTables.Count != 0) _dropdownTables.Selected = _sharedStringTables.First().TableCollectionName;
                else _dropdownTables.Selected = KEYWORD_NEWTABLE;
            }
            
            _currentScene = DatabaseProject.GetCurrentScene();
            _nameTable = "StringTable_" + _currentScene.name + "_Scene";
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.MinHeight(170)); // Main Begin 
			EditorGUILayout.BeginFadeGroup(1); // Begin 0

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Scene", GUILayout.Width(k_SeparationWidth));
            EditorGUILayout.LabelField(_currentScene.name);
            EditorGUILayout.EndHorizontal();

            _dropdownTables.Draw();

            TextField_NewStringTable();
            CheckNameStringTable();

            _dropdownLanguages.Draw();

            Toggle_SkipPrefabs();
            Toggle_SkipEmptyText();
            Toggle_RemoveMissStringEvents();
            Toggle_AutoSave();

            EditorGUILayout.EndFadeGroup(); // End 0
			EditorGUILayout.BeginFadeGroup(1); // Begin 1

            EditorGUILayout.LabelField("Search UI Elements:");
            _checkListSearchElements.Draw();

            EditorGUILayout.EndFadeGroup(); // End 1
            EditorGUILayout.EndHorizontal(); // Main End  

            if (GUILayout.Button("Search text for localization"))
            {
                StartSearch();
                FillDispalay_StatusLocalization();
            }

            if ( _statusLocalizationScene != null )
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

            if ( !string.IsNullOrEmpty(_infoLocalization) ) EditorGUILayout.HelpBox(_infoLocalization, MessageType.Info);
        }

        private void StartSearch()
		{
            _searchTextParameters.SkipPrefab = _skipPrefab;
            _searchTextParameters.SkipEmptyText = _skipEmptyText;
            _searchTextParameters.Lists = _checkListSearchElements.GetElements();

            _statusLocalizationScene = SearchTextForLocalization.Search(_currentScene, _searchTextParameters);
        }

        private string StartAddLocalization()
		{
            AddLocalizationParameters parameters = new AddLocalizationParameters();

            if (_dropdownTables.Selected == KEYWORD_NEWTABLE) parameters.NameTable = _nameTable;
            else parameters.NameTable = _dropdownTables.Selected;

            if (string.IsNullOrEmpty(parameters.NameTable)) return "nameTable is null";

            parameters.IsSkipPrefab = _skipPrefab;
            parameters.IsSkipEmptyText = _skipEmptyText;
            parameters.SourceLocale = _selectedLocale;
            parameters.Lists = _checkListSearchElements.GetElements();

            if (_statusLocalizationScene == null) StartSearch();
            else GetCheckTable();

            AddLocalization.Execute(parameters, _statusLocalizationScene);
            if (_removeMissStringEvents) AddLocalization.RemoveMiss_LocalizeStringEvent(_statusLocalizationScene.LocalizeStringEvents);
            if (_autoSave) EditorSceneManager.SaveOpenScenes();
            return "Good";
		}
    }
}