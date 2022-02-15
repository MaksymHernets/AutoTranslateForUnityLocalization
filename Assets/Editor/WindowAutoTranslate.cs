using GoodTime.Tools.GoogleApiTranslate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEditor.Localization;
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

        // Temp
        private LocalizationSettings _localizationSettings;
        private List<Locale> _locales;
        private Locale _selectedLocale;
        private IList<StringTable> _tables;
        private IList<SharedTableData> _sharedtables;

        // Arguments for translate
        private string _selectedLanguage = string.Empty;
        private bool _canOverrideWords = true;
        private bool _canTranslateEmptyWords = true;
        private bool _canTranslateSmartWords = true;
        private List<bool> _canTranslateTableCollections;

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
            LoadSettings();
            LoadTables();
            InstTranslateTableCollections();
        }

        private void OnFocus()
        {
            LoadSettings();
            LoadTables();
            InstTranslateTableCollections();
        }

        void OnGUI()
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
            _canOverrideWords = EditorGUILayout.Toggle("Override words that have a translation", _canOverrideWords);
            _canTranslateEmptyWords = EditorGUILayout.Toggle("Translate words that don't have a translation", _canTranslateEmptyWords);
            _canTranslateSmartWords = EditorGUILayout.Toggle("Translate smart words", _canTranslateSmartWords);

            GUILayout.Space(10);
            EditorGUILayout.HelpBox("  Found " + _locales.Count + " languages" +
                "\n  Found " + _sharedtables.Count + " table collection" + 
                "\n  Found " + _tables.Count + " tables", MessageType.Info);

            if ( _sharedtables != null)
            {
                EditorGUILayout.LabelField("Selected collection tables for translation:", GUILayout.Width(300));
                EditorGUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(10,10,10,10) });
                int index = 0;
                foreach (var sharedtable in _sharedtables)
                {
                    _canTranslateTableCollections[index] = EditorGUILayout.ToggleLeft(sharedtable.TableCollectionName, _canTranslateTableCollections[index]);
                    ++index;
                }
                
                EditorGUILayout.EndVertical();
            }

            if (_isErrorTooManyRequests)
            {
                TimeSpan leftTime = _diedLineErrorTooManyRequests.Subtract(DateTime.Now);
                EditorGUILayout.HelpBox("The remote server returned an error: (429) Too Many Requests. Need to wait " + _timeNeedForWaitErrorMinute + " minutes. " + leftTime.Minutes + " minutes " + leftTime.Seconds + " left", MessageType.Error);
                if (_diedLineErrorTooManyRequests < DateTime.Now)
                {
                    _isErrorTooManyRequests = false;
                }
            }
            if( _localizationSettings == null )
            {
                EditorGUILayout.HelpBox("Localization settings not found! Please create one via 'Edit/Project Settings/Localization'", MessageType.Error);
                GUI.enabled = false;
            }
            if( _locales == null || _locales.Count == 0 )
            {
                EditorGUILayout.HelpBox("Languages not found! Please add languages via 'Edit/Project Settings/Localization' => Locale Generator and reload project", MessageType.Error);
                GUI.enabled = false;
            }
            if( _tables.Count == 0 )
            {
                EditorGUILayout.HelpBox("String Tables not found! Please add string table via 'Window/Asset Management/Localization Tables' => New Table Collection", MessageType.Error);
                GUI.enabled = false;
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Translate"))
            {
                ButtonTranslate_Click();
            }
        }

        private void LoadSettings()
        {
            string[] guids = AssetDatabase.FindAssets("Localization Settings t:LocalizationSettings", null);

            if (guids.Length != 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);

                _localizationSettings = AssetDatabase.LoadAssetAtPath<LocalizationSettings>(path);
            }

            _locales = new List<Locale>();

            if (_localizationSettings != null)
            {
                _locales = LocalizationSettings.Instance.GetAvailableLocales().Locales;

                Locale selectedLocale = LocalizationSettings.ProjectLocale;

                if ( string.IsNullOrEmpty(_selectedLanguage) )
                {
                    if (selectedLocale != null)
                    {
                        _selectedLanguage = selectedLocale.name;
                    }
                    else if (_locales.Count != 0)
                    {
                        _selectedLanguage = _locales[0].LocaleName;
                    }
                }
            }
        }

        private void ButtonTranslate_Click()
        {
            EditorUtility.DisplayCancelableProgressBar("Translating", "Load Tables", 0);

            LoadTables();

            if (_tables == null) return;

            PreparationTranslate();

            TranslateTables();

            SaveTables();

            EditorUtility.ClearProgressBar();
        }

        private void LoadTables()
        {
            try
            {
                _tables = new List<StringTable>();

                IList<string> labels = _locales.Select(w => "Locale-" + w.Formatter).ToList();

                var locations = Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.Union, typeof(StringTable)).WaitForCompletion();

                _tables = Addressables.LoadAssetsAsync<StringTable>(locations, null).WaitForCompletion();

                _sharedtables = _tables.Select(w => w.SharedData).Distinct().ToList();

                return;
            }
            catch (Exception exception)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void InstTranslateTableCollections()
        {
            _canTranslateTableCollections.Clear();
            foreach (var item in _sharedtables)
            {
                _canTranslateTableCollections.Add(true);
            }
        }

        private void PreparationTranslate()
        {
            EditorUtility.DisplayCancelableProgressBar("Translating", "Preparation translate", 0.1f);

            _selectedLocale = _locales.First(w => w.LocaleName == _selectedLanguage);

            _sharedtables = _tables.Select(w => w.SharedData).Distinct().ToList();
        }    

        private void TranslateTables()
        {
            GoogleApiTranslate translator = new GoogleApiTranslate();

            float progressRate = 0.9f / _tables.Count;
            int indexTable = 0;
            int indexTableCollection = -1;

            try
            {
                foreach (var sharedtable in _sharedtables)
                {
                    ++indexTableCollection;
                    if ( _canTranslateTableCollections[indexTableCollection] == false)
                    {
                        continue;
                    }
                    StringTable sourceLanguageTable = new StringTable();
                    List<StringTable> tablesForTranslate = new List<StringTable>();
                    foreach (var table in _tables)
                    {
                        if ( table.TableCollectionName == sharedtable.TableCollectionName)
                        {
                            if (table.LocaleIdentifier != _selectedLocale.Identifier)
                            {
                                tablesForTranslate.Add(table);
                            }
                            else
                            {
                                sourceLanguageTable = table;
                            }
                        }
                        
                    }

                    foreach (StringTable targetLanguageTable in tablesForTranslate)
                    {
                        ++indexTable;
                        float progress = 0.1f + indexTable * progressRate;
                        if( EditorUtility.DisplayCancelableProgressBar("Translating", "Translate Table" + sharedtable.TableCollectionName + " .Language " + targetLanguageTable.LocaleIdentifier.CultureInfo, progress) )
                        {
                            return;
                        }

                        foreach (var entry in sharedtable.Entries)
                        {
                            StringTableEntry sourceWord;
                            if (!sourceLanguageTable.TryGetValue(entry.Id, out sourceWord))
                            {
                                continue;
                            }
                            if (sourceWord == null)
                            {
                                continue;
                            }
                            if (sourceWord.IsSmart == true && _canTranslateSmartWords == false)
                            {
                                continue;
                            }

                            StringTableEntry targetWord;
                            if (targetLanguageTable.TryGetValue(entry.Id, out targetWord))
                            {
                                if (_canOverrideWords == false)
                                {
                                    continue;
                                }
                            }

                            string result = translator.Translate(sourceWord.Value, sourceLanguageTable.LocaleIdentifier.Code, targetLanguageTable.LocaleIdentifier.Code);
                            targetLanguageTable.AddEntry(entry.Key, result);
                        }
                    }
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

        private void SaveTables()
        {
            foreach (var table in _tables)
            {
                EditorUtility.SetDirty(table);
            }
        }
    }
}

