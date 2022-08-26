using GoodTime.Tools.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    [AddComponentMenu("Localization/Localize Dropdown Event")]
    public class LocalizeDropdownEvent : LocalizeStringEvent
    {
        [SerializeField] private Dropdown _dropdown;

        public void set_text(string text)
		{
            _dropdown.SetOptions(text);
        }
    }
}