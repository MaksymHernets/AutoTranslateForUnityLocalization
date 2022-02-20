using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using GoodTime.Tools.InterfaceTranslate;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
    public static class MyProjectSettings_AutoTranslate
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Auto Translate", SettingsScope.Project)
            {
                label = "Auto Translate",

                guiHandler = (searchContext) =>
                {
                    AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target platform for auto translate", GUILayout.Width(200));

                    var posit = new Rect(new Vector2(210, 10), new Vector2(200, 10));
                    if (EditorGUILayout.DropdownButton(new GUIContent(setting.PlatformForTranslate.ToString()), FocusType.Passive))
                    {
                        GenericMenu genericMenu = new GenericMenu();

                        foreach (var option in setting.Platforms)
                        {
                            bool selected = option == setting.PlatformForTranslate.ToString();
                            genericMenu.AddItem(new GUIContent(option), selected, () =>
                            {
                                setting.PlatformForTranslate = (TypePlatformTranslate)Enum.Parse(typeof(TypePlatformTranslate), option);
                            });
                        }
                        genericMenu.DropDown(posit);
                    }
                    EditorGUILayout.EndHorizontal();
                },

                keywords = new HashSet<string>(new[] { "Auto", "Translate", "Unity", "Localization", "Api" })
            };

            return provider;
        }
    }
}