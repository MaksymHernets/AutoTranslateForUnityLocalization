using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class SearchTextPrefab_EditorWindow : BaseSearchText_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search text for Localization in Prefab";

        private PrefabStage _prefabStage;

        [MenuItem("Window/Auto Localization/Search Text for Tables in Prefab")]
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

            UpdateLocalization();
            UpdateParameter();
            _statusLocalizationScene = new StatusLocalizationScene();
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            UpdateLocalization();
            UpdateParameter();
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
            EditorGUIUtility.labelWidth = k_SeparationWidth;

            bool IsOpenPrefab = _prefabStage == null;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Prefab", GUILayout.Width(k_SeparationWidth));
            if ( IsOpenPrefab )
                EditorGUILayout.LabelField("Not open");
            else
                EditorGUILayout.LabelField(_prefabStage.prefabContentsRoot.name);
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

            _skipPrefab = EditorGUILayout.Toggle("Skip sub prefabs", _skipPrefab);

            EditorGUILayout.LabelField("Search UI Elements:");
            _checkListSearchElements.Draw();
            GUILayout.Space(10);

            if (IsOpenPrefab)
            {
                EditorGUILayout.HelpBox("Prefab not open", MessageType.Error);
                GUI.enabled = false;
            }

            if (GUILayout.Button("Search text for localization"))
            {
                _statusLocalizationScene = SearchTextForLocalization.CheckTextAboutLocalization(_prefabStage.prefabContentsRoot, _skipPrefab);
            }

            if (_statusLocalizationScene != null) EditorGUILayout.HelpBox(_statusLocalizationScene.ToString(), MessageType.Info);

            CheckNameStringTable();

            ValidateLocalizationSettings();
            ValidateLocales();

            if (GUILayout.Button("Add localization"))
            {
                _infoLocalization = StartSearch();
            }

            if (!string.IsNullOrEmpty(_infoLocalization)) EditorGUILayout.HelpBox(_infoLocalization, MessageType.Info);
        }

        private string StartSearch()
        {
            SearchTextParameters parameters = new SearchTextParameters();

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

			return SearchTextForLocalization.Search(parameters, _prefabStage.prefabContentsRoot);
        }
    }
}