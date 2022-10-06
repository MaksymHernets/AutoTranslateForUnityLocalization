using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class InstalledPackages_AutoTranslate : AssetPostprocessor
{
	private const string NameDefine_AutoTranslate = "AutoLocalization";
	private const string NamePackage_AutoTranslate = "AutoLocalization";

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, 
		string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
	{
		if (importedAssets.Any(w => w == NamePackage_AutoTranslate))
		{
			AddDefine_AutoTranslate();
			Debug.Log("Found Auto Localization");
		}
	}
	
	[MenuItem("Window/Auto Localization/Add Define Auto Localization", false, 11)]
	public static void AddDefine_AutoTranslate()
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
			names = string.Join("," , list.ToArray());
			PlayerSettings.SetScriptingDefineSymbolsForGroup(namedBuildTarget, names);
		}
	}
}

public static class NamedBuildTargetExtension
{
	public static List<BuildTargetGroup> GetNamedBuildTargets()
	{
		List<BuildTargetGroup> namedBuildTargets = new List<BuildTargetGroup>();
		namedBuildTargets.Add(BuildTargetGroup.Android); // +
		namedBuildTargets.Add(BuildTargetGroup.iOS); // +
		namedBuildTargets.Add(BuildTargetGroup.Lumin);
		namedBuildTargets.Add(BuildTargetGroup.PS4);
		namedBuildTargets.Add(BuildTargetGroup.WebGL); // +
		namedBuildTargets.Add(BuildTargetGroup.Standalone); // +
		
		return namedBuildTargets;
	}
}
