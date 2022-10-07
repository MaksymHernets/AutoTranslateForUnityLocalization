using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.Helpers.GUIElements
{
	public class CheckListGUI : IGUI
	{
		public List<RowCheckList> RowCheckLists { get; private set; }
		public int Width;
		public int Height;
		public Color BackColor = Color.white;

		private GUIStyle CheckListStyle;
		private Texture2D texture2D;

		private Vector2 vector2 = Vector2.zero;

		public CheckListGUI(List<string> elements, bool isActive = true, int width = 400, int height = 0)
		{
			Width = width;
			Height = height;
			RowCheckLists = new List<RowCheckList>();
			FillElements(elements, isActive);
			texture2D = GUIHelper.MakeTex(600, 10, BackColor);
		}

		public CheckListGUI(List<RowCheckList> elements, int width = 400, int height = 0)
		{
			Width = width;
			Height = height;
			RowCheckLists = elements;
			texture2D = GUIHelper.MakeTex(600, 10, BackColor);
		}

		public void FillElements(List<string> elements, bool isActive = true)
		{
			RowCheckLists.Clear();
			foreach (string element in elements)
			{
				RowCheckLists.Add(new RowCheckList(element, isActive));
			}
		}

		public void Draw()
		{
			CheckListStyle = new GUIStyle("button");
			CheckListStyle.padding = new RectOffset(7, 7, 7, 7);
			CheckListStyle.normal.background = texture2D;

			vector2 = EditorGUILayout.BeginScrollView(vector2, CheckListStyle, GUILayout.MaxHeight(Height));
			foreach (RowCheckList element in RowCheckLists)
			{
				GUI.enabled = element.IsAvailable;
				EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
				EditorGUILayout.LabelField(element.Name);
				element.IsActive = EditorGUILayout.Toggle(element.IsActive, GUILayout.Width(20));
				EditorGUILayout.EndHorizontal();
			}
			GUI.enabled = true;
			EditorGUILayout.EndScrollView();
		}

		public void Update(List<string> elements)
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
				if ( pair == false) newRowCheckLists.Add(new RowCheckList(newelement, true));
				pair = false;
			}

			RowCheckLists = newRowCheckLists;
		}

		public Dictionary<string, bool> GetElements()
		{
			Dictionary<string, bool> elements = new Dictionary<string, bool>();
			foreach (RowCheckList rowCheckList in RowCheckLists)
			{
				elements.Add(rowCheckList.Name, rowCheckList.IsActive);
			}
			return elements;
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