using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Localization;
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
        //public static partial void Execute(AddLocalizationParameters parameters, StatusLocalizationScene statusLocalizationScene);

        public static void Execute(AddLocalizationParameters parameters, StatusLocalizationScene statusLocalizationScene)
        {
            SharedTableData sharedTable = SharedTableDataExtension.GetOrAdd_SharedTableData(parameters.NameTable);
            StringTable stringTable = SimpleInterfaceStringTable.GetStringTable(sharedTable, parameters.SourceLocale);

            if (parameters.Lists.ContainsKey("Text Legacy") && parameters.Lists["Text Legacy"])
            {
                AddLocalization_TextLegacy(statusLocalizationScene.LegacyTexts, stringTable, sharedTable);
            }
		}

        public static void RemoveMiss_LocalizeStringEvent(List<LocalizeStringEvent> list)
		{
            var locales = LocalizationEditorSettings.GetLocales();
            var tableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (LocalizeStringEvent localizeStringEvent in list)
            {
                var m_SelectedTableCollection = tableCollections.FirstOrDefault(t => t.TableCollectionName == localizeStringEvent.StringReference.TableReference);
                if (localizeStringEvent.StringReference.TableEntryReference.ReferenceType == TableEntryReference.Type.Name)
                {
                    SharedTableData.SharedTableEntry m_SelectedEntry = m_SelectedTableCollection.SharedData.GetEntry(localizeStringEvent.StringReference.TableEntryReference.Key);
                    if (m_SelectedEntry == null)
                    {
                        UnityEngine.Object.DestroyImmediate(localizeStringEvent, true);
                    }
                }
                else
                {
                    SharedTableData.SharedTableEntry m_SelectedEntry = m_SelectedTableCollection.SharedData.GetEntry(localizeStringEvent.StringReference.TableEntryReference.KeyId);
                    if (m_SelectedEntry == null)
                    {
                        UnityEngine.Object.DestroyImmediate(localizeStringEvent, true);
                    }
                }
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
    }
}