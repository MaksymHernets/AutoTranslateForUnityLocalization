using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public class StatusLocalizationScene
    {
        public List<Text> LegacyTexts;
        public List<Dropdown> LegacyDropdowns;

        public List<GameObject> Prefabs;
        public List<LocalizeStringEvent> LocalizeStringEvents;

        public List<TextMeshProUGUI> TextMeshs;
        public List<TMP_Dropdown> TMP_Dropdowns;

        public StatusLocalizationScene()
        {
            LegacyTexts = new List<Text>();
            LegacyDropdowns = new List<Dropdown>();
            Prefabs = new List<GameObject>();
            LocalizeStringEvents = new List<LocalizeStringEvent>();
            TextMeshs = new List<TextMeshProUGUI>();
            TMP_Dropdowns = new List<TMP_Dropdown>();
        }

        public override string ToString()
        {
            return String.Format(" Found:\n Text Legacy: {0}\n Localize string event: {1}\n Prefabs: {2}\n Dropdown Legacy: {3}\n TextMeshs: {4}\n TMP_Dropdowns: {5}",
                LegacyTexts.Count, LocalizeStringEvents.Count, Prefabs.Count, LegacyDropdowns.Count, TextMeshs.Count, TMP_Dropdowns.Count);
        }
    }
}
