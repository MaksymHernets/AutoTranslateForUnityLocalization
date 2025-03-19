using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace EqualchanceGames.Tools.AutoTranslate
{
    public class TranslateData
    {
        public Locale selectedLocale;
        public List<Locale> locales;
        public List<SharedTableData> sharedtables;
        public List<StringTable> stringTables;
    }
}
