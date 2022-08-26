using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate.UI
{
	public class DropdownLanguage : MonoBehaviour
	{
		[SerializeField] private Dropdown dropdown;

		private List<Locale> Locales;

		private void Start()
		{
			dropdown.onValueChanged.AddListener(Dropdown_Change);
			dropdown.ClearOptions();

			Locales = LocalizationSettings.AvailableLocales.Locales;
			if ( Locales != null && Locales.Count != 0)
			dropdown.AddOptions(Locales.Select(w => w.LocaleName).ToList());
		}

		private void Dropdown_Change(int index)
		{
			if ( index < Locales.Count )
			LocalizationSettings.SelectedLocale = Locales[index];
		}
	}
}
