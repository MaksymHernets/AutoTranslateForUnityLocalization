using EqualchanceGames.Tools.AutoTranslate;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;
using static UnityEngine.Localization.Tables.SharedTableData;

namespace EqualchanceGames.Tools.AutoTranslate
{
    public static class SharedTableDataExtension
    {
        public static void Clear(this SharedTableData sharedTable)
        {
            while (sharedTable.Entries.Count != 0)
                sharedTable.RemoveKey(sharedTable.Entries[0].Id);
        }

        public static SharedTableData GetSharedTable(string name)
        {
            List<SharedTableData> tables = SimpleInterfaceLocalization.GetAvailableSharedTableData();
            foreach (SharedTableData table in tables)
            {
                if (table.TableCollectionName == name)
                    return table;
            }
            return null;
        }

        public static SharedTableData AddSharedTableData(string name)
        {
            StringTableCollection stringTableCollection = LocalizationEditorSettings.CreateStringTableCollection(name, "Assets/" + name);
            Undo.RegisterCreatedObjectUndo(stringTableCollection, "Created StringTableCollection (Search Localization)");
            SharedTableData sharedTable = GetSharedTable(name);
            return sharedTable;
        }

        public static SharedTableData GetOrAdd_SharedTableData(string nameTable)
        {
            SharedTableData sharedTable = GetSharedTable(nameTable);

            if (sharedTable != null) sharedTable.Clear();
            else sharedTable = AddSharedTableData(nameTable);

            return sharedTable;
        }

        public static SharedTableEntry SharedTableData_AddEntry(SharedTableData sharedTable, string nameGameObjet, string parentName, string typeText)
        {
            string name = String.Format("[{0}][{1}][{2}]", nameGameObjet, parentName, typeText);
            int variants = 1;
            while (sharedTable.Contains(name))
            {
                name = String.Format("[{0}][{1}][{2}][{3}]", nameGameObjet, parentName, typeText, variants);
                ++variants;
            }
            Undo.RecordObject(sharedTable, "AddKey for SharedTable (Search Localization)");
            SharedTableEntry sharedTableEntry = sharedTable.AddKey(name);
            return sharedTableEntry;
        }
    }
}
