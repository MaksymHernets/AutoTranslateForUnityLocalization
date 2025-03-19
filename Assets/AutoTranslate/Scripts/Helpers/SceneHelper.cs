using EqualchanceGames.Tools.Helpers;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace EqualchanceGames.Helpers
{
    public class SceneHelper : MonoBehaviour
    {
        public static void ClearScene(Scene scene)
        {
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            foreach (GameObject gameObject in rootGameObjects)
            {
                DestroyImmediate(gameObject);
            }
        }

        public static List<GameObject> UnboxModel(GameObject gameObject)
        {
            
            List<GameObject> sectors = new List<GameObject>();
            StaticEditorFlags flags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OffMeshLinkGeneration | StaticEditorFlags.ReflectionProbeStatic;

            gameObject.transform.position = Vector3.zero;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            
            GameObjectUtility.SetStaticEditorFlags(gameObject.gameObject, flags);

            while (gameObject.transform.childCount > 0)
            {
                Transform child = gameObject.transform.GetChild(0);
                sectors.Add(child.gameObject);
                child.parent = null;
            }
            return sectors;
        }

        public static List<MeshCollider> AddMeshColliderForMeshRender(Scene scene, bool convex = false, bool trigger = false)
        {
            GameObject[] gameObjects = scene.GetRootGameObjects();

            List<MeshCollider> meshColliders = new List<MeshCollider>();

            MeshRenderer meshRenderer = default(MeshRenderer);
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.TryGetComponent<MeshRenderer>(out meshRenderer);
                if ( meshRenderer != null)
                {
                    MeshCollider meshChild = gameObject.AddComponent<MeshCollider>();
                    //meshChild.convex = true;
                    meshChild.isTrigger = trigger;
                    meshColliders.Add(meshChild);
                }
                meshRenderer = default(MeshRenderer);
            }
            return meshColliders;
        }

        public static void ReplaceScale(Scene scene, float example, float replaced)
        {
            GameObject[] gameObjects = scene.GetRootGameObjects();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.transform.localScale.x == example || gameObject.transform.localScale.y == example || gameObject.transform.localScale.z == example)
                {
                    gameObject.transform.localScale = new Vector3(replaced, replaced, replaced);
                }
            }
        }

        public static void SetObjectsStatic(Scene scene)
        {
            StaticEditorFlags flags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OccludeeStatic | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OffMeshLinkGeneration | StaticEditorFlags.ReflectionProbeStatic;

            GameObject[] gameObjects = scene.GetRootGameObjects();

            foreach (GameObject gameObject in gameObjects)
            {
                GameObjectUtility.SetStaticEditorFlags(gameObject, flags);
            }
        }

        public static List<GameObject> GetAllObjects(Scene scene)
        {
            List<GameObject> allObjects = new List<GameObject>();

            GameObject[] rootGameObjects = scene.GetRootGameObjects();

            foreach (GameObject gameObject in rootGameObjects)
            {
                allObjects.AddRange(gameObject.GetSubGameObjects());
            }

            return allObjects;
        }

        public static List<GameObject> GetGameObject(Scene scene, string name)
        {
            List<GameObject> allObjects = GetAllObjects(scene);
            List<GameObject> selected = new List<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj != null && obj.name.Contains(name))
                {
                    selected.Add(obj);
                }
            }
            return selected;
        }

        public static void AddBoxCollider(List<GameObject> gameObjects, bool meshRendererEnabled = true, bool trigger = false, UnityAction<GameObject> action = null)
        {
            MeshRenderer meshRenderer = default(MeshRenderer);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.TryGetComponent<MeshRenderer>(out meshRenderer);
                if (meshRenderer != null)
                {
                    BoxCollider meshChild = gameObject.AddComponent<BoxCollider>();
                    meshChild.isTrigger = trigger;
                }
                meshRenderer.enabled = meshRendererEnabled;
                meshRenderer = default(MeshRenderer);
                action?.Invoke(gameObject);
            }
        }

        public static void DeleteGameObjects(Scene scene, List<string> names)
        {
            List<GameObject> gameObjects = GetAllObjects(scene);
            
            for (int i = 0; i < gameObjects.Count; ++i)
            {
                if (gameObjects[i] == null) continue;
                GameObject gameObject = gameObjects[i];
                foreach (string name in names)
                {
                    if (gameObject.name.Contains(name))
                    {
                        DestroyImmediate(gameObject);
                        break;
                    }
                }
            }
        }
    }
}