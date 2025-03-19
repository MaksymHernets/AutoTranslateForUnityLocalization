using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace EqualchanceGames.Tools.GUIPro
{
    public class DropdownGUI : BaseGUI
    {
        public List<string> Options { get; private set; }
        public UnityAction<string> UpdateSelected;

        public string Name;
        public string Selected;
        public int Width;
        public string Filter = "";

        private GenericMenu genericMenu;
        private List<GUIContent> GUIContents;
        private Rect Position;

        private const string ConstString = "";
        protected const string KEYWORD_NEWTABLE = "-New-";

        public DropdownGUI(string label, List<string> options, string selected = ConstString, int width = 400, bool newOption = false)
		{
            Name = label;
            Options = options;
            if (string.IsNullOrEmpty(selected)) Selected = options.FirstOrDefault();
            else Selected = selected;
            Width = width;
            Position = new Rect(new Vector2(0, 0), new Vector2(width, 0));

            GUIContents = new List<GUIContent>();
            foreach (string option in Options)
            {
                GUIContents.Add(new GUIContent(option));
            }
        }

        public void AddOption(string name)
		{
            Options.Add(name);
            GUIContents.Add(new GUIContent(name));
        }

        public void AddOptions(List<string> names)
        {
            foreach (string name in names)
            {
                AddOption(name);
            }
        }

        public void ClearOptions()
        {
            Options.Clear();
            GUIContents.Clear();
        }

        public void Draw()
        {
            Position = EditorGUILayout.BeginHorizontal();
            Position.x = Position.x + Width;
            EditorGUILayout.LabelField(Name, GUILayout.Width(Width));
            if (EditorGUILayout.DropdownButton(new GUIContent(Selected), FocusType.Passive))
            {
                genericMenu = new GenericMenu();
                foreach (GUIContent content in GUIContents)
                {
                    if ( content.text.Contains(KEYWORD_NEWTABLE) || content.text.ToLower().Contains(Filter) )
                    {
                        genericMenu.AddItem(content, content.text == Selected, () =>
                        {
                            Selected = content.text;
                            UpdateSelected?.Invoke(Selected);
                        });
                    }
                }
                genericMenu.DropDown(Position);
            }
            EditorGUILayout.EndHorizontal();
        }

        public void DrawFilter(string name = "Filter:")
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.Width(Width));
            Filter = EditorGUILayout.TextField(Filter);
            EditorGUILayout.EndHorizontal();
        }

        public void DrawNewLine()
		{
            if (Selected == KEYWORD_NEWTABLE)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New string table", GUILayout.Width(Width));
                Name = EditorGUILayout.TextField("", Name);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}