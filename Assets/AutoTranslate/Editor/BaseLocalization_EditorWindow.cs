using GoodTime.HernetsMaksym.AutoTranslate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class BaseLocalization_EditorWindow : EditorWindow
{
    // Window parameters
    protected int k_SeparationWidth = 400;

    // Arguments for execute
    protected LocalizationSettings _localizationSettings;
    protected List<Locale> _locales;
    protected Locale _selectedLocale;

    // Data Localization
    protected IList<StringTable> _stringTables;
    protected IList<AssetTable> _assetTables;
    protected IList<SharedTableData> _sharedStringTables;
    protected IList<SharedTableData> _sharedAssetTables;

    protected GenericMenu genericMenu;

    protected string _selectedLanguage = string.Empty;

    protected virtual void OnEnable()
    {
        UpdateLocalization();
    }

    protected virtual void OnFocus()
    {
        UpdateLocalization();
    }

    protected void UpdateLocalization()
    {
        LoadSettings();
        if (_selectedLocale != null)
        {
            _selectedLanguage = _selectedLocale.LocaleName;
        }
        else
        {
            _selectedLanguage = string.Empty;
        }
    }

    protected void ShowNameWindow(string name)
	{
        GUI.enabled = false;
        GUILayout.Button(name, GUILayout.Height(60));
        GUI.enabled = true;
        GUILayout.Space(10);
    }

    protected void SourceLanguage(int width = 400)
	{
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Source language", GUILayout.Width(width));
        if (EditorGUILayout.DropdownButton(new GUIContent(_selectedLanguage), FocusType.Passive))
        {
            Rect posit = new Rect(new Vector2(310, 0), new Vector2(200, 20));
            genericMenu = new GenericMenu();
            foreach (string option in _locales.Select(w => w.name))
            {
                genericMenu.AddItem(new GUIContent(option), option == _selectedLanguage, () =>
                {
                    _selectedLanguage = option;
                });
            }
            genericMenu.DropDown(posit);
        }
        EditorGUILayout.EndHorizontal();
    }

    protected string SmartDropdown(string label, List<string> options, string select, Action<string> action, int width = 300)
	{
        string selected = string.Empty;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(width));
        if ( EditorGUILayout.DropdownButton(new GUIContent(select), FocusType.Passive) )
        {
            Rect posit = new Rect(new Vector2(310, 0), new Vector2(200, 20));
            genericMenu = new GenericMenu();
            foreach (string option in options)
            {
                genericMenu.AddItem(new GUIContent(option), option == select, () =>
                {
                    selected = option;
                });
            }
            genericMenu.DropDown(posit);
        }
        EditorGUILayout.EndHorizontal();
        return selected;
    }

    protected void ValidLocalization()
	{
        if (_localizationSettings == null)
        {
            EditorGUILayout.HelpBox("Localization settings not found! Please create one via 'Edit/Project Settings/Localization'", MessageType.Error);
            GUI.enabled = false;
        }
        if (_locales == null || _locales.Count == 0)
        {
            EditorGUILayout.HelpBox("Languages not found! Please add languages via 'Edit/Project Settings/Localization' => Locale Generator and reload project", MessageType.Error);
            GUI.enabled = false;
        }
        if (_stringTables == null || _stringTables.Count == 0)
        {
            EditorGUILayout.HelpBox("String Tables not found! Please add string table via 'Window/Asset Management/Localization Tables' => New Table Collection", MessageType.Error);
            GUI.enabled = false;
        }
    }

    protected bool LoadSettings()
    {
        _localizationSettings = SimpleInterfaceLocalization.GetLocalizationSettings();

        if (_localizationSettings == null)
        {
            return false;
        }

        _locales = SimpleInterfaceLocalization.GetAvailableLocales();

        if (_locales == null)
        {
            return false;
        }

        _selectedLocale = SimpleInterfaceLocalization.GetSelectedLocale();

        _stringTables = SimpleInterfaceLocalization.GetAvailableStringTable();
        if (_stringTables != null)
        {
            _sharedStringTables = _stringTables.Select(w => w.SharedData).Distinct().ToList();
        }

        _assetTables = SimpleInterfaceLocalization.GetAvailableAssetTable();
        if (_assetTables != null)
        {
            _sharedAssetTables = _assetTables.Select(w => w.SharedData).Distinct().ToList();
        }

        return true;
    }
}
