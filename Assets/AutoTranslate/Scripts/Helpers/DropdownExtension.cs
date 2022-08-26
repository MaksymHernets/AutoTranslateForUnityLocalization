using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GoodTime.Tools.Helpers
{
    public static class DropdownExtension
    {
        public static string GetLineOptions(this Dropdown dropdown)
		{
            //StringBuilder stringBuilder = new StringBuilder(50);
   //         foreach (string name in dropdown.options.Select(w => w.text))
			//{
   //             stringBuilder.AppendLine(string.Format("[{1}]", name));
   //         }
            return string.Join("][", dropdown.options.Select(w => w.text));
        }

        public static void SetOptions(this Dropdown dropdown, string line)
		{
            dropdown.options.Clear();
            dropdown.AddOptions(line.Split("][".ToCharArray() , 2).ToList());
        }
    }
}