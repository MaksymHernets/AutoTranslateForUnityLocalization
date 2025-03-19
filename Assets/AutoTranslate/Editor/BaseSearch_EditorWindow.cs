using EqualchanceGames.Tools.Helpers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EqualchanceGames.Tools.GUIPro;

namespace EqualchanceGames.Tools.AutoTranslate.Windows
{
    public class BaseSearch_EditorWindow : BaseLocalization_EditorWindow
    {
        protected StatusLocalizationScene _statusLocalizationScene;
        protected SearchTextParameters _searchTextParameters;

        protected string _infoLocalization = string.Empty;
        protected string _nameTable = string.Empty;

        protected bool _skipPrefab = false;
        protected bool _skipVariantPrefab = true;
        protected bool _skipEmptyText = true;
        protected bool _removeMissStringEvents = true;
        protected bool _autoSave = true;

        protected const string KEYWORD_NEWTABLE = "-New-";

        protected DropdownGUI _dropdownTables;
        protected CheckListGUI _checkListSkipParentComponents;
        protected CheckListGUI _checkListSearchComponents;
        protected ToolbarGUI _TabsGUI;

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

            _dropdownLanguages.Width = 200;

            List<string> tablelists = new List<string>();
            if (_sharedStringTables != null) tablelists = _sharedStringTables.Select(w => w.TableCollectionName).ToList();
            tablelists.Add(KEYWORD_NEWTABLE);

            _dropdownTables = new DropdownGUI("Select string Table", tablelists);
            _dropdownTables.Width = 200;
            _dropdownTables.Selected = KEYWORD_NEWTABLE;

            _searchTextParameters = new SearchTextParameters();

            List<RowCheckList> rowCheckLists = SearchTextForLocalization.GetAvailableForSearchUIComponents();
            _checkListSearchComponents = new CheckListGUI(rowCheckLists, 300, 400);
            _checkListSearchComponents.MinHeight = 100;

            List<RowCheckList> rowCheckSkipParentLists = SearchTextForLocalization.GetAvailableForSkipParentComponents();
            _checkListSkipParentComponents = new CheckListGUI(rowCheckSkipParentLists, 300, 500);
            _checkListSkipParentComponents.MinHeight = 160;

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
            _TabsGUI = new ToolbarGUI(tabGUIs);
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            _checkListSearchComponents?.Update(SearchTextForLocalization.GetAvailableForSearchUIComponents().Select(w=>w.Name).ToList());
        }

        protected void FillDispalay_StatusLocalization()
		{
            _checkLists[0].FillElements(_statusLocalizationScene.LegacyTexts.Select(w => w.gameObject.GetFullName(w.text)).ToList());
            _checkLists[2].FillElements(_statusLocalizationScene.TextMeshProUIs.Select(w => w.gameObject.GetFullName(w.text)).ToList());
            _checkLists[1].FillElements(_statusLocalizationScene.TextMeshPros.Select(w => w.gameObject.GetFullName(w.text)).ToList());
            _checkLists[3].FillElements(_statusLocalizationScene.LegacyMeshTexts.Select(w => w.gameObject.GetFullName(w.text)).ToList());
        }

        protected void GetCheckTable()
		{
            _statusLocalizationScene.LegacyTexts = GetBack<Text>(_statusLocalizationScene.LegacyTexts, _checkLists[0].GetElements());
            _statusLocalizationScene.TextMeshProUIs = GetBack<TextMeshProUGUI>(_statusLocalizationScene.TextMeshProUIs, _checkLists[2].GetElements());
            _statusLocalizationScene.TextMeshPros = GetBack<TextMeshPro>(_statusLocalizationScene.TextMeshPros, _checkLists[1].GetElements());
            _statusLocalizationScene.LegacyMeshTexts = GetBack<TextMesh>(_statusLocalizationScene.LegacyMeshTexts, _checkLists[3].GetElements());
        }

        protected List<T> GetBack<T>(List<T> lists, Dictionary<string, bool> keyValuePairs)
		{
            List<T> newsList = new List<T>();
            int index = 0;
            //if ( keyValuePairs.Count != lists.Count) return newsList;
            foreach (var item in keyValuePairs)
			{
                if (item.Value == true) newsList.Add(lists[index]);
                ++index;
            }
            return newsList;
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
            _prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        }
    }
}
