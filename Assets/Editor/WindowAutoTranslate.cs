using GoodTime.Tools.GoogleApiTranslate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.ResourceLocations;

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

                GUILayout.Space(10);
                EditorGUIUtility.labelWidth = 300;
                _isOverrideWords = EditorGUILayout.Toggle("Override words that have a translation", _isOverrideWords);
                GUILayout.Space(10);
                EditorGUIUtility.labelWidth = 300;
                _isTranslateEmptyWords = EditorGUILayout.Toggle("Translate words that don't have a translation", _isTranslateEmptyWords);
                GUILayout.Space(10);
                EditorGUIUtility.labelWidth = 300;
                _isTranslateSmartWords = EditorGUILayout.Toggle("Translate smart words", _isTranslateSmartWords);

                GUILayout.Space(20);
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
            string[] guids = AssetDatabase.FindAssets("Localization Settings t:LocalizationSettings", null);

            if (guids.Length != 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);

                LocalizationSettings localizationSettings = AssetDatabase.LoadAssetAtPath<LocalizationSettings>(path);

                _locales = localizationSettings.GetAvailableLocales().Locales;

                if ( _locales == null || _locales.Count == 0)
                {
                    _typeStage = TypeStage.ErrorNoFoundLocales;
                    return;
                }

                Locale selectedLocale = localizationSettings.GetSelectedLocale();

                if ( string.IsNullOrEmpty(_selectedLanguage) )
                {
                    if (selectedLocale != null)
                    {
                        _selectedLanguage = selectedLocale.LocaleName;
                    }

                    _selectedLanguage = _locales[0].LocaleName;
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

            LoadTables().ContinueWith( _ =>
            {
                TranslateTables();
                
                _typeStage = TypeStage.Ready;
            }
            , System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously);
        }

        private async Task LoadTables()
        {
            List<string> labels = _locales.Select(w=> "Locale-" + w.Formatter).ToList();

            IList<IResourceLocation> locations = await Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.Union, typeof(StringTable)).Task;

            _tables = await Addressables.LoadAssetsAsync<StringTable>(locations, null).Task;

            return;
        }

        private void TranslateTables()
        {
            if ( _tables == null ) return;

            _selectedLocale = _locales.First(w => w.LocaleName == _selectedLanguage);

            StringTable sourceLanguageTable = _tables.First(w => w.LocaleIdentifier == _selectedLocale.Identifier);

            IList<StringTable> tablesForTranslate = _tables;
            tablesForTranslate.Remove(sourceLanguageTable);

            GoogleApiTranslate translator = new GoogleApiTranslate();

            foreach (StringTable targetLanguageTable in tablesForTranslate)
            {
                foreach (var entry in sourceLanguageTable.SharedData.Entries)
                {
                    StringTableEntry sourceWord = sourceLanguageTable.GetEntry(entry.Key);
                    if ( sourceWord == null && string.IsNullOrEmpty(sourceWord.Value) )
                    {
                        continue;
                    }
                    if ( sourceWord.IsSmart == true && _isTranslateSmartWords == false)
                    {
                        continue;
                    }
                    StringTableEntry targetWord = targetLanguageTable.GetEntry(entry.Key);
                    if ( targetWord == null || string.IsNullOrEmpty(targetWord.Value) )
                    {
                        if ( _isTranslateEmptyWords == false)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if ( _isOverrideWords == false )
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

