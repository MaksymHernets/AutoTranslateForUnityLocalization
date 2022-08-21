using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class SearchTextScene_EditorWindow : BaseLocalization_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Find text for Localization in Scene";

        private Scene _currentScene;
        private string _selectedTable = KEYWORD_NEWTABLE;
        private string _nameTable = string.Empty;
        private bool _skipPrefab;

        private StatusLocalizationScene _statusLocalizationScene;

        private string _infoLocalization = string.Empty;

        private const string KEYWORD_NEWTABLE = "-New-";

        [MenuItem("Window/Auto Localization/Find Text for Tables in Scene")]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchTextScene_EditorWindow window = GetWindow<SearchTextScene_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.FindTextScene);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _statusLocalizationScene = new StatusLocalizationScene();

            UpdateParameter();
        }

        protected override void OnFocus()
        {
            base.OnFocus();

            UpdateParameter();

            _infoLocalization = string.Empty;
        }

        private void UpdateParameter()
		{
            if ( _sharedStringTables!= null && _sharedStringTables.Count != 0) _selectedTable = _sharedStringTables.First().TableCollectionName;
            else _selectedTable = KEYWORD_NEWTABLE;

            _currentScene = SimpleDatabaseProject.GetCurrentScene();
            _nameTable = "StringTable_" + _currentScene.name + "_Scene";
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Scene", GUILayout.Width(k_SeparationWidth));
            EditorGUILayout.LabelField(_currentScene.name);
            EditorGUILayout.EndHorizontal();

            Dropdown_StringTables(k_SeparationWidth);

            if ( _selectedTable == KEYWORD_NEWTABLE)
			{
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New string table", GUILayout.Width(k_SeparationWidth));
                _nameTable = EditorGUILayout.TextField("", _nameTable);
                EditorGUILayout.EndHorizontal();
            }
            
            Dropdown_SelectLanguage(k_SeparationWidth);

            EditorGUIUtility.labelWidth = k_SeparationWidth;
            _skipPrefab = EditorGUILayout.Toggle("Skip prefabs", _skipPrefab);

            if (GUILayout.Button("Search text for localization"))
            {
                _statusLocalizationScene = SearchTextForLocalization.CheckTextAboutLocalization(_currentScene);
            }

            if ( _statusLocalizationScene.CountText != 0 ) EditorGUILayout.HelpBox(_statusLocalizationScene.ToString() , MessageType.Info);

            if (SimpleInterfaceStringTable.CheckNameStringTable(_nameTable))
            {
                EditorGUILayout.HelpBox("StringTable - " + _nameTable + " exists. In this case, the table will be cleared and filled again. ", MessageType.Warning);
            }

            if ( string.IsNullOrEmpty(_nameTable) )
			{
                EditorGUILayout.HelpBox("Name table is empty", MessageType.Error);
                GUI.enabled = false;
            }
            ValidateLocalizationSettings();
            ValidateLocales();

            if (GUILayout.Button("Add localization"))
            {
                _infoLocalization = StartSearch();
            }

            if ( !string.IsNullOrEmpty(_infoLocalization) ) EditorGUILayout.HelpBox(_infoLocalization, MessageType.Info);
        }

        private void Dropdown_StringTables(int width = 300)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select string Table", GUILayout.Width(width));
            if (EditorGUILayout.DropdownButton(new GUIContent(_selectedTable), FocusType.Passive))
            {
                Rect posit = new Rect(new Vector2(width, 90), new Vector2(400, 20));
                genericMenu = new GenericMenu();
                foreach (string option in _sharedStringTables.Select(w => w.TableCollectionName))
                {
                    genericMenu.AddItem(new GUIContent(option), option == _selectedTable, () =>
                    {
                        _selectedTable = option;
                    });
                }
				genericMenu.AddItem(new GUIContent(KEYWORD_NEWTABLE), KEYWORD_NEWTABLE == _selectedTable, () =>
				{
					_selectedTable = KEYWORD_NEWTABLE;
				});
				genericMenu.DropDown(posit);
            }
            EditorGUILayout.EndHorizontal();
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

            parameters.Scene = _currentScene;
            parameters.SkipPrefab = _skipPrefab;
            parameters.SourceLocale = _selectedLocale;

            return SearchTextForLocalization.Search(parameters);
		}
    }
}