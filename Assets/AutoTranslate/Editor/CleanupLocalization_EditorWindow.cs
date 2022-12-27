using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement; // For Unity 2019.4 !!!!
using UnityEngine.SceneManagement;
using GoodTime.Tools.Helpers;
using System.Linq;
using System.Collections.Generic;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class CleanupLocalization_EditorWindow : BaseCustomWindow_EditorWindow
    {
        private const string k_WindowTitle = "Clean up Localization";

        private Scene _currentScene;
        private PrefabStage _prefabStage;

        protected StatusLocalizationScene _statusLocalizationScene;
        protected SearchTextParameters _searchTextParameters;

        [MenuItem("Window/Auto Localization/Clean up Localization", false, 80)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            CleanupLocalization_EditorWindow window = GetWindow<CleanupLocalization_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.CleanupLocalization);
            window.Show();
        }

        protected void OnEnable()
        {
            _searchTextParameters = new SearchTextParameters();
            Dictionary<string, bool> list = new Dictionary<string, bool>();
            foreach (var item in SearchTextForLocalization.GetAvailableForSearchUIElements())
			{
                list.Add(item.Name, true);
            }
            _searchTextParameters.Lists = list;

            _currentScene = DatabaseProject.GetCurrentScene();
            _prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            Check();
        }

        protected void OnFocus()
        {
            _currentScene = DatabaseProject.GetCurrentScene();
            _prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            Check();
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            if (_prefabStage == null)
			{
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Current Scene", GUILayout.Width(200));
                EditorGUILayout.LabelField(_currentScene.name);
                EditorGUILayout.EndHorizontal();
            }
            else
			{
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Current Prefab", GUILayout.Width(200));
                EditorGUILayout.LabelField(_prefabStage.prefabContentsRoot.name);
                EditorGUILayout.EndHorizontal();
            }

            if (_statusLocalizationScene != null)
            {
                EditorGUILayout.HelpBox(_statusLocalizationScene.ToString(), MessageType.Info);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Remove miss Localization"))
            {

            }
            GUILayout.Space(10);
            if (GUILayout.Button("Remove all Localization"))
            {

            }
        }

        private void Check()
		{
            if (_prefabStage == null) _statusLocalizationScene = SearchTextForLocalization.Search(_currentScene, _searchTextParameters);
            else _statusLocalizationScene = SearchTextForLocalization.Search(_prefabStage.prefabContentsRoot, _searchTextParameters);
        }
    }
}
