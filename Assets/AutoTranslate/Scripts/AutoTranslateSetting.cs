using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.InterfaceTranslate
{
    public class AutoTranslateSetting : ScriptableObject
    {
        public const string k_MyCustomSettingsPath = "Assets/AutoTranslate/ProjectSettings/AutoTranslateSetting.asset";

        [SerializeField] public TypePlatformTranslate PlatformForTranslate;
        [SerializeField] public string KeyForService = string.Empty;

        [NonSerialized] public string[] Platforms;

        public static AutoTranslateSetting GetOrCreateSettings()
        {
            string[] guids = AssetDatabase.FindAssets("AutoTranslateSetting t:AutoTranslateSetting", null);
            string path = string.Empty;

            if (guids.Length != 0) path = AssetDatabase.GUIDToAssetPath(guids[0]);
            else return null;

            AutoTranslateSetting settings = AssetDatabase.LoadAssetAtPath<AutoTranslateSetting>(path);

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<AutoTranslateSetting>();
                
                AssetDatabase.CreateAsset(settings, k_MyCustomSettingsPath);
                AssetDatabase.SaveAssets();

                settings.Platforms = GetNamePlatformTranslate();
                if (settings.Platforms.Length != 0)
                {
                    settings.PlatformForTranslate = TypePlatformTranslate.GoogleApiFree;
                }
            }

            settings.Platforms = GetNamePlatformTranslate();

            return settings;
        }

        public static string[] GetNamePlatformTranslate()
		{

            return Enum.GetNames(typeof(TypePlatformTranslate));
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}
