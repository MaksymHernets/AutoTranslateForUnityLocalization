using System;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class SearchAudio_EditorWindow : EditorWindow
    {
        private const string k_WindowTitle = "Search Audio";

        [MenuItem("Window/Auto Localization/Search Audio", false, 43)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchAudio_EditorWindow window = GetWindow<SearchAudio_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchAudio);
            window.Show();
        }

        private void OnEnable()
        {

        }

        private void OnFocus()
        {

        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Not working yet. In the plans...");
        }
    }
}