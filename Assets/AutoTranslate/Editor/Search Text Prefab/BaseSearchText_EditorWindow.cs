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
        protected CheckListGUI _checkListGUI;

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

            List<string> Checklists = new List<string>();
            Checklists.Add("Text Legacy");
            Checklists.Add("Dropdown Legacy");
            Checklists.Add("Text Mesh Pro");
            Checklists.Add("Dropdown Mesh Pro");
            _checkListGUI = new CheckListGUI(Checklists);
        }

        //protected void Dropdown_StringTables(int width = 300)
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    EditorGUILayout.LabelField("Select string Table", GUILayout.Width(width));
        //    if (EditorGUILayout.DropdownButton(new GUIContent(_selectedTable), FocusType.Passive))
        //    {
        //        Rect posit = new Rect(new Vector2(width, 90), new Vector2(400, 20));
        //        genericMenu = new GenericMenu();
        //        foreach (string option in _sharedStringTables.Select(w => w.TableCollectionName))
        //        {
        //            genericMenu.AddItem(new GUIContent(option), option == _selectedTable, () =>
        //            {
        //                _selectedTable = option;
        //            });
        //        }
        //        genericMenu.AddItem(new GUIContent(KEYWORD_NEWTABLE), KEYWORD_NEWTABLE == _selectedTable, () =>
        //        {
        //            _selectedTable = KEYWORD_NEWTABLE;
        //        });
        //        genericMenu.DropDown(posit);
        //    }
        //    EditorGUILayout.EndHorizontal();
        //}
    }
}
