using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.ResourceLocations;

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
            IList<StringTable> stringTables = new List<StringTable>();

            List<Locale> locale = GetAvailableLocales();

            IList<string> labels = locale.Select(w => "Locale-" + w.Formatter).ToList();

            IList<IResourceLocation> locations = Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.Union, typeof(StringTable)).WaitForCompletion();

            stringTables = Addressables.LoadAssetsAsync<StringTable>(locations, null).WaitForCompletion();

            return stringTables.ToList();
        }
    }
}