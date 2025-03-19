using EqualchanceGames.Tools.AutoTranslate.Editor;
using EqualchanceGames.Tools.GUIPro;
using EqualchanceGames.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EqualchanceGames.Tools.AutoTranslate.Windows
{
    public class CleanupLocalizationPrefabs_EditorWindow : BaseCustomWindow_EditorWindow
    {
        private const string k_WindowTitle = "Clean up Localization in Prefabs";

        private CheckListGUI _checkListPrefabs;
        private CheckListGUI _checkListComponents;

        protected SearchTextParameters _searchTextParameters;

        [MenuItem("Auto Localization/Clean up Localization in Prefabs", false, MyProjectSettings_AutoTranslate.BaseIndex + 81)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            CleanupLocalizationPrefabs_EditorWindow window = GetWindow<CleanupLocalizationPrefabs_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.CleanupLocalization);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _searchTextParameters = new SearchTextParameters();
            Dictionary<string, bool> list = new Dictionary<string, bool>();
            foreach (var item in SearchTextForLocalization.GetAvailableForSearchUIComponents())
            {
                list.Add(item.Name, true);
            }
            _searchTextParameters.ListSearchComponents = list;

            List<GameObject> gameObjects = DatabaseProject.GetPrefabs();
            _checkListPrefabs = new CheckListGUI(gameObjects.Select(w => w.name).ToList());
            _checkListPrefabs.Width = 150;
            _checkListPrefabs.Height = 1000;

            List<string> list2 = new List<string>();
            list2.Add("LocalizeStringEvent");
            list2.Add("LocalizeSpriteEvent");
            list2.Add("LocalizeTextureEvent");
            list2.Add("LocalizeAudioClipEvent");
            list2.Add("LocalizedGameObjectEvent");
            _checkListComponents = new CheckListGUI(list2);
            _checkListComponents.Height = 150;
        }

        protected override void OnFocus()
        {
            base.OnFocus();

            List<GameObject> gameObjects = DatabaseProject.GetPrefabs();
            _checkListPrefabs.Update(gameObjects.Select(w => w.name).ToList());
            //_checkListPrefabs.Width = 150;
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)); // Main Begin 

            EditorGUILayout.BeginVertical();
            _checkListPrefabs.DrawButtons("Prefabs:");
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(); // Begin 1
            Show_CurrentOpen();
            GUILayout.Space(10);
            _searchTextParameters.SkipPrefab = LinesGUI.DrawLineToggle("Skip sub prefabs", _searchTextParameters.SkipPrefab);
            _searchTextParameters.SkipEmptyText = LinesGUI.DrawLineToggle("Skip empty text", _searchTextParameters.SkipEmptyText);
            _searchTextParameters.IsRemoveMiss_StringTable = LinesGUI.DrawLineToggle("Remove only miss stringEvents", _searchTextParameters.IsRemoveMiss_StringTable);
            GUILayout.Space(10);
            _checkListComponents.DrawButtons("Remove Localization Components:");
            GUILayout.Space(10);
            if (GUILayout.Button("Remove Localization"))
            {
                StartRemoveLocalization();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        public void StartRemoveLocalization()
        {
            try
            {
                EditorUtility.DisplayProgressBar("Cleanup Localization in Prefabs", "Load prefabs", 0);

                _searchTextParameters.ListSearchComponents = _checkListComponents.GetElements(true, true);
                List<GameObject> gameObjects = DatabaseProject.GetPrefabs(_checkListPrefabs.GetNames(true, true));

                float dola = gameObjects.Count * 0.1f;
                int index = 0;

                foreach (GameObject gameObject in gameObjects)
                {
                    EditorUtility.DisplayProgressBar("Cleanup Localization in Prefabs", "Prefabs: " + gameObject.name, index * dola);

                    ClearUpLocalization.Execute(_searchTextParameters, gameObject);

                    EditorUtility.SetDirty(gameObject);
                }
                ++index;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
            Debug.Log("Completed Remove Localization for prefabs");
        }
    }
}
