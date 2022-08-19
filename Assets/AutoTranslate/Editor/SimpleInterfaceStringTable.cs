using GoodTime.HernetsMaksym.AutoTranslate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public static class SimpleInterfaceStringTable
{
    public static bool CheckNameStringTable(string name)
	{
		List<SharedTableData> tables = SimpleInterfaceLocalization.GetAvailableSharedTableData();
		foreach (SharedTableData table in tables)
		{
			if (table.TableCollectionName == name)
				return true;
		}
		return false;
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

	public static StringTable GetStringTable(SharedTableData SharedData, Locale locale)
	{
		List<StringTable> stringTables = SimpleInterfaceLocalization.GetAvailableStringTable();
		foreach (StringTable stringTable in stringTables)
		{
			if (stringTable.SharedData == SharedData && stringTable.LocaleIdentifier == locale.Identifier) return stringTable;
		}
		new Exception("Table not found " + SharedData.TableCollectionName + " " + locale.Identifier);
		return null;
	}

	public static SharedTableData AddStringTable(string name)
	{
		//SharedTableData sharedTable = new SharedTableData();
		//sharedTable.name = name;
		//AssetDatabase.CreateAsset(sharedTable, "Assets/" + name + ".asset");

		LocalizationEditorSettings.CreateStringTableCollection(name, "Assets/" + name);

		return null;
	}

	public static void AddKey(string name)
	{

	}
}
