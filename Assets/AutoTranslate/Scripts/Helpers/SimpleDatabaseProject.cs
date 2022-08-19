using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SimpleDatabaseProject
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
        
        //SceneManager.LoadScene(scene.name);
        //EditorSceneManager.OpenScene(lists[0].name);
        return scene.GetRootGameObjects();
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
