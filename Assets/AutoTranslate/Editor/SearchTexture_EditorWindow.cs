using System;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class SearchTexture_EditorWindow : EditorWindow
    {
        private const string k_WindowTitle = "Search Texture";

        [MenuItem("Window/Auto Localization/Search Texture", false, 42)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchTexture_EditorWindow window = GetWindow<SearchTexture_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchTexture);
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
