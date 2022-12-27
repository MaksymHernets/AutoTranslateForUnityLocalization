using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.Helpers;
using GoodTime.Tools.Helpers.GUIElements;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class BaseSearch_EditorWindow : BaseLocalization_EditorWindow
    {
        protected PrefabStage _prefabStage;
        protected Scene _currentScene;

        protected StatusLocalizationScene _statusLocalizationScene;
        protected SearchTextParameters _searchTextParameters;

        protected string _infoLocalization = string.Empty;
        protected string _nameTable = string.Empty;

        protected bool _skipPrefab = true;
        protected bool _skipEmptyText = true;
        protected bool _removeMissStringEvents = true;
        protected bool _autoSave = true;

        protected const string KEYWORD_NEWTABLE = "-New-";

        protected DropdownGUI _dropdownTables;
        protected CheckListGUI _checkListSearchElements;
        protected SmartToolbar _TabsGUI;

        protected List<CheckListGUI> _checkLists;

        protected void CheckNameStringTable()
        {
            if (SimpleInterfaceStringTable.CheckNameStringTable(_nameTable))
            {
                EditorGUILayout.HelpBox("StringTable - " + _nameTable + " exists. In this case, the table will be cleared and filled again. ", MessageType.Warning);
            }
        }

        protected void IsNullOrEmpty_NameStringTable()
        {
            if (string.IsNullOrEmpty(_nameTable))
            {
                EditorGUILayout.HelpBox("Name table is empty", MessageType.Error);
                GUI.enabled = false;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            List<string> tablelists = new List<string>();
            if (_sharedStringTables != null) tablelists = _sharedStringTables.Select(w => w.TableCollectionName).ToList();
            tablelists.Add(KEYWORD_NEWTABLE);

            _dropdownTables = new DropdownGUI("Select string Table", tablelists);
            _dropdownTables.Width = k_SeparationWidth;
            _dropdownTables.Selected = KEYWORD_NEWTABLE;

            _searchTextParameters = new SearchTextParameters();

            List<RowCheckList> rowCheckLists = SearchTextForLocalization.GetAvailableForSearchUIElements();
            _checkListSearchElements = new CheckListGUI(rowCheckLists, 300, 150);

            _checkLists = new List<CheckListGUI>();
            List<TabGUI> tabGUIs = new List<TabGUI>();
            foreach (var item in rowCheckLists)
			{
                CheckListGUI checkListGUI = new CheckListGUI(new List<string>());
                checkListGUI.Width = 1000;
                checkListGUI.Height = 800;
                _checkLists.Add(checkListGUI);
                tabGUIs.Add(new TabGUI(item.Name, checkListGUI));
            }
            _TabsGUI = new SmartToolbar(tabGUIs);
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            _checkListSearchElements?.Update(SearchTextForLocalization.GetAvailableForSearchUIElements().Select(w=>w.Name).ToList());
        }

        protected void FillDispalay_StatusLocalization()
		{
            _checkLists[0].FillElements(_statusLocalizationScene.LegacyTexts.Select(w => w.gameObject.GetFullName(w.text)).ToList());
            _checkLists[1].FillElements(_statusLocalizationScene.TextMeshs.Select(w => w.gameObject.GetFullName(w.text)).ToList());
            _checkLists[2].FillElements(_statusLocalizationScene.LegacyDropdowns.Select(w => w.gameObject.GetFullName(w.captionText.text)).ToList());
            _checkLists[3].FillElements(_statusLocalizationScene.TMP_Dropdowns.Select(w => w.gameObject.GetFullName(w.captionText.text)).ToList());
        }

        protected void GetCheckTable()
		{
            _statusLocalizationScene.LegacyTexts = GetBack<Text>(_statusLocalizationScene.LegacyTexts, _checkLists[0].GetElements());
            _statusLocalizationScene.TextMeshs = GetBack<TextMeshProUGUI>(_statusLocalizationScene.TextMeshs, _checkLists[1].GetElements());
            _statusLocalizationScene.LegacyDropdowns = GetBack<Dropdown>(_statusLocalizationScene.LegacyDropdowns, _checkLists[2].GetElements());
            _statusLocalizationScene.TMP_Dropdowns = GetBack<TMP_Dropdown>(_statusLocalizationScene.TMP_Dropdowns, _checkLists[3].GetElements());
        }

        protected List<T> GetBack<T>(List<T> lists, Dictionary<string, bool> keyValuePairs)
		{
            List<T> newsList = new List<T>();
            int index = 0;
			foreach (var item in keyValuePairs)
			{
                if (item.Value == true) newsList.Add(lists[index]);
                ++index;
            }
            return newsList;
        }

        protected void Toggle_SkipPrefabs()
		{
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Skip prefabs", GUILayout.Width(k_SeparationWidth));
            _skipPrefab = EditorGUILayout.Toggle(_skipPrefab);
            EditorGUILayout.EndHorizontal();
        }

        protected void Toggle_SkipEmptyText()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Skip empty text", GUILayout.Width(k_SeparationWidth));
            _skipEmptyText = EditorGUILayout.Toggle(_skipEmptyText);
            EditorGUILayout.EndHorizontal();
        }

        protected void Toggle_RemoveMissStringEvents()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Remove miss stringEvents", GUILayout.Width(k_SeparationWidth));
            _removeMissStringEvents = EditorGUILayout.Toggle(_removeMissStringEvents);
            EditorGUILayout.EndHorizontal();
        }

        protected void Toggle_AutoSave()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Auto Save", GUILayout.Width(k_SeparationWidth));
            _autoSave = EditorGUILayout.Toggle(_autoSave);
            EditorGUILayout.EndHorizontal();
        }

        protected void TextField_NewStringTable()
		{
            if (_dropdownTables.Selected == KEYWORD_NEWTABLE)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New string table", GUILayout.Width(k_SeparationWidth));
                _nameTable = EditorGUILayout.TextField("", _nameTable);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void Check()
		{
            _currentScene = DatabaseProject.GetCurrentScene();
            _prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        }
    }
}
