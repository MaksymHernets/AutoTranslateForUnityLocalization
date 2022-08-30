using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.Helpers.GUIElements
{
    public class SmartToolbar : IGUI
    {
        public int Selected = 0;
        public List<TabGUI> TabGUIs { get; private set; }
        private Texture2D texture2D;
        private GUIStyle CheckListStyle;

        public SmartToolbar(List<TabGUI> tabGUIs)
		{
            TabGUIs = tabGUIs;
            texture2D = GUIHelper.MakeTex(600, 10, Color.black);
        }

        public void DrawCustom()
		{
            CheckListStyle = new GUIStyle("button");
            CheckListStyle.padding = new RectOffset(7, 7, 7, 7);
            CheckListStyle.normal.background = texture2D;

            int index = 0;
            EditorGUILayout.BeginHorizontal();
            foreach (TabGUI tabGUI in TabGUIs)
			{
                if (Selected == index)
                {
                    if (GUILayout.Button(tabGUI.Name, CheckListStyle)) Selected = index;
                }
                else
                {
                    if (GUILayout.Button(tabGUI.Name)) Selected = index;
                }       
                ++index;
            }
            EditorGUILayout.EndHorizontal();
            TabGUIs[Selected].GUIelement.Draw();
        }

        public void Draw()
        {
            Selected = GUILayout.Toolbar(Selected, TabGUIs.Select(w => w.Name).ToArray());
            TabGUIs[Selected].GUIelement.Draw();
        }
    }

    public class TabGUI
	{
        public string Name;
        public IGUI GUIelement;

        public TabGUI(string name, IGUI gUIelement)
		{
            Name = name;
            GUIelement = gUIelement;
        }
    }
}