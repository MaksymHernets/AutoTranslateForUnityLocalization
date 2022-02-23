using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class TestWindow : EditorWindow
    {
        private const string k_WindowTitle = "Find text for Localization";

        [MenuItem("Window/Asset Management/Find Text for Tables")]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            TestWindow window = GetWindow<TestWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle);
            window.Show();
        }

        private void OnGUI()
        { 
            GUILayout.Space(10);
            if (GUILayout.Button("Translate"))
            {
                List<SceneAsset> lists = new List<SceneAsset>();

                string[] guids = AssetDatabase.FindAssets("t:Scene");

                foreach (var item in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(item);

                    lists.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));

                    
                }

                string path2 = AssetDatabase.GUIDToAssetPath(guids[0]);
                EditorSceneManager.OpenScene(path2);
                

                Scene scene = EditorSceneManager.GetSceneByName(lists[0].name);
                //SceneManager.LoadScene(scene.name);
                //EditorSceneManager.OpenScene(lists[0].name);
                GameObject[] mainGameObjects = scene.GetRootGameObjects();



                List<Text> listsText = new List<Text>();

                foreach (var gameobject in mainGameObjects)
                {
                    listsText.AddRange(gameobject.GetComponentsInChildren<Text>());
                }

                foreach (var text in listsText)
                {
                    LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
                    bool key = text.gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent);
                    if ( key == false)
                    {
                        text.gameObject.AddComponent<LocalizeStringEvent>();
                    }  
                }

                EditorSceneManager.SaveScene(scene);
                Debug.Log("Ok");
                //ButtonTranslate_Click();
            }
        }   
    }
}
