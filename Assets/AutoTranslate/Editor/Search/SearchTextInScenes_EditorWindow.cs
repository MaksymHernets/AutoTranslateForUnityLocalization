using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.GUIPro;
using GoodTime.Tools.Helpers;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class SearchTextInScenes_EditorWindow : BaseSearch_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search Text in Scenes";

        private CheckListGUI _checkListScenes;

        [MenuItem("Window/Auto Localization/Search Text in Scenes", false, MyProjectSettings_AutoTranslate.BaseIndex + 41)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchTextInScenes_EditorWindow window = GetWindow<SearchTextInScenes_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchText);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            string[] scenes = DatabaseProject.GetPathScenes();
            _checkListScenes = new CheckListGUI(scenes.ToList());
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            string[] scenes = DatabaseProject.GetPathScenes();
            _checkListScenes.Update(scenes.ToList());
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.MinHeight(170)); // Main Begin 
            EditorGUILayout.BeginFadeGroup(1); // Begin 0
            _checkListScenes.DrawButtons("Scenes:");
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.BeginFadeGroup(2); // Begin 1
            _skipPrefab = LinesGUI.DrawLineToggle("Skip prefabs", _skipPrefab);
            _skipEmptyText = LinesGUI.DrawLineToggle("Skip empty text", _skipEmptyText);
            _removeMissStringEvents = LinesGUI.DrawLineToggle("Remove miss stringEvents", _removeMissStringEvents);
            _autoSave = LinesGUI.DrawLineToggle("Auto Save", _autoSave);

            EditorGUILayout.HelpBox("Not working yet", MessageType.Error);
            GUI.enabled = false;
            if (GUILayout.Button("Add localization"))
            {
                StartAddLocalization();
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndHorizontal();
        }

        private string StartAddLocalization()
        {
            //AddLocalizationParameters parameters = new AddLocalizationParameters();

            //if (_dropdownTables.Selected == KEYWORD_NEWTABLE) parameters.NameTable = _nameTable;
            //else parameters.NameTable = _dropdownTables.Selected;

            //if (string.IsNullOrEmpty(parameters.NameTable)) return "nameTable is null";

            //parameters.IsSkipPrefab = _skipPrefab;
            //parameters.IsSkipEmptyText = _skipEmptyText;
            //parameters.SourceLocale = _selectedLocale;
            //parameters.Lists = _checkListSearchElements.GetElements();

            //if (_statusLocalizationScene == null) StartSearch();
            //else GetCheckTable();

            //AddLocalization.Execute(parameters, _statusLocalizationScene);
            //if (_removeMissStringEvents) AddLocalization.RemoveMiss_LocalizeStringEvent(_statusLocalizationScene.LocalizeStringEvents);
            //if (_autoSave)
            //{
            //    EditorSceneManager.SaveOpenScenes();
            //    EditorUtility.SetDirty(_prefabStage.prefabContentsRoot);
            //}

            return "Completed";
        }
    }
}