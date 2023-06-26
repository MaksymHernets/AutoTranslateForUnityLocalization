using System.Collections.Generic;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
	public class SearchTextParameters
	{
		public bool SkipPrefab = false;
        public bool SkipVariantPrefab = false;
        public bool SkipEmptyText = false;
		public Dictionary<string, bool> Lists;
		public bool IsRemoveMiss_StringTable = false;
	}
}