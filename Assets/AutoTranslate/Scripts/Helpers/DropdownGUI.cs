using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GoodTime.Tools.Helpers.GUI
{
    public class DropdownGUI
    {
        public List<string> Options { get; private set; }
        public UnityAction<string> UpdateSelected;

        public string Name;
        public string Selected;
        public int Width;

        private GenericMenu genericMenu;
        private List<GUIContent> GUIContents;
        private Rect Position;

        private const string ConstString = "";

        public DropdownGUI(string label, List<string> options, string selected = ConstString, int width = 400)
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
        }
    }
}