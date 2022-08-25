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

namespace GoodTime.HernetsMaksym.AutoTranslate
{
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

		public static StringTable GetStringTable(SharedTableData SharedData, Locale locale)
		{
			List<StringTable> stringTables = SimpleInterfaceLocalization.GetAvailableStringTable();
			foreach (StringTable stringTable in stringTables)
			{
				if (stringTable.SharedData == SharedData && stringTable.LocaleIdentifier == locale.Identifier) return stringTable;
			}
			//new Exception("Table not found " + SharedData.TableCollectionName + " " + locale.Identifier);
			return null;
		}
	}
}
