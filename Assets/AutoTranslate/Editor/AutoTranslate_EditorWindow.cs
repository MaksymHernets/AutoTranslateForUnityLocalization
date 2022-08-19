using GoodTime.Tools.Helpers;
using System;
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

        [MenuItem("Window/Auto Localization/Auto Translate for Tables")]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            AutoTranslate_EditorWindow window = GetWindow<AutoTranslate_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle);
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateParameter();
            _isErrorConnection = WebInformation.IsConnectedToInternet();
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            UpdateParameter();
            _isErrorConnection = WebInformation.IsConnectedToInternet();
        }

        protected void UpdateParameter()
        {
            if ( _sharedStringTables != null)
            {
                InitializationTranslateTableCollections();
            }
        }

        void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);
            SourceLanguage(k_SeparationWidth);

            EditorGUIUtility.labelWidth = k_SeparationWidth;
            _translateParameters.canOverrideWords = EditorGUILayout.Toggle("Override words that have a translation", _translateParameters.canOverrideWords);
            _translateParameters.canTranslateEmptyWords = EditorGUILayout.Toggle("Translate words that don't have a translation", _translateParameters.canTranslateEmptyWords);
            _translateParameters.canTranslateSmartWords = EditorGUILayout.Toggle("Translate smart words", _translateParameters.canTranslateSmartWords);

            GUILayout.Space(10);
            EditorGUILayout.HelpBox("  Found " + _locales?.Count + " languages" +
                "\n  Found " + _sharedStringTables?.Count + " table collection" + 
                "\n  Found " + _stringTables?.Count + " string tables" + 
                "\n  Found " + _assetTables?.Count + " asset tables", MessageType.Info);

            
            if (_sharedStringTables != null)
            {
                EditorGUILayout.LabelField("Selected collection tables for translation:", GUILayout.Width(k_SeparationWidth));
                EditorGUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(10,10,10,10) });
                int index = 0;
                foreach (var sharedtable in _sharedStringTables)
                {
                    _translateParameters.canTranslateTableCollections[index] = EditorGUILayout.ToggleLeft(sharedtable.TableCollectionName, _translateParameters.canTranslateTableCollections[index]);
                    ++index;
                }
                
                EditorGUILayout.EndVertical();
            }

            if ( _isErrorConnection )
            {
                EditorGUILayout.HelpBox("No internet connection", MessageType.Error);
            }
            if ( _isErrorTooManyRequests )
            {
                TimeSpan leftTime = _diedLineErrorTooManyRequests.Subtract(DateTime.Now);
                EditorGUILayout.HelpBox("The remote server returned an error: (429) Too Many Requests. Need to wait " + _timeNeedForWaitErrorMinute + " minutes. " + leftTime.Minutes + " minutes " + leftTime.Seconds + " left", MessageType.Error);
                if (_diedLineErrorTooManyRequests < DateTime.Now)
                {
                    _isErrorTooManyRequests = false;
                }
            }
            ValidLocalization();
            GUILayout.Space(10);
            if (GUILayout.Button("Translate"))
            {
                ButtonTranslate_Click();
            }
        }

        private void ButtonTranslate_Click()
        {
            _isErrorConnection = WebInformation.IsConnectedToInternet();
            if (_isErrorConnection )
            {
                return;
            }

            EditorUtility.DisplayCancelableProgressBar("Translating", "Load Tables", 0);

            LoadSettings();

            EditorUtility.DisplayCancelableProgressBar("Translating", "Preparation translate", 0.1f);

            _selectedLocale = _locales.First(w => w.LocaleName == _selectedLanguage);

            TranslateData translateData = new TranslateData();
            translateData.selectedLocale = _selectedLocale;
            translateData.sharedtables = _sharedStringTables.ToList();
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
                EditorUtility.ClearProgressBar();
                return;
            }

            _isErrorConnection = false;

            SaveStringTables();

            EditorUtility.DisplayProgressBar("Translating", "Completed", 1f);

            EditorUtility.ClearProgressBar();
        }

        private void InitializationTranslateTableCollections()
        {
            _translateParameters.canTranslateTableCollections.Clear();
            foreach (var item in _sharedStringTables)
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

