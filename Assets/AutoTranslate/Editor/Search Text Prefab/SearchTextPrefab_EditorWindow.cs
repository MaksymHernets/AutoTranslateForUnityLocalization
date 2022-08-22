using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
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
            UpdateLocalization();
            UpdateParameter();
            _statusLocalizationScene = new StatusLocalizationScene();
        }

        protected override void OnFocus()
        {
            UpdateLocalization();
            UpdateParameter();
        }

        private void UpdateParameter()
        {
            if (_sharedStringTables != null && _sharedStringTables.Count != 0) _selectedTable = _sharedStringTables.First().TableCollectionName;
            else _selectedTable = KEYWORD_NEWTABLE;

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

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Prefab", GUILayout.Width(k_SeparationWidth));
            if ( IsOpenPrefab )
                EditorGUILayout.LabelField("Not open");
            else
                EditorGUILayout.LabelField(_prefabStage.prefabContentsRoot.name);
            EditorGUILayout.EndHorizontal();

            Dropdown_StringTables(k_SeparationWidth);

            if (_selectedTable == KEYWORD_NEWTABLE)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New string table", GUILayout.Width(k_SeparationWidth));
                _nameTable = EditorGUILayout.TextField("", _nameTable);
                EditorGUILayout.EndHorizontal();
            }

            Dropdown_SelectLanguage(k_SeparationWidth);

            EditorGUIUtility.labelWidth = k_SeparationWidth;
            _skipPrefab = EditorGUILayout.Toggle("Skip sub prefabs", _skipPrefab);

            if (IsOpenPrefab)
            {
                EditorGUILayout.HelpBox("Prefab not open", MessageType.Error);
                GUI.enabled = false;
            }

            if (GUILayout.Button("Search text for localization"))
            {
                _statusLocalizationScene = SearchTextForLocalization.CheckTextAboutLocalization(_prefabStage.prefabContentsRoot);
            }

            if (_statusLocalizationScene?.CountText != 0) EditorGUILayout.HelpBox(_statusLocalizationScene.ToString(), MessageType.Info);

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

			if (_selectedTable == KEYWORD_NEWTABLE)
			{
				parameters.NameTable = _nameTable;
			}
			else
			{
				parameters.NameTable = _selectedTable;
			}

			if (string.IsNullOrEmpty(parameters.NameTable)) return "nameTable is null";

			parameters.SkipPrefab = _skipPrefab;
			parameters.SourceLocale = _selectedLocale;

			return SearchTextForLocalization.Search(parameters, _prefabStage.prefabContentsRoot);
        }
    }
}