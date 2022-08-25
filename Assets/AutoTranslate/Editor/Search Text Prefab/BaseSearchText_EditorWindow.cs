using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using GoodTime.Tools.Helpers.GUI;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class BaseSearchText_EditorWindow : BaseLocalization_EditorWindow
    {
        protected StatusLocalizationScene _statusLocalizationScene;
        protected string _infoLocalization = string.Empty;
        protected string _nameTable = string.Empty;
        protected bool _skipPrefab;
        protected const string KEYWORD_NEWTABLE = "-New-";

        protected DropdownGUI _dropdownTables;
        protected CheckListGUI _checkListSearchElements;

        protected void CheckNameStringTable()
        {
            if (SimpleInterfaceStringTable.CheckNameStringTable(_nameTable))
            {
                EditorGUILayout.HelpBox("StringTable - " + _nameTable + " exists. In this case, the table will be cleared and filled again. ", MessageType.Warning);
            }

            if (string.IsNullOrEmpty(_nameTable))
            {
                EditorGUILayout.HelpBox("Name table is empty", MessageType.Error);
                GUI.enabled = false;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            List<string> Tablelists = _sharedStringTables.Select(w => w.TableCollectionName).ToList();
            Tablelists.Add(KEYWORD_NEWTABLE);
            _dropdownTables = new DropdownGUI("Select string Table", Tablelists);
            _dropdownTables.Selected = KEYWORD_NEWTABLE;

            _checkListSearchElements = new CheckListGUI(SearchTextForLocalization.GetAvailableForSearchUIElements());
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            _checkListSearchElements.Update(SearchTextForLocalization.GetAvailableForSearchUIElements());
        }
    }
}
