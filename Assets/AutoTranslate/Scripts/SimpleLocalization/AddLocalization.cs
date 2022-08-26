using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Localization.Tables.SharedTableData;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public static class AddLocalization
    {
        public static string Execute(AddLocalizationParameters parameters, StatusLocalizationScene statusLocalizationScene)
        {
            SharedTableData sharedTable = SharedTableDataExtension.GetOrAdd_SharedTableData(parameters.NameTable);
            StringTable stringTable = SimpleInterfaceStringTable.GetStringTable(sharedTable, parameters.SourceLocale);

            if (parameters.Lists.ContainsKey("Text Legacy") && parameters.Lists["Text Legacy"])
            {
                AddLocalization_TextLegacy(statusLocalizationScene.LegacyTexts, stringTable, sharedTable);
            }
            if (parameters.Lists.ContainsKey("Text Mesh Pro") && parameters.Lists["Text Mesh Pro"])
            {
                AddLocalization_TextMeshPro(statusLocalizationScene.TextMeshs, stringTable, sharedTable);
            }

            return "Completed";
        }

        private static void AddLocalization_TextLegacy(List<Text> texts, StringTable stringTable, SharedTableData sharedTable)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            foreach (Text text in texts)
            {
                localizeStringEvent = GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableData_AddEntry(sharedTable, text.gameObject.name, text.transform.parent?.name, "TextLegacy");

                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                localizeStringEvent.Clear_OnUpdateString();
                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.Sign_OnUpdateString_TextLegacy(text);
            }
        }

        private static void AddLocalization_TextMeshPro(List<TextMeshProUGUI> texts, StringTable stringTable, SharedTableData sharedTable)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            foreach (TextMeshProUGUI text in texts)
            {
                localizeStringEvent = GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableData_AddEntry(sharedTable, text.gameObject.name, text.transform.parent?.name, "TextMeshPro");

                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                localizeStringEvent.Clear_OnUpdateString();
                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.Sign_OnUpdateString_TextMeshPro(text);
            }
        }

        private static void AddLocalization_DropdownLegacy(List<Dropdown> dropdowns, StringTable stringTable, SharedTableData sharedTable)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            foreach (Dropdown text in dropdowns)
            {
                localizeStringEvent = GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableData_AddEntry(sharedTable, text.gameObject.name, text.transform.parent?.name, "TextMeshPro");

                stringTable.AddEntry( sharedTableEntry.Key, text.GetLineOptions() );

                localizeStringEvent.Clear_OnUpdateString();
                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.Sign_OnUpdateString_DropdownLegacy(text);
            }
        }

        private static LocalizeStringEvent GetOrAdd_LocalizeStringEventComponent(GameObject gameObject)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            if (gameObject.TryGetComponent<LocalizeStringEvent>(out localizeStringEvent))
                return gameObject.GetComponent<LocalizeStringEvent>();
            else
                return gameObject.AddComponent<LocalizeStringEvent>();
        }

        private static SharedTableEntry SharedTableData_AddEntry(SharedTableData sharedTable, string nameGameObjet, string parentName, string typeText)
        {
            string name = String.Format("[{0}][{1}][{2}]", nameGameObjet, parentName, typeText);
            int variants = 1;
            while (sharedTable.Contains(name))
            {
                name = String.Format("[{0}][{1}][{2}][{3}]", nameGameObjet, parentName, typeText, variants);
                ++variants;
            }
            SharedTableEntry sharedTableEntry = sharedTable.AddKey(name);
            return sharedTableEntry;
        }
    }
}