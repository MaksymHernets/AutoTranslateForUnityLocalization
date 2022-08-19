using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Localization.Tables.SharedTableData;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class FindTextScene_EditorWindow : BaseLocalization_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Find text for Localization in Scene";

        // Parameters for execute
        private Scene _currentScene;
        private bool _isPrefab = true;
        private string _selectedTable = KEYWORD_NEWTABLE;
        private string _nameTable = string.Empty;

        // Temps
        private int _countText = 0;
        private int _countTextLocalization = 0;
        private string _infoLocalization = string.Empty;
        private int _countPrefabs = 0;

        private const string KEYWORD_NEWTABLE = "-New-";

        [MenuItem("Window/Auto Localization/Find Text for Tables in Scene")]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            FindTextScene_EditorWindow window = GetWindow<FindTextScene_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.FindTextScene);
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

            DropDownTables(k_SeparationWidth);

            if ( _selectedTable == KEYWORD_NEWTABLE)
			{
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New string table", GUILayout.Width(k_SeparationWidth));
                _nameTable = EditorGUILayout.TextField("", _nameTable);
                EditorGUILayout.EndHorizontal();
            }
            
            SourceLanguage(k_SeparationWidth);

            EditorGUIUtility.labelWidth = k_SeparationWidth;
            _isPrefab = EditorGUILayout.Toggle("Add localization for prefabs ( as an addition to the prefab )", _isPrefab);

            if (GUILayout.Button("Find text components"))
            {
                GameObject[] gameObjects = SimpleDatabaseProject.GetGameObjects(_currentScene.name);
                List<Text> texts = GetGameObjectsText(gameObjects);
                _countPrefabs = 0;
                foreach (Text text in texts)
                {
                    if( PrefabUtility.IsPartOfAnyPrefab(text.gameObject) ) ++_countPrefabs;
                }
                _countTextLocalization = CheckTextAboutLocalization(texts);
                _countText = texts.Count;
            }
            if ( _countText != 0 ) EditorGUILayout.HelpBox(" Found text: " + _countText.ToString() + "\n"
                + " Found localize string event: " + _countTextLocalization + "\n"
                + " Prefabs: " + _countPrefabs, MessageType.Info);
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
                _infoLocalization = Localization();
            }
            if ( !string.IsNullOrEmpty(_infoLocalization) ) EditorGUILayout.HelpBox(_infoLocalization, MessageType.Info);
        }

        private void DropDownTables(int width = 300)
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

        private List<Text> GetGameObjectsText(GameObject[] mainGameObjects)
		{
            List<Text> listsText = new List<Text>();
            foreach (var gameobject in mainGameObjects)
            {
                listsText.AddRange(gameobject.GetComponentsInChildren<Text>());
            }
            return listsText;
        }

        private int CheckTextAboutLocalization(List<Text> listsText)
		{
            int count = 0;
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            foreach (var text in listsText)
            {
                bool key = text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent);
                if (key == true)
                {
                    ++count;
                }
            }
            return count;
		}

        private string Localization()
		{
            if ( string.IsNullOrEmpty(_nameTable) ) return "nameTable is null";

            GameObject[] gameObjects = SimpleDatabaseProject.GetGameObjects(_currentScene.name);
            if ( gameObjects.Length == 0 ) return "gameObjects is 0";

            List<Text> listsText = GetGameObjectsText(gameObjects);
            if ( listsText.Count == 0 ) return "texts is 0";

            string NameTable = string.Empty;
            if ( _selectedTable == KEYWORD_NEWTABLE)
			{
                NameTable = _nameTable;
            }
            else
			{
                NameTable = _selectedTable;
            }
            SharedTableData sharedTable = SimpleInterfaceStringTable.GetSharedTable(NameTable);

            if ( sharedTable != null )
            {
                while ( sharedTable.Entries.Count != 0 )
				{
                    sharedTable.RemoveKey(sharedTable.Entries[0].Id);
                }
			}
            else
			{
                sharedTable = SimpleInterfaceStringTable.AddStringTable(_nameTable);
            }

            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            StringTable stringTable = default(StringTable);
            bool key = false;
            foreach (var text in listsText)
            {
                if ( _isPrefab == false && PrefabUtility.IsPartOfAnyPrefab(text.gameObject) ) continue;

                key = text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent);
                if ( key )
                {
                    localizeStringEvent = text.gameObject.GetComponent<LocalizeStringEvent>();
                }
                else
				{
                    localizeStringEvent = text.gameObject.AddComponent<LocalizeStringEvent>();
                }
                string name = String.Format("[{0}][{1}]", text.gameObject.name, text.gameObject.transform.parent?.name);
                int variants = 1;
				while ( sharedTable.Contains(name) )
				{
                    name = String.Format("[{0}][{1}][{2}]", text.gameObject.name, text.gameObject.transform.parent?.name, variants);
                    ++variants;
                }
                sharedTableEntry = sharedTable.AddKey(name);

                stringTable = SimpleInterfaceStringTable.GetStringTable(sharedTable, _selectedLocale);
                if ( stringTable != null) stringTable.AddEntry(name, text.text);

                LocalizedString localizedString = new LocalizedString();
                localizedString.SetReference(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.StringReference = localizedString;
            }

            return "Completed";
        }
    }

    public class Row
	{
        public string name;
        public bool check;

        public Row(string name)
        {
            this.name = name;
            check = true;
        }

	}
}