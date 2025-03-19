using System.Collections.Generic;
using UnityEngine.Localization;

namespace EqualchanceGames.Tools.AutoTranslate.Editor
{
	public class AddLocalizationParameters
    {
        public string NameTable;
        public bool IsSkipPrefab;
        public bool IsSkipVariantPrefab;
        public bool IsSkipEmptyText;
        public Dictionary<string, bool> Lists;
        public Locale SourceLocale;
    }
}
