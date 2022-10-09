using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement; 
using UnityEngine;
using UnityEditor.Experimental.SceneManagement; // For Unity 2019.4 !!!!

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class SearchTextPrefab_EditorWindow : BaseSearchText_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search text for Localization in Prefab";

        private PrefabStage _prefabStage;

        [MenuItem("Window/Auto Localization/Search Text for Tables in Prefab", false, 41)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchTextPrefab_EditorWindow window = GetWindow<SearchTextPrefab_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchTextPrefab);
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

            _prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (_prefabStage != null)
            {
                _nameTable = "StringTable_" + _prefabStage.prefabContentsRoot.name + "_Prefab";
            }
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            bool IsOpenPrefab = _prefabStage == null;

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.MinHeight(170));
            EditorGUILayout.BeginFadeGroup(1);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Prefab", GUILayout.Width(k_SeparationWidth));
            if ( IsOpenPrefab )
                EditorGUILayout.LabelField("Not open");
            else
                EditorGUILayout.LabelField(_prefabStage.prefabContentsRoot.name);
            EditorGUILayout.EndHorizontal();

            _dropdownTables.Draw();

            TextField_NewStringTable();
            CheckNameStringTable();

            _dropdownLanguages.Draw();

            Toggle_SkipPrefabs();
            Toggle_SkipEmptyText();
            Toggle_RemoveMissStringEvents();
            Toggle_AutoSave();

            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.BeginFadeGroup(1);

            EditorGUILayout.LabelField("Search UI Elements:");
            _checkListSearchElements.Draw();

            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndHorizontal();

            if (IsOpenPrefab)
            {
                EditorGUILayout.HelpBox("Prefab not open", MessageType.Error);
                GUI.enabled = false;
            }

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
            _searchTextParameters.SkipEmptyText = _skipEmptyText;
            _searchTextParameters.Lists = _checkListSearchElements.GetElements();

            _statusLocalizationScene = SearchTextForLocalization.Search(_prefabStage.prefabContentsRoot, _searchTextParameters);
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
            if (_autoSave) EditorUtility.SetDirty(_prefabStage.prefabContentsRoot);
            return "Good";
        }
    }
}