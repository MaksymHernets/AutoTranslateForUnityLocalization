using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.GUIPro;
using GoodTime.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class CleanupLocalizationScenes_EditorWindow : BaseCustomWindow_EditorWindow
    {
        private const string k_WindowTitle = "Clean up Localization in Scenes";

        private CheckListGUI _checkListScenes;
        private CheckListGUI _checkListComponents;

        protected SearchTextParameters _searchTextParameters;

        [MenuItem("Window/Auto Localization/Clean up Localization in Scenes", false, MyProjectSettings_AutoTranslate.BaseIndex + 80)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            CleanupLocalizationScenes_EditorWindow window = GetWindow<CleanupLocalizationScenes_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.CleanupLocalization);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _searchTextParameters = new SearchTextParameters();
            Dictionary<string, bool> list = new Dictionary<string, bool>();
            foreach (var item in SearchTextForLocalization.GetAvailableForSearchUIElements())
			{
                list.Add(item.Name, true);
            }
            _searchTextParameters.Lists = list;

            Scene[] scenes = DatabaseProject.GetScenes();
            _checkListScenes = new CheckListGUI(scenes.Select(w => w.name).ToList());

            List<string> list2 = new List<string>();
            list2.Add("LocalizeStringEvent");
            list2.Add("LocalizeSpriteEvent");
            list2.Add("LocalizeTextureEvent");
            list2.Add("LocalizeAudioClipEvent");
            list2.Add("LocalizedGameObjectEvent");
            _checkListComponents = new CheckListGUI(list2);
        }

        protected override void OnFocus()
        {
            base.OnFocus();

            Scene[] scenes = DatabaseProject.GetScenes();
            _checkListScenes = new CheckListGUI(scenes.Select(w => w.name).ToList());
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.MinHeight(400)); // Main Begin 
            
            EditorGUILayout.BeginFadeGroup(1); // Begin 0
            _checkListScenes.DrawButtons("Scenes:");
            EditorGUILayout.EndFadeGroup();

            EditorGUILayout.BeginVertical(); // Begin 1
            Show_CurrentOpen();
            GUILayout.Space(10);
            _searchTextParameters.SkipPrefab = LinesGUI.DrawLineToggle("Skip prefabs", _searchTextParameters.SkipPrefab);
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
                EditorUtility.DisplayProgressBar("Cleanup Localization in Scenes", "Load scenes", 0);

                _searchTextParameters.Lists = _checkListComponents.GetElements(true, true);
                List<Scene> scenes = DatabaseProject.GetScenes(_checkListScenes.GetNames(true, true));

                float dola = scenes.Count * 0.1f;
                int index = 0;

                foreach (Scene scene in scenes)
                {
                    EditorUtility.DisplayProgressBar("Cleanup Localization in Scenes", "Scene: " + scene.name, index * dola);

                    Scene openScene = EditorSceneManager.OpenScene(scene.path);

                    ClearUpLocalization.Execute(_searchTextParameters, openScene);

                    //EditorSceneManager.MarkSceneDirty(_currentScene);
                    //EditorSceneManager.SaveScene(_currentScene);

                    EditorSceneManager.SaveOpenScenes();
                }
                ++index;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
