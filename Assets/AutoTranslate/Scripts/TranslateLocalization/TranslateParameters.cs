using EqualchanceGames.Tools.GUIPro;
using System.Collections.Generic;

namespace EqualchanceGames.Tools.AutoTranslate
{
    public class TranslateParameters
    {
        public bool canOverrideWords = false;
        public bool canTranslateEmptyWords = false;
        public bool canTranslateSmartWords = true;
        public Dictionary<string, bool> IsTranslateStringTables = new Dictionary<string, bool>();

        public void FillDictinary(List<RowCheckList> rowCheckLists)
		{
            IsTranslateStringTables.Clear();
            foreach (RowCheckList rowCheckList in rowCheckLists)
			{
                IsTranslateStringTables.Add(rowCheckList.Name, rowCheckList.IsActive);
            }
		}
    }
}
