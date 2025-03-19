using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EqualchanceGames.Tools.GUIPro
{
    public class CheckListGUI : BaseGUI
	{
		public List<RowCheckList> RowCheckLists { get; private set; }
		public int Width = 1000;
        public int MinWidth = 100;
        public int Height;
        public int MinHeight = 100;
        public Color BackColor = Color.gray;

		private GUIStyle CheckListStyle;
		private Texture2D texture2D;

		private Vector2 vector2 = Vector2.zero;

		public CheckListGUI(List<string> elements, bool isActive = true, bool isAvailable = true, int width = 400, int height = 0)
		{
			Width = width;
			Height = height;
			RowCheckLists = new List<RowCheckList>();
			FillElements(elements, isActive, isAvailable);
			texture2D = HelperGUI.MakeTex(600, 10, BackColor);
		}

		public CheckListGUI(List<RowCheckList> elements, int width = 400, int height = 0)
		{
			Width = width;
			Height = height;
			RowCheckLists = elements;
			texture2D = HelperGUI.MakeTex(600, 10, BackColor);
		}

		public void FillElements(List<string> elements, bool isActive = true, bool isAvailable = true)
		{
			RowCheckLists.Clear();
			foreach (string element in elements)
			{
				RowCheckLists.Add(new RowCheckList(element, isActive, isAvailable));
			}
		}

		private void SetStyle()
		{
            CheckListStyle = new GUIStyle("button");
            CheckListStyle.padding = new RectOffset(7, 7, 7, 7);
            CheckListStyle.normal.background = texture2D;

            vector2 = EditorGUILayout.BeginScrollView(vector2, CheckListStyle, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(Height), GUILayout.MinHeight(MinHeight));
        }

		private void DrawElements()
		{
            foreach (RowCheckList element in RowCheckLists)
            {
                GUI.enabled = element.IsAvailable;
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(element.Name);
                element.IsActive = EditorGUILayout.Toggle(element.IsActive, GUILayout.Width(20));
                EditorGUILayout.EndHorizontal();
            }
        }

		public void Draw()
		{
			SetStyle();

			DrawElements();

            GUI.enabled = true;
			EditorGUILayout.EndScrollView();
		}

        public void DrawButtons()
        {
            SetStyle();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Deselect All")) { RowCheckLists.Where(w => w.IsAvailable == true).ToList().ForEach(w => w.IsActive = false); }
            if (GUILayout.Button("Select All")) { RowCheckLists.Where(w => w.IsAvailable == true).ToList().ForEach(w => w.IsActive = true); }
            EditorGUILayout.EndHorizontal();

            DrawElements();

            GUI.enabled = true;
            EditorGUILayout.EndScrollView();
        }

        public void DrawButtons(string name)
		{
            GUILayout.Label(name);

			DrawButtons();
        }

        public void Update(List<string> elements, bool newvalue = false)
		{
			List<RowCheckList> newRowCheckLists = new List<RowCheckList>();
			bool pair = false;

			foreach (string newelement in elements)
			{
				foreach (RowCheckList rowCheckLists in RowCheckLists)
				{
					if ( rowCheckLists.Name == newelement)
					{
						newRowCheckLists.Add(new RowCheckList(rowCheckLists.Name, rowCheckLists.IsActive, rowCheckLists.IsAvailable));
						pair = true;
					}
				}
				if ( pair == false) newRowCheckLists.Add(new RowCheckList(newelement, newvalue));
				pair = false;
			}

			RowCheckLists = newRowCheckLists;
		}

		public void UpdateCheck(List<string> elements, bool isActive = true, bool isAvailable = true )
		{
            foreach (string newelement in elements)
            {
                foreach (RowCheckList rowCheckLists in RowCheckLists)
                {
                    if (rowCheckLists.Name == newelement)
                    {
                        rowCheckLists.IsActive = isActive;
                        rowCheckLists.IsAvailable = isAvailable;

                    }
                }
            }
        }

        public Dictionary<string, bool> GetElements(bool OnlyActive = true, bool OnlyAvailable = true)
        {
            Dictionary<string, bool> elements = new Dictionary<string, bool>();
            foreach (RowCheckList rowCheckList in RowCheckLists)
            {
                if (rowCheckList.IsActive == OnlyActive && rowCheckList.IsAvailable == OnlyAvailable)
                {
                    elements.Add(rowCheckList.Name, rowCheckList.IsActive);
                }
            }
            return elements;
        }

        public List<string> GetNames(bool OnlyActive = true, bool OnlyAvailable = true)
        {
            List<string> names = new List<string>();
            foreach (RowCheckList rowCheckList in RowCheckLists)
            {
                if (rowCheckList.IsActive == OnlyActive && rowCheckList.IsAvailable == OnlyAvailable)
                {
                    names.Add(rowCheckList.Name);
                }
            }
            return names;
        }

        public bool CheckDifferent(List<string> elements)
		{
            foreach (string newelement in elements)
            {
                foreach (RowCheckList rowCheckLists in RowCheckLists)
                {
                    if (rowCheckLists.Name != newelement)
                    {
						return true;
                    }
                }
            }
            return false;
		}
	}

	public class RowCheckList
	{
		public string Name;
		public bool IsActive;
		public bool IsAvailable;

		public RowCheckList(string name, bool isActive, bool isAvailable = true)
		{
			Name = name;
			IsActive = isActive;
			IsAvailable = isAvailable;
		}
	}
}