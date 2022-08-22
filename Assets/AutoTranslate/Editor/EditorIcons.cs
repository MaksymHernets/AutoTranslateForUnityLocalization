using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public class EditorIcons
    {
        public static Texture AutoTranslate { get; private set; }
        public static Texture FindTextScene { get; private set; }

        static EditorIcons()
        {
            AutoTranslate = GetTexture("d_CustomTool@2x");
            FindTextScene = GetTexture("d_AlphabeticalSorting@2x");
        }

        static Texture GetTexture(string path) => EditorGUIUtility.IconContent(path).image;
    }
}