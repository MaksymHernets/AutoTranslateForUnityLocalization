using GoodTime.Tools.Helpers;
using GoodTime.Tools.Helpers.GUIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class AutoTranslate_EditorWindow : BaseLocalization_EditorWindow
    {
        // Window parameters
        protected const string k_WindowTitle = "Auto Translate for Localization";

        // Arguments for execute
        private TranslateParameters _translateParameters = new TranslateParameters();

        // Error
        private bool _isErrorTooManyRequests = false;
        private DateTime _diedLineErrorTooManyRequests;
        private double _timeNeedForWaitErrorMinute = 10;
        private bool _isErrorConnection = false;
        private CheckListGUI _checkListStringTable;

        [MenuItem("Window/Auto Localization/Auto Translate for String Tables", false, 1)]
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
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            _isErrorConnection = WebInformation.IsConnectedToInternet();

            if (_sharedStringTables != null)
                _checkListStringTable.Update(_sharedStringTables.Select(w => w.TableCollectionName).ToList());
        }

        void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);
            EditorGUIUtility.labelWidth = k_SeparationWidth;

            _dropdownLanguages.Draw();

            _translateParameters.canOverrideWords = EditorGUILayout.Toggle("Override words that have a translation", _translateParameters.canOverrideWords);
            _translateParameters.canTranslateEmptyWords = EditorGUILayout.Toggle("Translate words that don't have a translation", _translateParameters.canTranslateEmptyWords);
            _translateParameters.canTranslateSmartWords = EditorGUILayout.Toggle("Translate smart words", _translateParameters.canTranslateSmartWords);

            GUILayout.Space(10);

            EditorGUILayout.HelpBox("  Found " + _locales?.Count + " languages" +
                "\n  Found " + _sharedStringTables?.Count + " table collection" + 
                "\n  Found " + _stringTables?.Count + " string tables" + 
                "\n  Found " + _assetTables?.Count + " asset tables", MessageType.Info);

            EditorGUILayout.LabelField("Selected collection tables for translation:");
            _checkListStringTable.Draw();

            if ( _isErrorConnection ) EditorGUILayout.HelpBox("No internet connection", MessageType.Error);

            if ( _isErrorTooManyRequests )
            {
                TimeSpan leftTime = _diedLineErrorTooManyRequests.Subtract(DateTime.Now);
                EditorGUILayout.HelpBox("The remote server returned an error: (429) Too Many Requests. Need to wait " + _timeNeedForWaitErrorMinute + " minutes. " + leftTime.Minutes + " minutes " + leftTime.Seconds + " left", MessageType.Error);
                if (_diedLineErrorTooManyRequests < DateTime.Now) _isErrorTooManyRequests = false;
            }

            ValidateLocalizationSettings();
            ValidateLocales();
            ValidateStringTables();

            GUILayout.Space(10);

            if (GUILayout.Button("Translate")) ButtonTranslate_Click();
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
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                EditorUtility.ClearProgressBar();
                return;
            }

            _isErrorConnection = false;

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

