using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public class EditorIcons
    {
        public static Texture AutoTranslate { get; private set; }
        public static Texture SearchTextScene { get; private set; }
        public static Texture SearchTextPrefab { get; private set; }
        public static Texture CleanupLocalization { get; private set; }

        static EditorIcons()
        {
            AutoTranslate = GetTexture("d_CustomTool@2x");
            SearchTextScene = GetTexture("d_AlphabeticalSorting@2x");
            SearchTextPrefab = GetTexture("d_AlphabeticalSorting@2x");
            CleanupLocalization = GetTexture("ClothInspector.PaintTool");
        }

        static Texture GetTexture(string path) => EditorGUIUtility.IconContent(path).image;
    }
}