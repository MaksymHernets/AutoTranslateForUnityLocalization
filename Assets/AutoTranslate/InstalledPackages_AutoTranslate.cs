using System.Collections.Generic;
using System.Linq;
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
		List<NamedBuildTarget> namedBuildTargets = NamedBuildTargetExtension.GetNamedBuildTargets();
		string[] Names;
		List<string> lists;

		foreach (NamedBuildTarget namedBuildTarget in namedBuildTargets)
		{
			PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out Names);
			lists = Names.ToList();
			if (!lists.Contains(NameDefine_AutoTranslate))
				lists.Add(NameDefine_AutoTranslate);
			PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, lists.ToArray());
		}
	}
}

public static class NamedBuildTargetExtension
{
	public static List<NamedBuildTarget> GetNamedBuildTargets()
	{
		List<NamedBuildTarget> namedBuildTargets = new List<NamedBuildTarget>();
		namedBuildTargets.Add(NamedBuildTarget.Android); // +
		namedBuildTargets.Add(NamedBuildTarget.CloudRendering);
		namedBuildTargets.Add(NamedBuildTarget.EmbeddedLinux);
		namedBuildTargets.Add(NamedBuildTarget.iOS); // +
		namedBuildTargets.Add(NamedBuildTarget.NintendoSwitch);
		namedBuildTargets.Add(NamedBuildTarget.PS4);
		namedBuildTargets.Add(NamedBuildTarget.WindowsStoreApps); // + 
		namedBuildTargets.Add(NamedBuildTarget.WebGL); // +
		namedBuildTargets.Add(NamedBuildTarget.Standalone); // +
		
		return namedBuildTargets;
	}
}
