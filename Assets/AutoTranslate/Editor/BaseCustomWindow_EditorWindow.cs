using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class BaseCustomWindow_EditorWindow : EditorWindow
    {
        // Window parameters
        protected int k_SeparationWidth = 200;

        //public static void ShowWindow()
        //{
        //    Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
        //    CleanupLocalization_EditorWindow window = GetWindow<CleanupLocalization_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
        //    window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.CleanupLocalization);
        //    window.Show();
        //}

        protected void ShowNameWindow(string name)
        {
            GUI.enabled = false;
            GUILayout.Button(name, GUILayout.Height(30));
            GUI.enabled = true;
            GUILayout.Space(10);
        }
    }
}