using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using static UnityEngine.Localization.Tables.SharedTableData;

namespace GoodTime.HernetsMaksym.AutoTranslate
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
            if (parameters.Lists.ContainsKey("Text Mesh Pro") && parameters.Lists["Text Mesh Pro"])
            {
                AddLocalization_TextMeshPro(statusLocalizationScene.TextMeshs, stringTable, sharedTable);
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

        private static void AddLocalization_TextMeshPro(List<TextMeshProUGUI> texts, StringTable stringTable, SharedTableData sharedTable)
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
                localizeStringEvent.Sign_OnUpdateString_TextMeshPro(text);
            }
        }
    }
}