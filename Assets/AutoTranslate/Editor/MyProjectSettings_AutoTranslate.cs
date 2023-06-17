using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using GoodTime.Tools.InterfaceTranslate;
using GoodTime.Tools.GUIPro;
using GoodTime.Tools.Helpers;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
    public static class MyProjectSettings_AutoTranslate
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();

            DropdownGUI dropdownGUI = new DropdownGUI("Target service for auto translate",
                setting.ServiceApis.Select(w => w.Name).ToList(),
                setting.CurrentServiceTranslate.ToString(), 200);

            dropdownGUI.UpdateSelected += (name) => 
            { 
                setting.CurrentServiceTranslate = (TypeServiceTranslate)Enum.Parse(typeof(TypeServiceTranslate), name);
                setting.TempKeyForService = setting.GetCurrentKey();
            };

            SettingsProvider provider = new SettingsProvider("Project/Auto Translate", SettingsScope.Project)
            {
                label = "Auto Translate For Unity Localization",
                
                guiHandler = (searchContext) =>
                {
                    ServiceApi service = setting.GetCurrentService();

                    EditorGUILayout.Space(10);

                    if (!service.IsActive) EditorGUILayout.HelpBox("Not yet supported. In the plan to add a service for translating text.", MessageType.Error);

                    dropdownGUI.Draw();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Key for service", GUILayout.Width(200));
                    string key = EditorGUILayout.TextField("", setting.TempKeyForService);
                    if ( !string.Equals(key, setting.TempKeyForService))
					{
                        setting.SetCurrentKey(key);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Check service", GUILayout.Width(200)))
                    {
                        service.IsOnline = -1;
                    }

                    if (service.IsOnline == -2) { GUI.contentColor = Color.grey; EditorGUILayout.LabelField("Unknown"); }
                    else if (service.IsOnline == -1) { GUI.contentColor = Color.yellow; EditorGUILayout.LabelField("Cheking"); }
                    else if (service.IsOnline == 0) { GUI.contentColor = Color.green; EditorGUILayout.LabelField("Online"); }
                    else if (service.IsOnline == 1) { GUI.contentColor = Color.red; EditorGUILayout.LabelField("Offline"); }

                    GUI.contentColor = Color.white;
                    EditorGUILayout.EndHorizontal();

                    if ( !string.IsNullOrEmpty(service.Description) ) EditorGUILayout.HelpBox(service.Description, MessageType.Info);
                    if ( service.HasLimit ) EditorGUILayout.HelpBox("Has a limit on requests per day. Enter the key to remove the limit", MessageType.Warning);
                },

                keywords = new HashSet<string>(new[] { "Auto", "Translate", "Unity", "Localization", "Api" })
            };

            return provider;
        }
    }
}