using EqualchanceGames.Tools.Helpers;
using EqualchanceGames.Tools.GUIPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;
using EqualchanceGames.Tools.AutoTranslate.Editor;
using UnityEngine.Localization;
using EqualchanceGames.Tools.FactoryTranslate;
using EqualchanceGames.Tools.InterfaceTranslate;

namespace EqualchanceGames.Tools.AutoTranslate.Windows
{
	public class AutoTranslate_EditorWindow : BaseLocalization_EditorWindow
    {
        // Window parameters
        protected const string k_WindowTitle = "Auto Translate for Unity Localization";

        // Arguments for execute
        private TranslateParameters _translateParameters = new TranslateParameters();

        // Error
        private bool _isErrorTooManyRequests = false;
        private DateTime _diedLineErrorTooManyRequests;
        private double _timeNeedForWaitErrorMinute = 10;
        private bool _isErrorConnection = false;
        private CheckListGUI _checkListStringTable;
        private CheckListGUI _checkListLanguages;
        private bool LS = false;
        private bool WLS = false;
        private ITranslateApi translator;
        private Vector2 _position = Vector2.zero;
        private float MinChar = 0;
        private float MaxChar = 1000;

        [MenuItem("Auto Localization/Auto Translate for String Tables", false, MyProjectSettings_AutoTranslate.BaseIndex + 1)]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            AutoTranslate_EditorWindow window = GetWindow<AutoTranslate_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.AutoTranslate);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _isErrorConnection = WebInformation.IsConnectedToInternet();

            if (_sharedStringTables != null)
                _checkListStringTable = new CheckListGUI(_sharedStringTables.Select(w => w.TableCollectionName).ToList());
            else
                _checkListStringTable = new CheckListGUI(new List<string>());

            _checkListStringTable.Width = 100;
            _checkListStringTable.Height = 1000;

            if (_locales != null)
            {
                _checkListLanguages = new CheckListGUI(_locales.Select(w => w.name).ToList());
                _checkListLanguages.UpdateCheck(new List<string>() { _dropdownLanguages.Selected }, false, false);
            }
            else
                _checkListLanguages = new CheckListGUI(new List<string>());
            _checkListLanguages.Height = 600;
            _checkListLanguages.MinHeight = 500;

            _dropdownLanguages.UpdateSelected += DropdownLanguages_UpdateSelected;

            AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();
            translator = FactoryTranslateApi.GetTranslateApi(setting.CurrentServiceTranslate);
        }

        private void DropdownLanguages_UpdateSelected(string name)
        {
            _selectedLocale = _locales.First(w => w.LocaleName == _dropdownLanguages.Selected);
            _checkListLanguages.FillElements(_locales.Select(w => w.name).ToList());
            _checkListLanguages.UpdateCheck(new List<string>() { name }, false, false);
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            _isErrorConnection = WebInformation.IsConnectedToInternet();

            if (_sharedStringTables != null)
            {
                if (_checkListStringTable != null) 
                {
                    _checkListStringTable.Update(_sharedStringTables.Select(w => w.TableCollectionName).ToList());
                }
                else
                {
                    _checkListStringTable = new CheckListGUI(_sharedStringTables.Select(w => w.TableCollectionName).ToList());
                }
            }
            if (_locales != null)
            {
                _checkListLanguages = new CheckListGUI(_locales.Select(w => w.name).ToList());
                if (_dropdownLanguages != null) _checkListLanguages.UpdateCheck(new List<string>() { _dropdownLanguages.Selected }, false, false);
            }

            AutoTranslateSetting setting = AutoTranslateSetting.GetOrCreateSettings();
            translator = FactoryTranslateApi.GetTranslateApi(setting.CurrentServiceTranslate);
        }

        void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);
            //EditorGUIUtility.labelWidth = k_SeparationWidth;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(350), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField("Selected collection tables for translation:");
            _checkListStringTable.DrawButtons();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            _dropdownLanguages.Draw();
            if ( _selectedLocale != null && translator.ValidateLocale(_selectedLocale.Identifier.Code) == false)
            {
                EditorGUILayout.HelpBox(translator.GetNameService() + " service does not support some dialects of languages, the choice of language will be changed to the generally accepted.", MessageType.Warning);
            }

            _translateParameters.canOverrideWords = LinesGUI.DrawLineToggle("Override words that have a translation", _translateParameters.canOverrideWords, k_SeparationWidth);
            _translateParameters.canTranslateEmptyWords = LinesGUI.DrawLineToggle("Single word translation. No translation errors, but attempts are exhausted faster.", _translateParameters.canTranslateEmptyWords, k_SeparationWidth);
            _translateParameters.canTranslateSmartWords = LinesGUI.DrawLineToggle("Translate smart words", _translateParameters.canTranslateSmartWords, k_SeparationWidth);
            //EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            //EditorGUILayout.LabelField("Translate words that contain chars", GUILayout.Width(k_SeparationWidth));
            //int minchar = (int)MinChar;
            //int maxchar = (int)MaxChar;
            //EditorGUILayout.LabelField(minchar.ToString("G"), GUILayout.Width(30));
            //EditorGUILayout.MinMaxSlider(ref MinChar, ref MaxChar, 0, 1000);
            //EditorGUILayout.LabelField(maxchar.ToString("G"), GUILayout.Width(30));
            //EditorGUILayout.EndHorizontal();
            

            GUILayout.Space(10);

            EditorGUILayout.HelpBox("  Found " + _locales?.Count + " languages" +
                "\n  Found " + _sharedStringTables?.Count + " table collection" + 
                "\n  Found " + _stringTables?.Count + " string tables" + 
                "\n  Found " + _assetTables?.Count + " asset tables", MessageType.Info);

            if ( _isErrorConnection ) EditorGUILayout.HelpBox("No internet connection", MessageType.Error);

            if ( _isErrorTooManyRequests )
            {
                TimeSpan leftTime = _diedLineErrorTooManyRequests.Subtract(DateTime.Now);
                EditorGUILayout.HelpBox("The remote server returned an error: (429) Too Many Requests. Need to wait " + _timeNeedForWaitErrorMinute + " minutes. " + leftTime.Minutes + " minutes " + leftTime.Seconds + " left", MessageType.Error);
                if (_diedLineErrorTooManyRequests < DateTime.Now) _isErrorTooManyRequests = false;
            }

            LS = EditorGUILayout.BeginFoldoutHeaderGroup(LS, "Target languages");
            if (LS) _checkListLanguages.DrawButtons();
            EditorGUILayout.EndFoldoutHeaderGroup();

            var names = GetNames();

            if (names.Count != 0)
            {
                WLS = EditorGUILayout.BeginFoldoutHeaderGroup(WLS, "Warning " + names.Count.ToString() + " languages");
                if (WLS)
                {
                    _position = EditorGUILayout.BeginScrollView(_position);
                    foreach (var name in names)
                    {
                        EditorGUILayout.HelpBox(name + " " + translator.GetNameService() + " service does not support some dialects of languages, the choice of language will be changed to the generally accepted.", MessageType.Warning);
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            ValidateLocalizationSettings();
            ValidateLocales();
            ValidateStringTables();

            GUILayout.Space(10);

            if (GUILayout.Button("Translate")) ButtonTranslate_Click();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private List<string> GetNames()
        {
            List<string> strings = new List<string>();
            foreach (RowCheckList rowCheckList in _checkListLanguages.RowCheckLists)
            {
                foreach (Locale locale in _locales)
                {
                    if ( locale != null && locale.name == rowCheckList.Name)
                    {
                        if (rowCheckList.IsActive == true && translator.ValidateLocale(locale.Identifier.Code) == false)
                        {
                            strings.Add(rowCheckList.Name);
                        }
                    }
                }
            }
            return strings;
        }

        private void ButtonTranslate_Click()
        {
            _isErrorConnection = WebInformation.IsConnectedToInternet();
            if (_isErrorConnection ) return;

            EditorUtility.DisplayCancelableProgressBar("Translating", "Load Tables", 0);

            UpdateLocalization();

            EditorUtility.DisplayCancelableProgressBar("Translating", "Preparation translate", 0.1f);

            _selectedLocale = _locales.First(w => w.LocaleName == _dropdownLanguages.Selected);

            TranslateData translateData = new TranslateData();
            translateData.selectedLocale = _selectedLocale;
            translateData.sharedtables = _sharedStringTables.ToList();
            translateData.stringTables = _stringTables.ToList();
            translateData.locales = new List<Locale>();

            foreach (RowCheckList rowCheckList in _checkListLanguages.RowCheckLists)
            {
                if (rowCheckList.IsActive == false ) continue;
                foreach (Locale locale in _locales)
                {
                    if (locale.LocaleName == rowCheckList.Name)
                    {
                        translateData.locales.Add(locale);
                    }
                }
            }

            TranslateLocalization translateLocalization = new TranslateLocalization();

            _translateParameters.FillDictinary(_checkListStringTable.RowCheckLists);

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
                if ( webException.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    _isErrorConnection = true;
                }
                else
                {
                    _diedLineErrorTooManyRequests = DateTime.Now;
                    _diedLineErrorTooManyRequests = _diedLineErrorTooManyRequests.AddMinutes(_timeNeedForWaitErrorMinute);
                    _isErrorTooManyRequests = true;
                }
                EditorUtility.ClearProgressBar();
                return;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            SaveStringTables();

            EditorUtility.DisplayProgressBar("Translating", "Completed", 1f);

            EditorUtility.ClearProgressBar();
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

