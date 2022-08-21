namespace GoodTime.HernetsMaksym.AutoTranslate
{
    public class TranslateStatus
    {
        public float progress = 0f;
        public string sharedTable = string.Empty;
        public string targetLanguageTable = string.Empty;

        public TranslateStatus(float progress, string sharedTable, string targetLanguageTable)
        {
            this.progress = progress;
            this.sharedTable = sharedTable;
            this.targetLanguageTable = targetLanguageTable;
        }
    }
}
