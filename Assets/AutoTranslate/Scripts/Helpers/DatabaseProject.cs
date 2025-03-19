using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EqualchanceGames.Tools.Helpers
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
            string[] guids = GetGuidScenes();
            Scene[] scenes = new Scene[EditorSceneManager.sceneCount];

            int index = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.AssetPathToGUID(guid);

                scenes[index] = EditorSceneManager.OpenScene(path);
            }
            return scenes;
        }

        public static string[] GetPathScenes()
        {
            string[] guids = GetGuidScenes();
            string[] paths = new string[guids.Length];

            int index = 0;
            foreach (string guid in guids)
            {
                paths[index] = AssetDatabase.GUIDToAssetPath(guid);
                ++index;
            }
            return paths;
        }

        public static List<Scene> GetScenes(List<string> names)
        {
            Scene[] scenes = GetScenes();
            List<Scene> activeScenes = new List<Scene>();
            foreach (Scene scene in scenes)
            {
                foreach (string name in names)
                {
                    if (scene.name == name)
                    {
                        activeScenes.Add(scene);
                        continue;
                    }
                }
            }
            return activeScenes;
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

        public static List<GameObject> GetPrefabs(List<string> names)
        {
            List<GameObject> gameObjects = GetPrefabs();
            List<GameObject> newgameObjects = new List<GameObject>();
            foreach (GameObject gameObject in gameObjects)
            {
                foreach (string name in names)
                {
                    if (gameObject.name == name)
                    {
                        newgameObjects.Add(gameObject);
                        continue;
                    }
                }
            }
            return newgameObjects;
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