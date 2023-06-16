using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GoodTime.Tools.Helpers.GUIElements
{
    public class DropdownGUI : IGUI
    {
        public List<string> Options { get; private set; }
        public UnityAction<string> UpdateSelected;

        public string Name;
        public string Selected;
        public int Width;

#if UNITY_EDITOR
        private GenericMenu genericMenu;
#endif
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

        public void Draw()
        {
#if UNITY_EDITOR
            Position = EditorGUILayout.BeginHorizontal();
            Position.x = Width;
            EditorGUILayout.LabelField(Name, GUILayout.Width(Width));
            if (EditorGUILayout.DropdownButton(new GUIContent(Selected), FocusType.Passive))
            {
                genericMenu = new GenericMenu();
                foreach (GUIContent content in GUIContents)
                {
                    genericMenu.AddItem(content, content.text == Selected, () =>
                    {
                        Selected = content.text;
                        UpdateSelected?.Invoke(Selected);
                    });
                }
                genericMenu.DropDown(Position);
            }
            EditorGUILayout.EndHorizontal();
#endif
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