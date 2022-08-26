using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.Helpers;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class SearchTextScene_EditorWindow : BaseSearchText_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search text for Localization in Scene";

        private Scene _currentScene;

        [MenuItem("Window/Auto Localization/Search Text for Tables in Scene")]
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
            EditorGUIUtility.labelWidth = k_SeparationWidth;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Scene", GUILayout.Width(k_SeparationWidth));
            EditorGUILayout.LabelField(_currentScene.name);
            EditorGUILayout.EndHorizontal();

            _dropdownTables.Draw();

            if (_dropdownTables.Selected == KEYWORD_NEWTABLE)
			{
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New string table", GUILayout.Width(k_SeparationWidth));
                _nameTable = EditorGUILayout.TextField("", _nameTable);
                EditorGUILayout.EndHorizontal();
            }

            _dropdownLanguages.Draw();

            _skipPrefab = EditorGUILayout.Toggle("Skip prefabs", _skipPrefab);

            EditorGUILayout.LabelField("Search UI Elements:");
            _checkListSearchElements.Draw();
            GUILayout.Space(10);

            if (GUILayout.Button("Search text for localization"))
            {
                _searchTextParameters.SkipPrefab = _skipPrefab;
                _searchTextParameters.Lists = _checkListSearchElements.GetElements();

                _statusLocalizationScene = SearchTextForLocalization.Search(_currentScene, _searchTextParameters);
            }

            if ( _statusLocalizationScene != null ) EditorGUILayout.HelpBox(_statusLocalizationScene.ToString() , MessageType.Info);

            CheckNameStringTable();

            ValidateLocalizationSettings();
            ValidateLocales();

            if (GUILayout.Button("Add localization"))
            {
                _infoLocalization = StartSearch();
            }

            if ( !string.IsNullOrEmpty(_infoLocalization) ) EditorGUILayout.HelpBox(_infoLocalization, MessageType.Info);
        }

        private string StartSearch()
		{
            AddLocalizationParameters parameters = new AddLocalizationParameters();

            if (_dropdownTables.Selected == KEYWORD_NEWTABLE)
            {
                parameters.NameTable = _nameTable;
            }
            else
            {
                parameters.NameTable = _dropdownTables.Selected;
            }

            if (string.IsNullOrEmpty(parameters.NameTable)) return "nameTable is null";

            parameters.IsSkipPrefab = _skipPrefab;
            parameters.SourceLocale = _selectedLocale;
            parameters.Lists = _checkListSearchElements.GetElements();

            return AddLocalization.Execute(parameters, _statusLocalizationScene);
		}
    }
}