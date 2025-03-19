using System.Collections.Generic;

namespace EqualchanceGames.Tools.AutoTranslate
{
	public class SearchTextParameters
	{
		public bool SkipPrefab = false;
        public bool SkipVariantPrefab = false;
        public bool SkipEmptyText = false;
		public Dictionary<string, bool> ListSearchComponents;
        public Dictionary<string, bool> ListSkipParentComponents;
        public bool IsRemoveMiss_StringTable = false;
	}
}