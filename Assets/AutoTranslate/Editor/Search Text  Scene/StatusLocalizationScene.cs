using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
	public class StatusLocalizationScene
    {
        public List<Text> Texts;
        public List<GameObject> Prefabs;
        public List<LocalizeStringEvent> LocalizeStringEvents;

        public StatusLocalizationScene()
		{
            Texts = new List<Text>();
            Prefabs = new List<GameObject>();
            LocalizeStringEvents = new List<LocalizeStringEvent>();
        }

        public override string ToString()
        {
            return String.Format(" Found text: {0} \n Found localize string event: {1} \n Prefabs: {2}",
                Texts.Count, LocalizeStringEvents.Count, Prefabs.Count);
        }
    }
}
