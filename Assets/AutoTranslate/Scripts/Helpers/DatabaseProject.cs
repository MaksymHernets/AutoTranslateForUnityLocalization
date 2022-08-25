using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GoodTime.Tools.Helpers
{
    public static class DatabaseProject
    {
        public static string[] GetGuidScenes()
        {
            return AssetDatabase.FindAssets("t:Scene");
        }

        public static List<SceneAsset> GetSceneAssets()
        {
            string[] guids = GetGuidScenes();
            List<SceneAsset> lists = new List<SceneAsset>();
            string path;
            foreach (var item in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(item);
                lists.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
            }
            return lists;
        }

        public static Scene[] GetScenes()
        {
            Scene[] scenes = new Scene[EditorSceneManager.sceneCountInBuildSettings];

            for (int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; ++i)
            {
                scenes[i] = EditorSceneManager.GetSceneAt(i);
            }
            return scenes;
        }

        public static void OpenScene(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EditorSceneManager.OpenScene(path);
        }

        public static GameObject[] GetGameObjects(string nameScene)
        {
            Scene scene = EditorSceneManager.GetSceneByName(nameScene);
            return scene.GetRootGameObjects();
        }

        public static List<GameObject> GetPrefabs()
        {
            string[] guids = AssetDatabase.FindAssets("t:prefab");
            List<GameObject> gameObjects = new List<GameObject>();
            string path = string.Empty;
            if (guids.Length != 0)
            {
                foreach (string guid in guids)
                {
                    path = AssetDatabase.GUIDToAssetPath(guid);
                    gameObjects.Add(AssetDatabase.LoadAssetAtPath<GameObject>(path));
                }
            }
            return gameObjects;
        }

        public static void SaveScene(Scene scene)
        {
            EditorSceneManager.SaveScene(scene);
        }

        public static Scene GetCurrentScene()
        {
            return EditorSceneManager.GetActiveScene();
        }
    }
}