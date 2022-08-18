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
	public class FindTextScene_EditorWindow : EditorWindow
    {
        private const string k_WindowTitle = "Find text for Localization in Scene";

        private Scene _currentScene;
        private bool _isPrefab = true;
        private List<Row> _lists;
        private int _countText = 0;
        private int _countTextLocalization = 0;
        private string _nameTable = string.Empty;
        private string _infoLocalization = string.Empty;
        private Locale _selectedLocale;
        private GenericMenu genericMenu;
        private List<Locale> _locales;
        private string _selectedLanguage = string.Empty;

        [MenuItem("Window/Auto Localization/Find Text for Tables in Scene")]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            FindTextScene_EditorWindow window = GetWindow<FindTextScene_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle);
            window.Show();
        }

        public void OnEnable()
        {
            _locales = SimpleInterfaceLocalization.GetAvailableLocales();
            _selectedLocale = SimpleInterfaceLocalization.GetSelectedLocale();

            if (_selectedLocale != null)
            {
                _selectedLanguage = _selectedLocale.LocaleName;
            }
            else
            {
                _selectedLanguage = string.Empty;
            }

            _currentScene = SimpleDatabaseProject.GetCurrentScene();
            _lists = new List<Row>();
            _lists.Add(new Row("Text Legacy"));
            _lists.Add(new Row("Dropdown"));
            _nameTable = "StringTable_" + _currentScene.name + "_Scene";
        }

        private void OnFocus()
        {
            _locales = SimpleInterfaceLocalization.GetAvailableLocales();
            _currentScene = SimpleDatabaseProject.GetCurrentScene();
            _infoLocalization = string.Empty;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(k_WindowTitle, GUILayout.Width(300));
            GUILayout.Space(10);

            EditorGUILayout.LabelField("Current Scene: " + _currentScene.name, GUILayout.Width(300));
            _nameTable = EditorGUILayout.TextField("Name table:", _nameTable);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Source language", GUILayout.Width(300));
            var posit = new Rect(new Vector2(310, 0), new Vector2(200, 20));
            if (EditorGUILayout.DropdownButton(new GUIContent(_selectedLanguage), FocusType.Passive))
            {
                genericMenu = new GenericMenu();

                foreach (var option in _locales.Select(w => w.name))
                {
                    bool selected = option == _selectedLanguage;
                    genericMenu.AddItem(new GUIContent(option), selected, () =>
                    {
                        _selectedLanguage = option;
                    });
                }
                genericMenu.DropDown(posit);
            }
            EditorGUILayout.EndHorizontal();

            _isPrefab = EditorGUILayout.ToggleLeft("Add localization for prefabs ( as an addition to the prefab )", _isPrefab);

            EditorGUILayout.LabelField("Selected text components need to translate:", GUILayout.Width(300));
   //         EditorGUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(10, 10, 10, 10) });
			//foreach (Row row in _lists)
			//{
			//	row.check = EditorGUILayout.ToggleLeft( row.name, row.check );
			//}
			//EditorGUILayout.EndVertical();

            if (GUILayout.Button("Find text components"))
            {
                GameObject[] gameObjects = SimpleDatabaseProject.GetGameObjects(_currentScene.name);
                List<Text> texts = GetGameObjectsText(gameObjects);
                _countTextLocalization = CheckTextAboutLocalization(texts);
                _countText = texts.Count;
            }
            if ( _countText != 0 ) EditorGUILayout.HelpBox(" Found text: " + _countText.ToString() + "\n"
                + " Found localize string event: " + _countTextLocalization, MessageType.Info);
            if (SimpleInterfaceStringTable.CheckNameStringTable(_nameTable))
            {
                EditorGUILayout.HelpBox("StringTable - " + _nameTable + " exists. In this case, the table will be cleared and filled again. ", MessageType.Warning);
            }
            if ( string.IsNullOrEmpty(_nameTable) )
			{
                EditorGUILayout.HelpBox("Name table is empty", MessageType.Error);
                GUI.enabled = false;
            }
            if (GUILayout.Button("Add localization"))
            {
                _infoLocalization = Localization();
            }
            if ( !string.IsNullOrEmpty(_infoLocalization) ) EditorGUILayout.HelpBox(_infoLocalization, MessageType.Info);
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

            SharedTableData sharedTable = SimpleInterfaceStringTable.GetSharedTable(_nameTable);
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
                key = text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent);
                if ( key )
                {
                    localizeStringEvent = text.gameObject.GetComponent<LocalizeStringEvent>();
                }
                else
				{
                    localizeStringEvent = text.gameObject.AddComponent<LocalizeStringEvent>();
                }

                string name = text.gameObject.name + text.gameObject.transform.parent?.name;
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