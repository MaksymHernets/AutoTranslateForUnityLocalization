using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.Helpers.GUIElements
{
	public class TableGUI : IGUI
	{
		public List<ColumnTableGUI> Columns { get; private set; }
		public List<RowTableGUI> Rows { get; private set; }

		private Vector2 vector2 = Vector2.zero;

		public TableGUI()
		{
			Columns = new List<ColumnTableGUI>();
			Rows = new List<RowTableGUI>();
		}

		public TableGUI(List<ColumnTableGUI> columns)
		{
			Columns = columns;
			Rows = new List<RowTableGUI>();
		}

		public TableGUI(List<ColumnTableGUI> columns, List<RowTableGUI> rows)
		{
			Columns = columns;
			AddRowRange(rows);
		}

		public void AddColumn(TypeColumn typeColumn, string name)
		{
			Columns.Add(new ColumnTableGUI(typeColumn, name));
		}

		public void AddRowRange(List<RowTableGUI> rows)
		{
			Rows.Clear();
			Rows = rows;
		}

		public void Draw()
		{
			vector2 = EditorGUILayout.BeginScrollView(vector2);
			DrawColumns();
			DrawRows();
			EditorGUILayout.EndScrollView();
		}

		private void DrawColumns()
		{
			EditorGUILayout.BeginHorizontal();
			foreach (ColumnTableGUI column in Columns)
			{
				GUILayout.Button(column.Name);
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawRows()
		{
			int index = 0;
			foreach (RowTableGUI row in Rows)
			{
				index = 0;
				EditorGUILayout.BeginHorizontal();
				foreach (ItemTableGUI item in row.Items)
				{
					switch (Columns[index].TypeColumns)
					{
						case TypeColumn.Text:
							EditorGUILayout.LabelField(item.Text);
							break;
						case TypeColumn.Toggle:
							item.Toggle = EditorGUILayout.Toggle(item.Toggle);
							break;
						case TypeColumn.Button:
							GUILayout.Button(item.Text);
							break;
						case TypeColumn.InputField:
							item.Text = EditorGUILayout.TextField(item.Text);
							break;
					}
					++index;
				}
				EditorGUILayout.EndHorizontal();
			}
		}
	}

	public class ColumnTableGUI
	{
		public TypeColumn TypeColumns;
		public string Name;

		public ColumnTableGUI(TypeColumn typeColumns, string name)
		{
			TypeColumns = typeColumns;
			Name = name;
		}
	}

	public class RowTableGUI
	{
		public List<ItemTableGUI> Items;

		public RowTableGUI()
		{
			Items = new List<ItemTableGUI>();
		}

		public void AddText(string text)
		{
			Items.Add(new ItemTableGUI() { Text = text });
		}

		public void AddToggle(bool toggle)
		{
			Items.Add(new ItemTableGUI() { Toggle = toggle });
		}

		public void AddIndex(int index)
		{
			Items.Add(new ItemTableGUI() { Index = index });
		}
	}

	public class ItemTableGUI
	{
		public string Text = string.Empty;
		public bool Toggle = true;
		public int Index = 0;
	}

	public enum TypeColumn
	{
		Text = 0,
		Toggle = 1,
		InputField = 2,
		Button = 3,
		Dropdown = 4,
	}
}
