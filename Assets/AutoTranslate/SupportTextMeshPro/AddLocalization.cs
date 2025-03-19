using EqualchanceGames.Tools.AutoTranslate.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using static UnityEngine.Localization.Tables.SharedTableData;

namespace EqualchanceGames.Tools.AutoTranslate.SupportTextMeshPro
{
    public static class AddLocalization
    {
        //public static void Execute(AddLocalizationParameters parameters, StatusLocalizationScene statusLocalizationScene);

        public static void Execute(AddLocalizationParameters parameters, StatusLocalizationScene statusLocalizationScene)
		{
            SharedTableData sharedTable = SharedTableDataExtension.GetOrAdd_SharedTableData(parameters.NameTable);
            StringTable stringTable = SimpleInterfaceStringTable.GetStringTable(sharedTable, parameters.SourceLocale);

            if (parameters.Lists.ContainsKey("Text Mesh Pro") && parameters.Lists["Text Mesh Pro"])
            {
                AddLocalization_TextMeshProUI(statusLocalizationScene.TextMeshProUIs, stringTable, sharedTable);
            }
        }

        private static void AddLocalization_TextMeshProUI(List<TextMeshProUGUI> texts, StringTable stringTable, SharedTableData sharedTable)
        {
            LocalizeStringEvent localizeStringEvent = default(LocalizeStringEvent);
            SharedTableEntry sharedTableEntry = default(SharedTableEntry);
            foreach (TextMeshProUGUI text in texts)
            {
                localizeStringEvent = LocalizeStringEventExtension.GetOrAdd_LocalizeStringEventComponent(text.gameObject);

                sharedTableEntry = SharedTableDataExtension.SharedTableData_AddEntry(sharedTable, text.gameObject.name, text.transform.parent?.name, "TextMeshPro");

                stringTable.AddEntry(sharedTableEntry.Key, text.text);

                localizeStringEvent.Clear_OnUpdateString();
                localizeStringEvent.Sign_ReferenceTable(sharedTable.TableCollectionName, sharedTableEntry.Key);
                localizeStringEvent.Sign_OnUpdateString_TextMeshProUI(text);
            }
        }
    }
}
