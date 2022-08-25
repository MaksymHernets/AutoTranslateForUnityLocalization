using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.Helpers
{
    public static class GameObjectHelper
    {
        public static List<T> GetComponentsInChildrens<T>(GameObject[] mainGameObjects)
        {
            List<T> listsText = new List<T>();
            foreach (GameObject gameobject in mainGameObjects)
            {
                listsText.AddRange(gameobject.GetComponentsInChildren<T>());
            }
            return listsText;
        }

        public static GameObject[] GetSubGameObjects(this GameObject gameObject)
        {
            return gameObject.GetComponentsInChildren<Transform>().Select(w => w.gameObject).ToArray();
        }

        public static List<GameObject> DetectPrefabs(GameObject[] gameObjects)
        {
            List<GameObject> prefabs = new List<GameObject>();
            foreach (GameObject gameObject in gameObjects)
            {
                GameObject[] subGameObjects = GameObjectHelper.GetSubGameObjects(gameObject);
                foreach (GameObject subGameObject in subGameObjects)
                {
                    if (PrefabUtility.IsAnyPrefabInstanceRoot(subGameObject))
                    {
                        prefabs.Add(subGameObject);
                    }
                }
            }
            return prefabs;
        }
    }
}