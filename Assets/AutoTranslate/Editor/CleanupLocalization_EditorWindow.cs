using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement; // For Unity 2019.4 !!!!
using UnityEngine.SceneManagement;
using GoodTime.Tools.Helpers;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class CleanupLocalization_EditorWindow : EditorWindow
    {
        private const string k_WindowTitle = "Clean up Localization";

        private Scene _currentScene;
        private PrefabStage _prefabStage;

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
            _currentScene = DatabaseProject.GetCurrentScene();
            _prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        }

        protected void OnFocus()
        {
            _currentScene = DatabaseProject.GetCurrentScene();
            _prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        }

        private void OnGUI()
        {
            GUI.enabled = false;
            GUILayout.Button(k_WindowTitle, GUILayout.Height(30));
            GUI.enabled = true;
            GUILayout.Space(10);

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
            GUILayout.Space(10);
            if (GUILayout.Button("Remove miss Localization"))
            {

            }
            GUILayout.Space(10);
            if (GUILayout.Button("Remove all Localization"))
            {

            }
        }
    }
}
