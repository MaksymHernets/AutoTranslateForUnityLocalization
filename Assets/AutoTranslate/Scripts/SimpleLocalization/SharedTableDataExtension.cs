using GoodTime.HernetsMaksym.AutoTranslate;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace GoodTime.HernetsMaksym.AutoTranslate
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
            LocalizationEditorSettings.CreateStringTableCollection(name, "Assets/" + name);
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
    }
}
