using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
			AutoLocalization_MenuItems.AddDefine_AutoLocalization();
			Debug.Log("Found Auto Localization");
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
