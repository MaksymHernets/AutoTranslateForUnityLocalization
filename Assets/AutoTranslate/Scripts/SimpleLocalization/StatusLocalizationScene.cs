using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace EqualchanceGames.Tools.AutoTranslate
{
    public class StatusLocalizationScene
    {
        public List<Text> LegacyTexts;
        public List<TextMesh> LegacyMeshTexts;
        public List<TextMeshProUGUI> TextMeshProUIs;
        public List<TextMeshPro> TextMeshPros;

        public List<Dropdown> LegacyDropdowns;
        public List<TMP_Dropdown> TMP_Dropdowns;

        public List<GameObject> Prefabs;
        public List<GameObject> VariantPrefabs;
        public List<LocalizeStringEvent> LocalizeStringEvents;

        public StatusLocalizationScene()
        {
            LegacyTexts = new List<Text>();
            LegacyMeshTexts = new List<TextMesh>();
            TextMeshProUIs = new List<TextMeshProUGUI>();
            TextMeshPros = new List<TextMeshPro>();

            LegacyDropdowns = new List<Dropdown>();
            TMP_Dropdowns = new List<TMP_Dropdown>();

            Prefabs = new List<GameObject>();
            VariantPrefabs = new List<GameObject>();
            LocalizeStringEvents = new List<LocalizeStringEvent>();
        }

        public override string ToString()
        {
            return String.Format("Found:\nText Legacy: {0}\nText Mesh Legacy: {1}\nText Mesh Pro UI: {2}\nText Mesh Pro: {3}\nLocalize string event: {4}\nPrefabs: {5}\nVariant Prefabs: {6}",
                LegacyTexts.Count, LegacyMeshTexts.Count,
                TextMeshProUIs.Count, TextMeshPros.Count,
                LocalizeStringEvents.Count, Prefabs.Count, VariantPrefabs.Count);
        }
    }
}
