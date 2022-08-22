using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class BaseSearchText_EditorWindow : BaseLocalization_EditorWindow
    {
        protected StatusLocalizationScene _statusLocalizationScene;
        protected string _infoLocalization = string.Empty;
        protected string _selectedTable = KEYWORD_NEWTABLE;
        protected string _nameTable = string.Empty;
        protected bool _skipPrefab;
        protected const string KEYWORD_NEWTABLE = "-New-";

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

        protected void Dropdown_StringTables(int width = 300)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select string Table", GUILayout.Width(width));
            if (EditorGUILayout.DropdownButton(new GUIContent(_selectedTable), FocusType.Passive))
            {
                Rect posit = new Rect(new Vector2(width, 90), new Vector2(400, 20));
                genericMenu = new GenericMenu();
                foreach (string option in _sharedStringTables.Select(w => w.TableCollectionName))
                {
                    genericMenu.AddItem(new GUIContent(option), option == _selectedTable, () =>
                    {
                        _selectedTable = option;
                    });
                }
                genericMenu.AddItem(new GUIContent(KEYWORD_NEWTABLE), KEYWORD_NEWTABLE == _selectedTable, () =>
                {
                    _selectedTable = KEYWORD_NEWTABLE;
                });
                genericMenu.DropDown(posit);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
