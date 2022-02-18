
using System.Collections.Generic;

namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public class TranslateParameters
    {
        public bool canOverrideWords = true;
        public bool canTranslateEmptyWords = true;
        public bool canTranslateSmartWords = true;
        public List<bool> canTranslateTableCollections = new List<bool>();
    }
}
