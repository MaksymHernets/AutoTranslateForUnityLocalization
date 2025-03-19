using EqualchanceGames.Tools.GUIPro;
using EqualchanceGames.Tools.InterfaceTranslate;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EqualchanceGames.Tools.AutoTranslate.Editor
{
    public static class MyProjectSettings_AutoTranslate
    {
        public const int BaseIndex = 10000;

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

                    if (setting.CurrentServiceTranslate != TypeServiceTranslate.GoogleApiFree)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Key for service", GUILayout.Width(200));
                        string key = EditorGUILayout.TextField("", setting.TempKeyForService);
                        if (!string.Equals(key, setting.TempKeyForService))
                        {
                            setting.SetCurrentKey(key);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Check service", GUILayout.Width(200)))
                    {
                        service.IsOnline = 2;
                    }

                    if (service.IsOnline == (int)StatusConnect.Unknown) { GUI.contentColor = Color.grey; EditorGUILayout.LabelField(StatusConnect.Unknown.ToString()); }
                    else if (service.IsOnline == (int)StatusConnect.Cheking) { GUI.contentColor = Color.yellow; EditorGUILayout.LabelField(StatusConnect.Cheking.ToString()); }
                    else if (service.IsOnline == (int)StatusConnect.Online) { GUI.contentColor = Color.green; EditorGUILayout.LabelField(StatusConnect.Online.ToString()); }
                    else if (service.IsOnline == (int)StatusConnect.Offline) { GUI.contentColor = Color.red; EditorGUILayout.LabelField(StatusConnect.Offline.ToString()); }

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

    public enum StatusConnect
    {
        Unknown = -2,
        Cheking = -1,
        Online = 0,
        Offline = 1,
        Error = 2
    }
}