using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.Helpers.GUI
{
	public class CheckListGUI
	{
		public List<RowCheckList> RowCheckLists;
		public int Width;
		public Color BackColor = Color.white;

		private GUIStyle CheckListStyle;
		private Texture2D texture2D;

		public CheckListGUI(List<string> elements, bool isActive = true, int width = 400)
		{
			Width = width;
			RowCheckLists = new List<RowCheckList>();
			foreach (string element in elements)
			{
				RowCheckLists.Add(new RowCheckList(element, isActive));
			}
			texture2D = MakeTex(600, 10, BackColor);
		}

		public CheckListGUI(List<RowCheckList> elements, int width = 400)
		{
			Width = width;
			RowCheckLists = elements;
			texture2D = MakeTex(600, 10, BackColor);
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
	}

	public class RowCheckList
	{
		public string Name;
		public bool IsActive;

		public RowCheckList(string name, bool isActive)
		{
			Name = name;
			IsActive = isActive;
		}
	}
}