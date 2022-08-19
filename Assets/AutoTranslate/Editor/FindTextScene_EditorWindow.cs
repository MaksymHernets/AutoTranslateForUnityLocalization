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
        private string _nameTable = string.Empty;

        // Temps
        private int _countText = 0;
        private int _countTextLocalization = 0;
        private string _infoLocalization = string.Empty;

        [MenuItem("Window/Auto Localization/Find Text for Tables in Scene")]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            FindTextScene_EditorWindow window = GetWindow<FindTextScene_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle);
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

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name string table", GUILayout.Width(k_SeparationWidth));
            _nameTable = EditorGUILayout.TextField("", _nameTable);
            EditorGUILayout.EndHorizontal();

            SourceLanguage(k_SeparationWidth);

            EditorGUIUtility.labelWidth = k_SeparationWidth;
            _isPrefab = EditorGUILayout.Toggle("Add localization for prefabs ( as an addition to the prefab )", _isPrefab);

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
            ValidLocalization();
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