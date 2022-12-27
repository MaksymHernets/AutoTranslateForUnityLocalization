using System.Collections.Generic;
using UnityEngine.Localization;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
	public class AddLocalizationParameters
    {
        public string NameTable;
        public bool IsSkipPrefab;
        public bool IsSkipEmptyText;
        public Dictionary<string, bool> Lists;
        public Locale SourceLocale;
    }
}
