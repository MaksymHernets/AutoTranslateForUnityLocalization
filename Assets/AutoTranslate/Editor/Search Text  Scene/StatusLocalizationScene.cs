using System;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor
{
	public class StatusLocalizationScene
    {
        public int CountText = 0;
        public int CountPrefabs = 0;
        public int CountTextLocalization = 0;

        public override string ToString()
        {
            return String.Format(" Found text: {0} \n Found localize string event: {1} \n Prefabs: {2}",
                CountText, CountTextLocalization, CountPrefabs);
        }
    }
}
