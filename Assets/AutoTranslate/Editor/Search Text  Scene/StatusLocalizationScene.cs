using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
	public class StatusLocalizationScene
    {
        public List<Text> LegacyTexts;
        public List<Dropdown> LegacyDropdowns;

        public List<GameObject> Prefabs;
        public List<LocalizeStringEvent> LocalizeStringEvents;

        public StatusLocalizationScene()
		{
            LegacyTexts = new List<Text>();
            LegacyDropdowns = new List<Dropdown>();
            Prefabs = new List<GameObject>();
            LocalizeStringEvents = new List<LocalizeStringEvent>();
        }

        public override string ToString()
        {
            return String.Format(" Found:\n Text Legacy: {0}\n Localize string event: {1}\n Prefabs: {2}\n Dropdown Legacy: {3}",
                LegacyTexts.Count, LocalizeStringEvents.Count, Prefabs.Count, LegacyDropdowns.Count);
        }
    }
}
