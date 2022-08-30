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

        public static string GetFullName(this GameObject gameObject)
        {
            string fullName = string.Empty;
            if ( gameObject.transform.parent != null) fullName = string.Format("[{0}][{1}]", gameObject.transform.parent.name, gameObject.name);
            else fullName = string.Format("[{0}][{1}]", "null", gameObject.name);
            return fullName;
        }

        public static string GetFullName(this GameObject gameObject, string text, int length = 70)
        {
            string subtext = string.Empty;
            if (text.Length < length) subtext = text.Substring(0, text.Length);
            else subtext = text.Substring(0, length);
            string fullName = string.Empty;
            if (gameObject.transform.parent != null) fullName = string.Format("[{0}][{1}][{2}]", gameObject.transform.parent.name, gameObject.name, subtext);
            else fullName = string.Format("[{0}][{1}][{2}]", "null", gameObject.name, text);
            return fullName;
        }
    }
}