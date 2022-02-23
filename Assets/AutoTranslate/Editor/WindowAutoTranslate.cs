using GoodTime.Tools.InterfaceTranslate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
    public class WindowAutoTranslate : EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Auto Translate for Localization";

        // Data Localization
        private LocalizationSettings _localizationSettings;
        private List<Locale> _locales;
        private Locale _selectedLocale;
        private IList<StringTable> _stringTables;
        private IList<SharedTableData> _sharedTables;

        // Arguments for translate
        private string _selectedLanguage = string.Empty;
        private TranslateParameters _translateParameters = new TranslateParameters();

        // Error
        private bool _isErrorTooManyRequests = false;
        private DateTime _diedLineErrorTooManyRequests;
        private double _timeNeedForWaitErrorMinute = 10;
        private GenericMenu genericMenu;

        [MenuItem("Window/Asset Management/Auto Translate for Tables")]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            WindowAutoTranslate window = GetWindow<WindowAutoTranslate>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle);
            window.Show();
        }

        public void OnEnable()
        {
            UpdateParameters();
        }

        private void OnFocus()
        {
            UpdateParameters();
        }

        private void UpdateParameters()
        {
            LoadSettings();
            if ( _sharedTables != null)
            {
                InitializationTranslateTableCollections();
            }
            if (_selectedLocale != null)
            {
                _selectedLanguage = _selectedLocale.LocaleName;
            }
            else
            {
                _selectedLanguage = string.Empty;
            }
        }

        void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Source language", GUILayout.Width(300));

            var posit = new Rect(new Vector2(310, 0), new Vector2(200, 20));
            if (EditorGUILayout.DropdownButton(new GUIContent(_selectedLanguage), FocusType.Passive))
            {
                genericMenu = new GenericMenu();

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
            _translateParameters.canOverrideWords = EditorGUILayout.Toggle("Override words that have a translation", _translateParameters.canOverrideWords);
            _translateParameters.canTranslateEmptyWords = EditorGUILayout.Toggle("Translate words that don't have a translation", _translateParameters.canTranslateEmptyWords);
            _translateParameters.canTranslateSmartWords = EditorGUILayout.Toggle("Translate smart words", _translateParameters.canTranslateSmartWords);

            GUILayout.Space(10);
            EditorGUILayout.HelpBox("  Found " + _locales?.Count + " languages" +
                "\n  Found " + _sharedTables?.Count + " table collection" + 
                "\n  Found " + _stringTables?.Count + " tables", MessageType.Info);

            if ( _sharedTables != null)
            {
                EditorGUILayout.LabelField("Selected collection tables for translation:", GUILayout.Width(300));
                EditorGUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(10,10,10,10) });
                int index = 0;
                foreach (var sharedtable in _sharedTables)
                {
                    _translateParameters.canTranslateTableCollections[index] = EditorGUILayout.ToggleLeft(sharedtable.TableCollectionName, _translateParameters.canTranslateTableCollections[index]);
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
            if( _stringTables.Count == 0 )
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

        private bool LoadSettings()
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

            _sharedTables = SimpleInterfaceLocalization.GetAvailableSharedTableData();

            if (_sharedTables == null)
            {
                return false;
            }

            _stringTables = SimpleInterfaceLocalization.GetAvailableStringTable();

            if (_stringTables == null)
            {
                return false;
            }
            return true;
        }

        private void ButtonTranslate_Click()
        {
            EditorUtility.DisplayCancelableProgressBar("Translating", "Load Tables", 0);

            LoadSettings();

            EditorUtility.DisplayCancelableProgressBar("Translating", "Preparation translate", 0.1f);

            _selectedLocale = _locales.First(w => w.LocaleName == _selectedLanguage);

            TranslateData translateData = new TranslateData();
            translateData.selectedLocale = _selectedLocale;
            translateData.sharedtables = _sharedTables.ToList();
            translateData.stringTables = _stringTables.ToList();

            TranslateLocalization translateLocalization = new TranslateLocalization();

            try
            {
                foreach (var translateStatus in translateLocalization.Make(_translateParameters, translateData))
                {
                    if (EditorUtility.DisplayCancelableProgressBar("Translating", "Translate... Table " + translateStatus.sharedTable + " | Language -" + translateStatus.targetLanguageTable, translateStatus.progress))
                    {
                        return;
                    }
                }
            }
            catch (WebException webException)
            {
                EditorUtility.ClearProgressBar();
                _diedLineErrorTooManyRequests = DateTime.Now;
                _diedLineErrorTooManyRequests = _diedLineErrorTooManyRequests.AddMinutes(_timeNeedForWaitErrorMinute);
                _isErrorTooManyRequests = true;
                Debug.Log(webException.Message);
                return;
            }
            catch (Exception exception)
            {
                EditorUtility.ClearProgressBar();
                Debug.Log(exception.Message);
                return;
            }

            SaveStringTables();

            EditorUtility.DisplayProgressBar("Translating", "Completed", 1f);

            EditorUtility.ClearProgressBar();
        }

        private void InitializationTranslateTableCollections()
        {
            _translateParameters.canTranslateTableCollections.Clear();
            foreach (var item in _sharedTables)
            {
                _translateParameters.canTranslateTableCollections.Add(true);
            }
        }  

        private void SaveStringTables()
        {
            foreach (var stringTable in _stringTables)
            {
                EditorUtility.SetDirty(stringTable);
            }
        }
    }
}

