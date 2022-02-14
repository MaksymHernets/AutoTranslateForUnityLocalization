using GoodTime.Tools.GoogleApiTranslate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class WindowAutoTranslate : EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Auto Translate for Localization";
        private TypeStage _typeStage;

        // Temp
        private List<Locale> _locales;
        private Locale _selectedLocale;
        private IList<StringTable> _tables;

        // Arguments for translate
        private string _selectedLanguage = string.Empty;
        private bool _isOverrideWords = true;
        private bool _isTranslateEmptyWords = true;
        private bool _isTranslateSmartWords = true;

        private bool _isErrorTooManyRequests = false;
        private DateTime _diedLineErrorTooManyRequests;
        private double _timeNeedForWaitErrorMinute = 10;

        [MenuItem("Window/Asset Management/Auto Translate for Tables")]
        public static void ShowWindow()
        {
            var window = GetWindow<WindowAutoTranslate>(false, k_WindowTitle, true);
            window.titleContent = new GUIContent(k_WindowTitle);
            window.Show();
        }

        public void OnEnable()
        {
            _typeStage = TypeStage.Loading;

            LoadSettings();
        }

        private void OnFocus()
        {
            LoadSettings();
        }

        void OnGUI()
        {
            if (_typeStage == TypeStage.Loading)
            {
                EditorGUILayout.LabelField("Please wait. Search Localization Settings", EditorStyles.boldLabel);
            }
            else if (_typeStage == TypeStage.Ready)
            {
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Source language", GUILayout.Width(300));

                var posit = new Rect(new Vector2(310, 10), new Vector2(200, 20));
                if (EditorGUILayout.DropdownButton(new GUIContent(_selectedLanguage), FocusType.Passive))
                {
                    var genericMenu = new GenericMenu();

                    foreach (var option in _locales.Select(w => w.name))
                    {
                        bool selected = option == _selectedLanguage;
                        genericMenu.AddItem(new GUIContent(option), selected, () =>
                        {
                            _selectedLanguage = option;
                        });
                    }
                    genericMenu.DropDown(posit);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUIUtility.labelWidth = 300;
                _isOverrideWords = EditorGUILayout.Toggle("Override words that have a translation", _isOverrideWords);
                _isTranslateEmptyWords = EditorGUILayout.Toggle("Translate words that don't have a translation", _isTranslateEmptyWords);
                _isTranslateSmartWords = EditorGUILayout.Toggle("Translate smart words", _isTranslateSmartWords);

                GUILayout.Space(10);
                EditorGUILayout.HelpBox("\n  Found " + _locales.Count + " languages\n", MessageType.Info);

                if ( _isErrorTooManyRequests )
                {
                    TimeSpan leftTime = _diedLineErrorTooManyRequests.Subtract(DateTime.Now);
                    EditorGUILayout.HelpBox("The remote server returned an error: (429) Too Many Requests. Need to wait " + _timeNeedForWaitErrorMinute + " minutes. " + leftTime.Minutes + " minutes " + leftTime.Seconds + " left", MessageType.Error);
                    if ( _diedLineErrorTooManyRequests < DateTime.Now )
                    {
                        _isErrorTooManyRequests = false;
                    }
                }
                GUILayout.Space(10);
                if (GUILayout.Button("Translate"))
                {
                    ButtonTranslate_Click();
                }
            }
            else if (_typeStage == TypeStage.Translating)
            {
                EditorGUILayout.LabelField("Translating", EditorStyles.boldLabel);
            }
            else if (_typeStage == TypeStage.ErrorNoFoundSettings)
            {
                EditorGUILayout.LabelField("Error. Localization settings not found!", EditorStyles.boldLabel);
            }
            else if (_typeStage == TypeStage.ErrorNoFoundLocales)
            {
                EditorGUILayout.LabelField("Error. No languages found!", EditorStyles.boldLabel);
            }
        }

        private void LoadSettings()
        {
            LocalizationSettings localizationSettings = LocalizationSettings.InitializationOperation.WaitForCompletion();

            if ( localizationSettings != null)
            {
                _locales = localizationSettings.GetAvailableLocales().Locales;

                if ( _locales == null || _locales.Count == 0)
                {
                    _typeStage = TypeStage.ErrorNoFoundLocales;
                    return;
                }

                Locale selectedLocale = LocalizationSettings.ProjectLocale;

                if ( string.IsNullOrEmpty(_selectedLanguage) )
                {
                    if (selectedLocale != null)
                    {
                        _selectedLanguage = selectedLocale.name;
                    }
                    else
                    {
                        _selectedLanguage = _locales[0].LocaleName;
                    }
                }
                
                _typeStage = TypeStage.Ready;
            }
            else
            {
                _typeStage = TypeStage.ErrorNoFoundSettings;
            }
        }

        private void ButtonTranslate_Click()
        {
            _typeStage = TypeStage.Translating;

            LoadTables();

            TranslateTables();

            //Addressables.CheckForCatalogUpdates();
            //AssetBundle.UnloadAllAssetBundles(true);

            EditorUtility.ClearProgressBar();

            _typeStage = TypeStage.Ready;
        }

        private void LoadTables()
        {
            try
            {
                EditorUtility.DisplayCancelableProgressBar("Translating", "Load Tables", 0);

                IList<string> labels = _locales.Select(w => "Locale-" + w.Formatter).ToList();

                var locations = Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.Union, typeof(StringTable)).WaitForCompletion();

                _tables = Addressables.LoadAssetsAsync<StringTable>(locations, null).WaitForCompletion();

                return;
            }
            catch (Exception exception)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void TranslateTables()
        {
            if ( _tables == null ) return;

            EditorUtility.DisplayCancelableProgressBar("Translating", "Preparation translate", 0.1f);

            _selectedLocale = _locales.First(w => w.LocaleName == _selectedLanguage);

            StringTable sourceLanguageTable = _tables.FirstOrDefault(w => w.LocaleIdentifier == _selectedLocale.Identifier);

            List<StringTable> tablesForTranslate = new List<StringTable>();
            foreach (var item in _tables)
            {
                if ( item.LocaleIdentifier != _selectedLocale.Identifier)
                {
                    tablesForTranslate.Add(item);
                } 
            }

            GoogleApiTranslate translator = new GoogleApiTranslate();

            float progressRate = 0.9f / tablesForTranslate.Count;
            int indexTable = 0;

            try
            {
                foreach (StringTable targetLanguageTable in tablesForTranslate)
                {
                    float progress = 0.1f + indexTable * progressRate;
                    EditorUtility.DisplayCancelableProgressBar("Translating", "Translate " + targetLanguageTable.LocaleIdentifier.CultureInfo, progress);
                    foreach (var entry in sourceLanguageTable.SharedData.Entries)
                    {
                        StringTableEntry sourceWord;
                        if (!sourceLanguageTable.TryGetValue(entry.Id, out sourceWord))
                        {
                            continue;
                        }
                        //sourceWord = sourceLanguageTable.GetEntry(entry.Key);
                        if (sourceWord == null)
                        {
                            continue;
                        }
                        if (sourceWord.IsSmart == true && _isTranslateSmartWords == false)
                        {
                            continue;
                        }
                        StringTableEntry targetWord;

                        if ( targetLanguageTable.TryGetValue(entry.Id, out targetWord) )
                        {
                            if (_isOverrideWords == false)
                            {
                                continue;
                            }
                        }
                        string result = translator.Translate(sourceWord.Value, sourceLanguageTable.LocaleIdentifier.Code, targetLanguageTable.LocaleIdentifier.Code);
                        targetLanguageTable.AddEntry(entry.Key, result);
                    }
                    ++indexTable;
                }
            }
            catch (WebException webException)
            {
                EditorUtility.ClearProgressBar();
                _diedLineErrorTooManyRequests = DateTime.Now;
                _diedLineErrorTooManyRequests = _diedLineErrorTooManyRequests.AddMinutes(_timeNeedForWaitErrorMinute);
                _isErrorTooManyRequests = true;
            }
            catch (Exception exception)
            {
                EditorUtility.ClearProgressBar();
                Debug.Log(exception.Message);
            }

            EditorUtility.DisplayProgressBar("Translating", "Completed", 1f);
        }
    }

    public enum TypeStage
    {
        Loading,
        Ready,
        Translating,
        Done,
        ErrorNoFoundSettings,
        ErrorNoFoundLocales
    }
}

