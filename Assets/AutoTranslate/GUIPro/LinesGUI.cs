using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.GUIPro
{
    public static class LinesGUI
    {
        private static bool DrawLineCheck(string name, bool key)
        {
            bool keyout = false;
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(name);
            keyout = EditorGUILayout.Toggle(key);
            EditorGUILayout.EndHorizontal();
            return keyout;
        }

        private static float DrawLineFloat(string name, float key)
        {
            float keyout = 0;
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(name);
            keyout = EditorGUILayout.FloatField(key);
            EditorGUILayout.EndHorizontal();
            return keyout;
        }

        private static int DrawLineInt(string name, int key)
        {
            int keyout = 0;
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(name);
            keyout = EditorGUILayout.IntField(key);
            EditorGUILayout.EndHorizontal();
            return keyout;
        }

        private static float DrawLineSlider(string name, float key, int min = 0, int max = 5)
        {
            float keyout = 0;
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(name);
            keyout = EditorGUILayout.Slider(key, min, max);
            EditorGUILayout.EndHorizontal();
            return keyout;
        }

    }
}