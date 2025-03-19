using EqualchanceGames.Tools.AutoTranslate.Editor;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using static UnityEngine.Localization.Tables.SharedTableData;

namespace EqualchanceGames.Tools.AutoTranslate
{
    public static class AddLocalization
    {
        //public static partial void Execute(AddLocalizationParameters parameters, StatusLocalizationScene statusLocalizationScene);

        public static void Execute(AddLocalizationParameters parameters, StatusLocalizationScene statusLocalizationScene)
        {
            SharedTableData sharedTable = SharedTableDataExtension.GetOrAdd_SharedTableData(parameters.NameTable);
            StringTable stringTable = SimpleInterfaceStringTable.GetStringTable(sharedTable, parameters.SourceLocale);

            if (parameters.Lists.ContainsKey("Text Legacy") && parameters.Lists["Text Legacy"])
            {
                AddLocalization_TextLegacy(statusLocalizationScene.LegacyTexts, stringTable, sharedTable);
            }
            if (parameters.Lists.ContainsKey("Text Mesh Pro UI") && parameters.Lists["Text Mesh Pro UI"])
            {
                AddLocalization_TextMeshProUI(statusLocalizationScene.TextMeshProUIs, stringTable, sharedTable);
            }
            if (parameters.Lists.ContainsKey("Text Mesh Pro") && parameters.Lists["Text Mesh Pro"])
            {
                AddLocalization_TextMeshPro(statusLocalizationScene.TextMeshPros, stringTable, sharedTable);
            }
            if (parameters.Lists.ContainsKey("Text Mesh Legacy") && parameters.Lists["Text Mesh Legacy"])
            {
                AddLocalization_TextMeshLegacy(statusLocalizationScene.LegacyMeshTexts, stringTable, sharedTable);
            }
        }

        private static void AddLocalization_TextLegacy(List<Text> texts, StringTable stringTable, SharedTableData sharedTable)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);

            foreach (Text text in texts)
            {
                localizeStringEvent = LocalizeStringEventExtension.GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableDataExtension.SharedTableData_AddEntry(sharedTable, text.gameObject.name, text.transform.parent?.name, "TextLegacy");

                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                localizeStringEvent.Clear_OnUpdateString();
                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.Sign_OnUpdateString_TextLegacy(text);
            }
        }

        private static void AddLocalization_TextMeshProUI(List<TextMeshProUGUI> texts, StringTable stringTable, SharedTableData sharedTable)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            foreach (TextMeshProUGUI text in texts)
            {
                localizeStringEvent = LocalizeStringEventExtension.GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableDataExtension.SharedTableData_AddEntry(sharedTable, text.gameObject.name, text.transform.parent?.name, "TextMeshProUI");

                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                localizeStringEvent.Clear_OnUpdateString();
                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.Sign_OnUpdateString_TextMeshProUI(text);
            }
        }

        private static void AddLocalization_TextMeshPro(List<TextMeshPro> texts, StringTable stringTable, SharedTableData sharedTable)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            foreach (TextMeshPro text in texts)
            {
                localizeStringEvent = LocalizeStringEventExtension.GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableDataExtension.SharedTableData_AddEntry(sharedTable, text.gameObject.name, text.transform.parent?.name, "TextMeshPro");

                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                localizeStringEvent.Clear_OnUpdateString();
                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.Sign_OnUpdateString_TextMeshPro(text);
            }
        }

        private static void AddLocalization_TextMeshLegacy(List<TextMesh> texts, StringTable stringTable, SharedTableData sharedTable)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            foreach (TextMesh text in texts)
            {
                localizeStringEvent = LocalizeStringEventExtension.GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableDataExtension.SharedTableData_AddEntry(sharedTable, text.gameObject.name, text.transform.parent?.name, "TextMeshLegacy");

                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                localizeStringEvent.Clear_OnUpdateString();
                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.Sign_OnUpdateString_TextMeshLegacy(text);
            }
        }
    }
}