using System.Collections.Generic;
using System.Linq;
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
        private const string k_WindowTitle = "Auto Translate for Localization";
        static readonly Vector2 k_MinSize = new Vector2(900, 600);

        private LocalizationSettings _localizationSettings;
        private TypeStage _typeStage;
        private List<Locale> _locales;

        private string _selectedLocale = "English";
        private bool _isTranslateAll = true;
        
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

            this.minSize = k_MinSize;

            LoadSettings();

            if (_locales.Count != 0)
            {
                _selectedLocale = _locales[0].name;
            }
        }

        private void OnFocus()
        {
            LoadSettings();
        }

        private async void LoadSettings()
        {
            string[] guids = AssetDatabase.FindAssets("Localization Settings t:LocalizationSettings", null);

            if ( guids.Length != 0 )
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);

                _localizationSettings = AssetDatabase.LoadAssetAtPath<LocalizationSettings>(path);

                _locales =  _localizationSettings.GetAvailableLocales().Locales;

                Locale selectedLocale = await _localizationSettings.GetSelectedLocaleAsync().Task;

                if (selectedLocale != null)
                {
                    _selectedLocale = selectedLocale.name;
                }
                
                _typeStage = TypeStage.Ready;
            } 
        }

        private async void LoadLaunchers()
        {
            _locales = new List<Locale>();

            List<string> labels = new List<string>();
            labels.Add("Locale");
            IList<IResourceLocation> locations = await Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.None, typeof(Locale)).Task;

            foreach (var location in locations)
            {
                _locales.Add(await Addressables.LoadAssetAsync<Locale>(location).Task);
            }

            // It is no work. Why?. I dont know...
            //IList<Locale> lists = await Addressables.LoadAssetsAsync<Locale>(locations, null, Addressables.MergeMode.Union).Task;

            _typeStage = TypeStage.Ready;
        }

        void OnGUI()
        {
            if (_typeStage == TypeStage.Loading)
            {
                GUILayout.Label("Please wait. Search Localization Settings", EditorStyles.boldLabel);
            }
            else if (_typeStage == TypeStage.Ready)
            {
                GUILayout.Label("Source Launcher", EditorStyles.boldLabel);
                var posit = new Rect(new Vector2(100, 100), new Vector2(200, 20));
                if (EditorGUI.DropdownButton(posit, new GUIContent(_selectedLocale), FocusType.Passive))
                {
                    var genericMenu = new GenericMenu();

                    foreach (var option in _locales.Select(w => w.name))
                    {
                        bool selected = option == _selectedLocale;
                        genericMenu.AddItem(new GUIContent(option), selected, () =>
                        {
                            _selectedLocale = option;
                        });
                    }
                    genericMenu.DropDown(posit);
                }

                _isTranslateAll = GUI.Toggle(new Rect(new Vector2(100, 200), new Vector2(200, 20)), _isTranslateAll, "Translate all words or only empty words");

                if (GUI.Button(new Rect(new Vector2(100, 300), new Vector2(200, 20)), "Translate"))
                {
                    Translate();
                }
            }
            else if (_typeStage == TypeStage.Translating)
            {
                GUILayout.Label("Translating", EditorStyles.boldLabel);
            }
        }

        private void Translate()
        {
            _typeStage = TypeStage.Translating;
            LoadTables();
        }

        public async void LoadTables()
        {
            List<string> labels = new List<string>();

            foreach (var item in _locales)
            {
                labels.Add("Locale-" + item.Formatter);      
            }

            IList<IResourceLocation> locations = await Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.Union, typeof(StringTable)).Task;

            var tables = await Addressables.LoadAssetsAsync<StringTable>(locations, null).Task;

            foreach (var table in tables)
            {
                //table["SampleText"].Value = "sample";

                table.AddEntry("SampleText", "sample4");
            }

            _typeStage = TypeStage.Ready;
        }
    }

    public enum TypeStage
    {
        Loading,
        Ready,
        Translating,
        Done
    }
}

