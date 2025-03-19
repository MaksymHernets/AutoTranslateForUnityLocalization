using UnityEditor;
using UnityEngine;

namespace EqualchanceGames.Tools.GUIPro
{
    public static class LinesGUI
    {
        public static void DrawTexts(string name, string name2, int width = 150)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.Width(width));
            EditorGUILayout.LabelField(name2);
            EditorGUILayout.EndHorizontal();
        }

        public static bool DrawLineToggle(string name, bool key, int width = 200)
        {
            bool keyout = false;
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(name, GUILayout.Width(width));
            keyout = EditorGUILayout.Toggle(key);
            EditorGUILayout.EndHorizontal();
            return keyout;
        }

        public static float DrawLineFloat(string name, float key)
        {
            float keyout = 0;
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(name);
            keyout = EditorGUILayout.FloatField(key);
            EditorGUILayout.EndHorizontal();
            return keyout;
        }

        public static int DrawLineInt(string name, int key)
        {
            int keyout = 0;
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(name);
            keyout = EditorGUILayout.IntField(key);
            EditorGUILayout.EndHorizontal();
            return keyout;
        }

        public static float DrawLineSlider(string name, float key, int min = 0, int max = 5)
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