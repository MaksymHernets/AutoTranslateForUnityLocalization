using EqualchanceGames.Tools.GUIPro;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace EqualchanceGames.Tools.AutoTranslate.Windows
{
    public class BaseLocalization_EditorWindow : BaseCustomWindow_EditorWindow
    {
        // Arguments for execute
        protected LocalizationSettings _localizationSettings;
        protected List<Locale> _locales;
        protected Locale _selectedLocale;

        // Data Localization
        protected IList<StringTable> _stringTables;
        protected IList<AssetTable> _assetTables;
        protected IList<SharedTableData> _sharedStringTables;
        protected IList<SharedTableData> _sharedAssetTables;

        protected DropdownGUI _dropdownLanguages;

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateLocalization();

            if (_locales != null)
                _dropdownLanguages = new DropdownGUI("Source language", _locales.Select(w => w.name).ToList());
            else
                _dropdownLanguages = new DropdownGUI("Source language", new List<string>());
            _dropdownLanguages.Width = k_SeparationWidth;
            InitDefaultDropdownLocalization();
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            _localizationSettings = SimpleInterfaceLocalization.GetLocalizationSettings();

            UpdateLocalization();

            if (_locales != null)
            {
                if ( _dropdownLanguages != null)
                {
                    _dropdownLanguages.ClearOptions();
                    _dropdownLanguages.AddOptions(_locales.Select(w => w.name).ToList());
                }
                else
                {
                    _dropdownLanguages = new DropdownGUI("Source language", _locales.Select(w => w.name).ToList());
                }
            }
        }

        protected void InitDefaultDropdownLocalization()
		{
            if (_selectedLocale != null) _dropdownLanguages.Selected = _selectedLocale.LocaleName;
            else _dropdownLanguages.Selected = string.Empty;
        }

        protected void ValidateLocalizationSettings()
        {
            if (_localizationSettings == null)
            {
                EditorGUILayout.HelpBox("Localization settings not found! Please create one via 'Edit/Project Settings/Localization'", MessageType.Error);
                GUI.enabled = false;
            }
        }

        protected void ValidateLocales()
        {
            if (_locales == null || _locales.Count == 0)
            {
                EditorGUILayout.HelpBox("Languages not found! Please add languages via 'Edit/Project Settings/Localization' => Locale Generator and reload project", MessageType.Error);
                GUI.enabled = false;
            }
        }

        protected void ValidateStringTables()
        {
            if (_stringTables == null || _stringTables.Count == 0)
            {
                EditorGUILayout.HelpBox("String Tables not found! Please add string table via 'Window/Asset Management/Localization Tables' => New Table Collection", MessageType.Error);
                GUI.enabled = false;
            }
        }

        protected bool UpdateLocalization()
        {
            _localizationSettings = SimpleInterfaceLocalization.GetLocalizationSettings();

            if (_localizationSettings == null)
            {
                return false;
            }

            _locales = _localizationSettings.GetAvailableLocales().Locales;

            if (_locales == null)
            {
                return false;
            }

            if (_selectedLocale == null ) _selectedLocale = SimpleInterfaceLocalization.GetSelectedLocale();

            _stringTables = SimpleInterfaceLocalization.GetAvailableStringTable();
            if (_stringTables != null)
            {
                _sharedStringTables = _stringTables.Select(w => w.SharedData).Distinct().ToList();
            }

            _assetTables = SimpleInterfaceLocalization.GetAvailableAssetTable();
            //if (_assetTables != null)
            //{
            //    _sharedAssetTables = _assetTables.Select(w => w.SharedData).Distinct().ToList();
            //}

            return true;
        }
    }
}