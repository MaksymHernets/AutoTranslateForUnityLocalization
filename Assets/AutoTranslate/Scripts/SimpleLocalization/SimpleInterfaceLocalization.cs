using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
	public class SimpleInterfaceLocalization
    {
        public static bool HasLocalizationSettings()
        {
            string[] guids = AssetDatabase.FindAssets("Localization Settings t:LocalizationSettings", null);

            return guids.Length != 0;
        }

        public static LocalizationSettings GetLocalizationSettings()
        {
            string[] guids = AssetDatabase.FindAssets("Localization Settings t:LocalizationSettings", null);

            if (guids.Length != 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);

                return AssetDatabase.LoadAssetAtPath<LocalizationSettings>(path);
            }
            return default(LocalizationSettings);
        }

        public static List<Locale> GetAvailableLocales()
        {
            return LocalizationSettings.Instance.GetAvailableLocales().Locales;
            //return LocalizationEditorSettings.GetLocales();
        }

        public static Locale GetSelectedLocale()
        {
            
            Locale selectedLocale = LocalizationSettings.SelectedLocale;
            if (selectedLocale != null)
            {
                return selectedLocale;
            }
            selectedLocale = LocalizationSettings.ProjectLocale;
            if (selectedLocale != null)
            {
                return selectedLocale;
            }
            var locales = GetAvailableLocales();
            if (locales.Count != 0)
            {
                return locales[0];
            }
            return null;
        }

        public static List<SharedTableData> GetAvailableSharedTableData()
        {
            string[] guids = AssetDatabase.FindAssets("t:SharedTableData", null);

            List<SharedTableData> sharedTableDatas = new List<SharedTableData>();

            string path;
            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                sharedTableDatas.Add(AssetDatabase.LoadAssetAtPath<SharedTableData>(path));
            }

            return sharedTableDatas;
        }

        public static List<StringTable> GetAvailableStringTable()
        {
            string[] guids = AssetDatabase.FindAssets("t:StringTable", null);

            List<StringTable> stringTable = new List<StringTable>();

            string path;
            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                stringTable.Add(AssetDatabase.LoadAssetAtPath<StringTable>(path));
            }

            return stringTable;
        }

        public static List<AssetTable> GetAvailableAssetTable()
        {
            string[] guids = AssetDatabase.FindAssets("t:AssetTable", null);

            List<AssetTable> assetTables = new List<AssetTable>();

            string path;
            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);

                assetTables.Add(AssetDatabase.LoadAssetAtPath<AssetTable>(path));
            }

            return assetTables;
        }
    }
}