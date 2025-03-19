using EqualchanceGames.Tools.GUIPro;
using EqualchanceGames.Tools.Helpers;
using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace EqualchanceGames.Tools.AutoTranslate.Windows
{
    public class BaseCustomWindow_EditorWindow : EditorWindow
    {
        protected UnityEditor.SceneManagement.PrefabStage _prefabStage;
        protected Scene _currentScene;

        // Window parameters
        protected int k_SeparationWidth = 260;

        //public static void ShowWindow()
        //{
        //    Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
        //    CleanupLocalization_EditorWindow window = GetWindow<CleanupLocalization_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
        //    window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.CleanupLocalization);
        //    window.Show();
        //}

        protected virtual void OnEnable()
        {
            _currentScene = DatabaseProject.GetCurrentScene();
            _prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        }

        protected virtual void OnFocus()
        {
            _currentScene = DatabaseProject.GetCurrentScene();
            _prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        }

        protected void ShowNameWindow(string name)
        {
            GUI.enabled = false;
            GUILayout.Button(name, GUILayout.Height(30));
            GUI.enabled = true;
            GUILayout.Space(10);
        }

        protected void Show_CurrentOpen()
        {
            if (_prefabStage == null)
            {
                LinesGUI.DrawTexts("Current Scene", _currentScene.name, k_SeparationWidth);
            }
            else
            {
                LinesGUI.DrawTexts("Current Prefab", _prefabStage.prefabContentsRoot.name, k_SeparationWidth);
            }
        }
    }
}