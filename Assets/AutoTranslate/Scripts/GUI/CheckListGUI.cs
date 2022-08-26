using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.Helpers.GUI
{
	public class CheckListGUI
	{
		public List<RowCheckList> RowCheckLists { get; private set; }
		public int Width;
		public Color BackColor = Color.white;

		private GUIStyle CheckListStyle;
		private Texture2D texture2D;

		public CheckListGUI(List<string> elements, bool isActive = true, int width = 400)
		{
			Width = width;
			RowCheckLists = new List<RowCheckList>();
			FillElements(elements, isActive);
			texture2D = MakeTex(600, 10, BackColor);
		}

		public CheckListGUI(List<RowCheckList> elements, int width = 400)
		{
			Width = width;
			RowCheckLists = elements;
			texture2D = MakeTex(600, 10, BackColor);
		}

		private void FillElements(List<string> elements, bool isActive = true)
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

			EditorGUILayout.BeginVertical(CheckListStyle);
			foreach (RowCheckList element in RowCheckLists)
			{
				element.IsActive = EditorGUILayout.Toggle(element.Name, element.IsActive);
			}
			EditorGUILayout.EndVertical();
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
						newRowCheckLists.Add(new RowCheckList(rowCheckLists.Name, rowCheckLists.IsActive));
						pair = true;
					}
				}
				if ( pair == false) newRowCheckLists.Add(new RowCheckList(newelement, true));
				pair = false;
			}

			RowCheckLists = newRowCheckLists;
		}

		private Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width*height];
 
			for(int i = 0; i < pix.Length; i++)
				pix[i] = col;
 
			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();
 
			return result;
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