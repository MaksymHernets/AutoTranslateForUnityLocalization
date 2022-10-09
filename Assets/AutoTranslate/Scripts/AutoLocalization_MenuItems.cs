using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
	public class AutoLocalization_MenuItems : MonoBehaviour
    {
		private const string NameDefine_AutoTranslate = "AutoLocalization";
		private const string NamePackage_AutoTranslate = "AutoLocalization";

		[MenuItem("Window/Auto Localization/Add Define Auto Localization", false, 11)]
		public static void AddDefine_AutoLocalization()
		{
			List<BuildTargetGroup> namedBuildTargets = NamedBuildTargetExtension.GetNamedBuildTargets();
			string names;
			List<string> list;

			foreach (BuildTargetGroup namedBuildTarget in namedBuildTargets)
			{
				names = PlayerSettings.GetScriptingDefineSymbolsForGroup(namedBuildTarget);
				list = names.Split(',').ToList();
				if (!list.Contains(NameDefine_AutoTranslate))
					list.Add(NameDefine_AutoTranslate);
				names = string.Join(",", list.ToArray());
				PlayerSettings.SetScriptingDefineSymbolsForGroup(namedBuildTarget, names);
			}
		}
	}
}