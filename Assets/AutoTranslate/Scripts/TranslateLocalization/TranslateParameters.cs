
using GoodTime.Tools.Helpers.GUIElements;
using System.Collections.Generic;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public class TranslateParameters
    {
        public bool canOverrideWords = true;
        public bool canTranslateEmptyWords = true;
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
